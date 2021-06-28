using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;
using MyOrm.Common;

namespace DAL
{

    public interface ISalaryChangeProjectService : IEntityService<SalaryChangeProject>, IEntityViewService<SalaryChangeProject>, IEntityService, IEntityViewService
    {
        [Transaction]
        void Insert(SalaryChange changes, List<SalaryChangeProject> list);
        [Transaction]
        void Update(SalaryChange changes, List<SalaryChangeProject> list);
    }

    public class SalaryChangeProjectService : ServiceBase<SalaryChangeProject, SalaryChangeProject>, ISalaryChangeProjectService
    {
        public void Insert(SalaryChange changes, List<SalaryChangeProject> list)
        {
            try
            {
                Factory.SalaryChangeService.Insert(changes);

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ChangeID = changes.ID;
                    SalaryBase sb = ServiceFactory.Factory.SalaryBaseService.SearchOne(new ConditionSet { new SimpleCondition("SalaryProjectID", list[i].SourceProjectID), new SimpleCondition("EmployeeID", changes.EmployeeID) });
                    if (sb != null)
                    {
                        list[i].SourceAmount = sb.SalaryAmount;
                        sb.SalaryAmount = list[i].SalaryAmount;
                        Factory.SalaryBaseService.Update(sb);
                    }
                }

                Factory.SalaryChangeProjectService.BatchInsert(list);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(SalaryChange changes, List<SalaryChangeProject> list)
        {
            try
            {
                Factory.SalaryChangeService.Update(changes);

                Factory.SalaryChangeProjectService.BatchDelete(Factory.SalaryChangeProjectService.Search(new SimpleCondition("ChangeID", changes.ID)));


                for (int i = 0; i < list.Count; i++)
                {
                    list[i].ChangeID = changes.ID;
                    SalaryBase sb = ServiceFactory.Factory.SalaryBaseService.SearchOne(new ConditionSet { new SimpleCondition("ID", list[i].SourceProjectID), new SimpleCondition("EmployeeID", changes.EmployeeID) });
                    if (sb != null)
                    {
                        list[i].SourceAmount = sb.SalaryAmount;
                        sb.SalaryAmount = list[i].SalaryAmount;
                        Factory.SalaryBaseService.Update(sb);
                    }
                }

                Factory.SalaryChangeProjectService.BatchInsert(list);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
