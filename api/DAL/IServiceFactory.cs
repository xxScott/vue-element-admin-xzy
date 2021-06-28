using System;
using System.Collections.Generic;
using System.Text;
using MyOrm.Common;

namespace DAL.Business
{
    public interface IServiceFactory
    {
        IUsersService UsersService { get; }
        IRolesService RolesService { get; }
        IUserInRoleService UserInRoleService { get; }
        IRightsService RightsService { get; }
        IModulesService ModulesService { get; }
        IParamsService ParamsService { get; }
        IParamsTypeService ParamsTypeService { get; }
        IVerifyCodesService VerifyCodesService { get; }

        IGroupsService GroupsService { get; }
        IStoresService StoresService { get; }
        ICompanyUsersService CompanyUsersService { get; }
        ICompanysService CompanysService { get; }
        IDeptsService DeptsService { get; }
        IFormationsService FormationsService { get; }
        IWorkStationsService WorkStationsService { get; }
        ISalaryProjectsService SalaryProjectsService { get; }
        IClassesService ClassesService { get; }
        IPeriodService PeriodService { get; }

        IEmployeeBaseService EmployeeBaseService { get; }
        IEmployeeRalationService EmployeeRalationService { get; }
        IEmployeeContractService EmployeeContractService { get; }
        IEmployeeChangeStationService EmployeeChangeStationService { get; }
        IEmployeeAwardService EmployeeAwardService { get; }
        IEmployeeLeaveService EmployeeLeaveService { get; }

        IMakeClassPlanService MakeClassPlanService { get; }
        IMakeClassDataService MakeClassDataService { get; }
        IMakeClassLogService MakeClassLogService { get; }

        ISalaryBaseService SalaryBaseService { get; }
        ISalaryChangeService SalaryChangeService { get; }
        ISalaryChangeProjectService SalaryChangeProjectService { get; }
        ISalaryAssessService SalaryAssessService { get; }
        ISalaryAssessDataService SalaryAssessDataService { get; }
        ISalarySettleService SalarySettleService { get; }

        IAccessTokenService AccessTokenService { get; }
        IAccountService AccountService { get; }

        IAttendStorageService AttendStorageService { get; }
        IAttendParamService AttendParamService { get; }
        IAttendPeriodDataService AttendPeriodDataService { get; }
        IBarCodeService BarCodeService { get; }
    }
}
