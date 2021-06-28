using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;
using log4net;
using MyOrm;
using System.Collections;
using DAL.Data;
using MyOrm.Common;
using System.Diagnostics;
using System.Data;

namespace DAL.Business
{
    public class FactoryProxyInterceptor : IInterceptor
    {
        private Dictionary<MethodInfo, object> services = new Dictionary<MethodInfo, object>();

        public void Intercept(IInvocation invocation)
        {
            if (!services.ContainsKey(invocation.Method))
            {
                lock (services)
                {
                    if (!services.ContainsKey(invocation.Method))
                    {
                        ProxyGenerator proxy = new ProxyGenerator();
                        string serviceName = invocation.Method.Name;
                        if (serviceName.StartsWith("get_")) serviceName = serviceName.Substring(4);
                        invocation.Proceed();
                        services[invocation.Method] = proxy.CreateInterfaceProxyWithTarget(invocation.Method.ReturnType, invocation.ReturnValue, new ServiceInterceptor(serviceName));
                    }
                }
            }
            invocation.ReturnValue = services[invocation.Method];
        }
    }

    public class FactoryInterceptor : IInterceptor
    {
        private Dictionary<MethodInfo, object> services = new Dictionary<MethodInfo, object>();

        public void Intercept(IInvocation invocation)
        {
            if (!services.ContainsKey(invocation.Method))
            {
                lock (services)
                {
                    if (!services.ContainsKey(invocation.Method))
                    {
                        services[invocation.Method] = ServiceBase.ServiceContainer.Resolve(invocation.Method.ReturnType);
                    }
                }
            }
            invocation.ReturnValue = services[invocation.Method];
        }
    }

    public class ServiceInterceptor : IInterceptor
    {
        private static readonly ILog logger = LogManager.GetLogger("ServiceInterceptor");
        public static readonly object SyncLock = new object();
        private Stopwatch timer = new Stopwatch();

        private Dictionary<MethodInfo, ServiceDescription> methodDescriptions = new Dictionary<MethodInfo, ServiceDescription>();

        public ServiceInterceptor(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                timer.Reset();
                timer.Start();
                Invoke(invocation);
                timer.Stop();
                LogInvoke(invocation, timer.Elapsed);
            }
            catch (Exception e)
            {
                LogException(invocation, e);
                throw;
            }

        }

        private void Invoke(IInvocation invocation)
        {
            ServiceDescription serviceDescription = GetDescription(invocation);

            lock (SyncLock)
            {
                if (serviceDescription.IsTransaction)
                {
                    SessionManager sessionManager = ServiceFactory.SessionManager;
                    if (sessionManager.Connection.State != ConnectionState.Open) sessionManager.Connection.Open();
                    try
                    {
                        sessionManager.BeginTransaction();
                        invocation.Proceed();
                        sessionManager.Commit();
                    }
                    catch
                    {
                        sessionManager.Rollback();
                        throw;
                    }
                }
                else
                {
                    invocation.Proceed();
                }
            }
        }


        private void LogException(IInvocation invocation, Exception e)
        {
            ServiceDescription serviceDescription = GetDescription(invocation);
            Exception innerExp = e;
            while (innerExp is TargetInvocationException && innerExp.InnerException != null)
            {
                innerExp = innerExp.InnerException;
            }
            string errorMsg = String.Format("<Exception>{0}.{1}{{{2}}}", ServiceName, serviceDescription.MethodName, GetLogString(GetLogArgs(invocation)));

            if (innerExp is CustomException)
            {
                logger.Warn(errorMsg + " - " + innerExp.Message);
            }
            else
            {
                logger.Error(errorMsg, innerExp);
            }
        }

        private string GetLogString(object[] args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object arg in args)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append(Util.GetLogString(arg, 1));
            }
            return sb.ToString();
        }

        private void LogInvoke(IInvocation invocation, TimeSpan elapsedTime)
        {
            ServiceDescription serviceDescription = GetDescription(invocation);

            switch (serviceDescription.LogLevel)
            {
                case LogLevel.Debug:
                    if (logger.IsDebugEnabled)
                    {
                        logger.InfoFormat("<Invoke>{0}.{1}{{{2}}}\r\n +{3} <Return>{4}", ServiceName, serviceDescription.MethodName,
                             (serviceDescription.LogFormat & LogFormat.Args) == LogFormat.Args ? GetLogString(GetLogArgs(invocation)) : null,
                            elapsedTime.TotalSeconds,
                            (serviceDescription.LogFormat & LogFormat.ReturnValue) == LogFormat.ReturnValue ? Util.GetLogString(invocation.ReturnValue, 0) : null);
                    }
                    break;
                case LogLevel.Info:
                    if (logger.IsInfoEnabled)
                    {
                        logger.InfoFormat("<Invoke>{0}.{1}{{{2}}}\r\n +{3} <Return>{4}", ServiceName, serviceDescription.MethodName,
                            (serviceDescription.LogFormat & LogFormat.Args) == LogFormat.Args ? GetLogString(GetLogArgs(invocation)) : null,
                            elapsedTime.TotalSeconds,
                            (serviceDescription.LogFormat & LogFormat.ReturnValue) == LogFormat.ReturnValue ? Util.GetLogString(invocation.ReturnValue, 0) : null);
                    }
                    break;
            }
        }

        private object[] GetLogArgs(IInvocation invocation)
        {
            ServiceDescription serviceDescription = GetDescription(invocation);
            object[] args = new object[invocation.Arguments.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (serviceDescription.ArgsLogable[i]) args[i] = invocation.Arguments[i];
                else args[i] = "***";
            }
            return args;
        }

        private ServiceDescription GetDescription(IInvocation invocation)
        {
            if (!methodDescriptions.ContainsKey(invocation.Method))
            {
                lock (methodDescriptions)
                {
                    if (!methodDescriptions.ContainsKey(invocation.Method))
                    {
                        ServiceDescription serviceDescription = new ServiceDescription(invocation.Method.Name);
                        ServiceLogAttribute serviceLogAtt = GetAttribute<ServiceLogAttribute>(invocation);
                        if (serviceLogAtt != null)
                        {
                            serviceDescription.LogFormat = serviceLogAtt.LogFormat;
                            serviceDescription.LogLevel = serviceLogAtt.LogLevel;
                        }

                        ServicePermissionAttribute permissionAtt = GetAttribute<ServicePermissionAttribute>(invocation);
                        if (permissionAtt != null)
                        {
                            serviceDescription.AllowAnonymous = permissionAtt.AllowAnonymous;
                            if (!String.IsNullOrEmpty(permissionAtt.AllowRoles)) serviceDescription.AllowRoles = permissionAtt.AllowRoles.Split(',');
                        }

                        TransactionAttribute transactionAttribute = GetAttribute<TransactionAttribute>(invocation);
                        if (transactionAttribute != null)
                        {
                            serviceDescription.IsTransaction = transactionAttribute.IsTransaction;
                        }

                        ServiceAttribute serviceAttribute = GetAttribute<ServiceAttribute>(invocation);
                        if (serviceAttribute != null)
                        {
                            serviceDescription.IsService = serviceAttribute.IsService;
                        }

                        ParameterInfo[] targetParameters = invocation.MethodInvocationTarget.GetParameters();
                        ParameterInfo[] parameters = invocation.Method.GetParameters();

                        bool[] argsLogable = new bool[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            LogAttribute[] logAtts = (LogAttribute[])targetParameters[i].GetCustomAttributes(typeof(LogAttribute), true);
                            if (logAtts.Length == 0) logAtts = (LogAttribute[])parameters[i].GetCustomAttributes(typeof(LogAttribute), true);
                            argsLogable[i] = logAtts.Length > 0 ? logAtts[0].Enabled : true;
                        }

                        serviceDescription.ArgsLogable = argsLogable;
                        methodDescriptions[invocation.Method] = serviceDescription;
                    }
                }
            }

            return methodDescriptions[invocation.Method];
        }

        private T GetAttribute<T>(IInvocation invocation) where T : Attribute
        {
            T att = null;
            if (invocation.InvocationTarget != null) att = Utility.GetAttribute<T>(invocation.MethodInvocationTarget);
            if (att == null) att = MyOrm.Common.Utility.GetAttribute<T>(invocation.Method);
            if (att == null && invocation.InvocationTarget != null) att = MyOrm.Common.Utility.GetAttribute<T>(invocation.TargetType);
            if (att == null && invocation.InvocationTarget != null) att = MyOrm.Common.Utility.GetAttribute<T>(invocation.MethodInvocationTarget.DeclaringType);
            if (att == null) att = MyOrm.Common.Utility.GetAttribute<T>(invocation.Method.DeclaringType);
            return att;
        }
    }
}
