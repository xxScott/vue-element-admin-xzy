<%@ WebHandler Language="C#" Class="AjaxHandler" %>

using System;
using System.Web;
using System.IO;
using System.Threading;
using Com.Caimomo.Common;
using System.Collections.Specialized;

public class AjaxHandler : IHttpAsyncHandler
{
    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
    {
        context.Response.ContentType = "application/json";
        AsyncRequestResultE reqState = new AsyncRequestResultE(context, cb);
        ProcessDelegate prg = new ProcessDelegate(ProcessHandler);
        prg.BeginInvoke(reqState, null, null);

        return reqState;
    }

    public void EndProcessRequest(IAsyncResult result)
    {
        AsyncRequestResultE ars = result as AsyncRequestResultE;
        if (ars != null)
        {
            ars.Context.Response.Write(result.AsyncState);
        }
    }

    public void ProcessHandler(AsyncRequestResultE aresult)
    {
        HttpContext context = aresult.Context;
        HttpContext.Current = context;

        HandleResult result = null;
        string method = context.Request.QueryString["methodName"];
        string oldString = context.Request.QueryString["oldString"];
        string jsonp = context.Request.QueryString["jsonp"];
        string callbackFunName = context.Request.QueryString["callbackparam"];

        try
        {
            Stream inputStream = context.Request.GetBufferlessInputStream();
            StreamReader reader = new StreamReader(inputStream);
            string content = reader.ReadToEnd();

            if (!string.IsNullOrEmpty(method))
            {
                IAjaxHandler handler = AjaxHandlerFactory.GetHandler(method);
                result = handler.HandleRequest(context, context.Request.QueryString, content);
            }
            else
            {
                result = new HandleResult { ResultCode = 1, Message = "找不到指定的方法", Data = "找不到指定的方法" };
            }
        }
        catch (Exception e)
        {
            Log.WriteLog(e.Message + "\r\n" + e.StackTrace);
            result = new HandleResult { ResultCode = 1, Message = e.Message, Data = e.Message };
        }

        if ("1".Equals(oldString))
        {
            if (!result.Data.GetType().Equals(typeof(String)))
                aresult.AsyncState = Newtonsoft.Json.JsonConvert.SerializeObject(result.Data);
            else
                aresult.AsyncState = result.Data;
        }
        else
            aresult.AsyncState = Newtonsoft.Json.JsonConvert.SerializeObject(result);

        if ("1".Equals(jsonp))
            aresult.AsyncState = getJsonp("1".Equals(oldString) ? result.Data : result, callbackFunName);

        aresult.CompleteRequest();
    }

    string getJsonp(object data, string callbackFunName)
    {
        if (string.IsNullOrEmpty(callbackFunName))
            return data as string;
        else
        {
            if (data.GetType().Equals(typeof(String)))
                return callbackFunName + "({result:\"" + data + "\"})";
            else
                return callbackFunName + "(" + Newtonsoft.Json.JsonConvert.SerializeObject(data) + ")";
        }
    }

    public void ProcessRequest(HttpContext context)
    {
    }

    public bool IsReusable { get { return false; } }

    delegate void ProcessDelegate(AsyncRequestResultE state);
}



public class AsyncRequestResultE : IAsyncResult
{
    public AsyncRequestResultE(HttpContext ctx, AsyncCallback cb)
    {
        _ctx = ctx;
        _cb = cb;
    }

    private HttpContext _ctx;
    private AsyncCallback _cb;
    private bool _isCompleted = false;
    private ManualResetEvent _callCompleteEvent = null;

    internal void CompleteRequest()
    {
        _isCompleted = true;
        lock (this)
        {
            if (_callCompleteEvent != null)
                _callCompleteEvent.Set();
        }
        if (_cb != null)
            _cb(this);
    }

    public object AsyncState { get; set; }
    public bool CompletedSynchronously { get { return (false); } }
    public bool IsCompleted { get { return (_isCompleted); } }
    public HttpContext Context { get { return _ctx; } }

    public WaitHandle AsyncWaitHandle
    {
        get
        {
            lock (this)
            {
                if (_callCompleteEvent == null)
                    _callCompleteEvent = new ManualResetEvent(false);

                return _callCompleteEvent;
            }
        }
    }
}