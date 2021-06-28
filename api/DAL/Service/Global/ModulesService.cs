
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DAL.Business;
using System.Data;
using MyOrm.Common;

namespace DAL
{
    public interface IModulesService : IEntityService<Modules>, IEntityViewService<Modules>, IEntityService, IEntityViewService
    {
        string GenBusiNo(string prefix, string format, int length);
        DataTable GetAll();
        DataTable GetAll(bool? hide);
        DataTable GetByUser(int userId);
        DataTable GetParentByUser(int userId);
        DataTable GetChildByUser(int moduleId, int userId);
        List<Modules> GetUpper();
        List<Modules> GetPage();
    }

    public class ModulesService : ServiceBase<Modules, Modules>, IModulesService
    {
        public string GenBusiNo(string prefix, string format, int length)
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GenBusiNo(prefix, format, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll()
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GetAll(null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll(bool? hide)
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GetAll(hide);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetByUser(int userId)
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GetByUserId(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetParentByUser(int userId)
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GetParentByUserId(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChildByUser(int moduleId, int userId)
        {
            try
            {
                return ((ModulesViewDAO)ObjectViewDAO).GetClientByUserId(moduleId, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Modules> GetUpper()
        {
            try
            {
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("Src", ConditionOperator.Equals, null));

                Sorting s = new Sorting();
                s.PropertyName = "Sequence";
                s.Direction = System.ComponentModel.ListSortDirection.Ascending;

                List<Modules> list = Factory.ModulesService.SearchWithOrder(condition, new Sorting[] { s });
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Modules> GetPage()
        {
            try
            {
                ConditionSet condition = new ConditionSet();
                condition.Add(new SimpleCondition("Src", ConditionOperator.Equals, null, true));
                condition.Add(new SimpleCondition("Visible", 1));

                Sorting s1 = new Sorting();
                s1.PropertyName = "UpperSequence";
                s1.Direction = System.ComponentModel.ListSortDirection.Ascending;

                Sorting s2 = new Sorting();
                s2.PropertyName = "UpperDisplayName";
                s2.Direction = System.ComponentModel.ListSortDirection.Ascending;

                Sorting s3 = new Sorting();
                s3.PropertyName = "Sequence";
                s3.Direction = System.ComponentModel.ListSortDirection.Ascending;
                List<Modules> list = Factory.ModulesService.SearchWithOrder(condition, new Sorting[] { s1, s2, s3 });
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}