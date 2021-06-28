using DAL.Data;
using MyOrm.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace DAL
{
    [Serializable]
    [Table("GLO_Role")]
    [Description("系统角色")]
    public class Roles : BusinessEntity
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Table("GLO_User")]
    [Description("系统用户")]
    public class Users : BusinessEntity
    {
        public string UID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string QQ { get; set; }
        public string WXID { get; set; }
        public string ShouQuanCode { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string KeyNum { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsChangePwd { get; set; }
        public DateTime? LastLogin { get; set; }
        public string NavigationUrl { get; set; }
        public int? GroupID { get; set; }
        public int? StoreID { get; set; }
    }

    [Serializable]
    [Table("GLO_Role_User")]
    [Description("系统用户隶属用户")]
    public class UserInRole : EntityBase
    {
        public int? RoleID { get; set; }
        public int? UserID { get; set; }
    }

    [Serializable]
    [Table("GLO_Right")]
    [Description("系统权限")]
    public class Rights : BusinessEntity
    {
        public int? RoleID { get; set; }
        public int? ModuleID { get; set; }

        public bool? HasAdd { get; set; }
        public bool? HasUpdate { get; set; }
        public bool? HasDelete { get; set; }
        public bool? HasSubmit { get; set; }
        public bool? HasQuery { get; set; }
    }

    [Serializable]
    [Table("GLO_Module")]
    [Description("功能菜单")]
    public class Modules : BusinessEntity
    {
        public string DisplayName { get; set; }
        [ForeignType(typeof(Modules))]
        public int? UpperModuleID { get; set; }
        public string Src { get; set; }
        public string ImageUrl { get; set; }
        public int? Sequence { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public bool? Hide { get; set; }

        public bool? RefreshFlag { get; set; }
        public bool? AddFlag { get; set; }
        public bool? UpdateFlag { get; set; }
        public bool? DeleteFlag { get; set; }
        public bool? SubmitFlag { get; set; }
        public bool? QueryFlag { get; set; }
        public bool? KeywordFlag { get; set; }

        public string RefreshFunc { get; set; }
        public string AddFunc { get; set; }
        public string UpdateFunc { get; set; }
        public string DeleteFunc { get; set; }
        public string SubmitFunc { get; set; }
        public string QueryFunc { get; set; }

        public string RefreshIcon { get; set; }
        public string AddIcon { get; set; }
        public string UpdateIcon { get; set; }
        public string DeleteIcon { get; set; }
        public string SubmitIcon { get; set; }
        public string QueryIcon { get; set; }

        public string RefreshCaption { get; set; }
        public string AddCaption { get; set; }
        public string UpdateCaption { get; set; }
        public string DeleteCaption { get; set; }
        public string SubmitCaption { get; set; }
        public string QueryCaption { get; set; }

        public string Component { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Redirect { get; set; }
        public string AlwaysShow { get; set; }
        public string Hidden { get; set; }
        public string NoCache { get; set; }
        public string BreadCrumb { get; set; }
        public string Affix { get; set; }

    }

    [Serializable]
    [Table("GLO_ParamType")]
    [Description("系统参数分类")]
    public class ParamsType : EntityBase
    {
        public string ParamTypeCode { get; set; }
        public string ParamTypeName { get; set; }
    }

    [Serializable]
    [Table("GLO_Param")]
    [Description("系统参数")]
    public class Params : BusinessEntity
    {
        [ForeignType(typeof(ParamsType))]
        public int? ParamTypeID { get; set; }
        public string ParamCode { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public int? Sequence { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    [Description("系统参数视图")]
    public class ParamsView : Params
    {
        [ForeignColumn(typeof(ParamsType))]
        public string ParamTypeCode { get; set; }
        [ForeignColumn(typeof(ParamsType))]
        public string ParamTypeName { get; set; }
    }

    [Serializable]
    [Table("GLO_VerifyCode")]
    [Description("短信验证码信息")]
    public class VerifyCodes : EntityBase
    {
        public string MobilePhone { get; set; }
        public string VerifyCode { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public int? State { get; set; }
    }
}