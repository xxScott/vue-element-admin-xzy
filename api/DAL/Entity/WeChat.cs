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
    [Table("B_AccessToken")]
    [Description("微信接口令牌")]
    public class AccessToken : EntityBase
    {
        public string ParamType { get; set; }
        public string Access_Token { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    [Serializable]
    [Table("B_Account")]
    [Description("微信账户信息")]
    public class Account : BusinessEntity
    {
        public string OpenId { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string NickName { get; set; }
        public int? Sex { get; set; }
        public string HeadImgUrl { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
        public bool? Enabled { get; set; }
    }
}
