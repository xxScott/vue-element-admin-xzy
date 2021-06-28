using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Business;

namespace DAL
{

    public interface ISalaryAssessService : IEntityService<SalaryAssess>, IEntityViewService<SalaryAssess>, IEntityService, IEntityViewService
    {
        DataTable QueryEmp(int storeId, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount);

        DataTable QueryAssess(int storeId, int state, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount);
        [Transaction]
        bool SaveAssess(SalaryAssess assess, SalaryAssessData data);
        [Transaction]
        bool UpdateAssess(SalaryAssess assess, SalaryAssessData data);


    }

    public class SalaryAssessService : ServiceBase<SalaryAssess, SalaryAssess>, ISalaryAssessService
    {

        public DataTable QueryEmp(int storeId, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            try
            {
                return ((SalaryAssessViewDAO)ObjectViewDAO).QueryEmp(storeId, peroidId, keyword, pageNumber, pageSize, out totalCount);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable QueryAssess(int storeId, int state, int peroidId, string keyword, int? pageNumber, int? pageSize, out int totalCount)
        {
            try
            {
                return ((SalaryAssessViewDAO)ObjectViewDAO).QueryAssess(storeId, state, peroidId, keyword, pageNumber, pageSize, out totalCount);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveAssess(SalaryAssess assess, SalaryAssessData data)
        {
            try
            {
                Factory.SalaryAssessService.Insert(assess);

                data.AssessID = assess.ID;

                Factory.SalaryAssessDataService.Insert(data);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateAssess(SalaryAssess assess, SalaryAssessData data)
        {
            try
            {
                Factory.SalaryAssessService.UpdateOrInsert(assess);

                data.AssessID = assess.ID;
                Factory.SalaryAssessDataService.UpdateOrInsert(data);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
