using Adapters;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using Newtonsoft.Json.Linq;

namespace HrApp.Api.Global
{
    public class EnumController : Controller
    {
        public JsonResult Query(string enumType)
        {
            CustomJsonResult json = new CustomJsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.ContentType = "text/plain";
            try
            {
                if (enumType.IndexOf("|") < 0)
                {
                    if (enumType.ToUpper() == "SalaryProjectProperty".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_SalaryProjectProperty)));
                    }
                    else if (enumType.ToUpper() == "SalaryProjectSource".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_SalaryProjectSource)));
                    }
                    else if (enumType.ToUpper() == "Sex".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_Sex)));
                    }
                    else if (enumType.ToUpper() == "Nation".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_Nation)));
                    }
                    else if (enumType.ToUpper() == "PoliticalStatus".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_PoliticalStatus)));
                    }
                    else if (enumType.ToUpper() == "EducationDegree".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_EducationDegree)));
                    }
                    else if (enumType.ToUpper() == "MarriageStatus".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_MarriageStatus)));
                    }
                    else if (enumType.ToUpper() == "ContractType".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_ContractType)));
                    }
                    else if (enumType.ToUpper() == "AwardType".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_AwardType)));

                    }
                    else if (enumType.ToUpper() == "LeaveStatus".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_LeavelStatus)));

                    }
                    else if (enumType.ToUpper() == "RestType".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_RestType)));

                    }
                    else if (enumType.ToUpper() == "AttendStatus".ToUpper())
                    {
                        json.Data = JsonUtil.GetSuccessForObject(new ComboboxAdapter().GetEnum(typeof(DAL.Enum_AttendStatus)));

                    }
                    else
                    {
                        json.Data = JsonUtil.GetFail(DAL.Enum_StatusCode.Error_DataNotExist);
                    }
                }
                else
                {
                    JObject data = new JObject();
                    string[] _enumType = enumType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < _enumType.Length; i++)
                    {
                        if (_enumType[i].ToUpper() == "SalaryProjectProperty".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_SalaryProjectProperty));
                        }
                        else if (_enumType[i].ToUpper() == "SalaryProjectSource".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_SalaryProjectSource));
                        }
                        else if (_enumType[i].ToUpper() == "Sex".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_Sex));
                        }
                        else if (_enumType[i].ToUpper() == "Nation".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_Nation));
                        }
                        else if (_enumType[i].ToUpper() == "PoliticalStatus".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_PoliticalStatus));
                        }
                        else if (_enumType[i].ToUpper() == "EducationDegree".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_EducationDegree));
                        }
                        else if (_enumType[i].ToUpper() == "MarriageStatus".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_MarriageStatus));
                        }
                        else if (_enumType[i].ToUpper() == "ContractType".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_ContractType));
                        }
                        else if (_enumType[i].ToUpper() == "AwardType".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_AwardType));
                        }
                        else if (_enumType[i].ToUpper() == "LeaveStatus".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_LeavelStatus));
                        }
                        else if (_enumType[i].ToUpper() == "RestType".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_RestType));
                        }
                        else if (_enumType[i].ToUpper() == "AttendStatus".ToUpper())
                        {
                            data[_enumType[i]] = new ComboboxAdapter().GetEnum(typeof(DAL.Enum_AttendStatus));
                        }

                    }
                    json.Data = JsonUtil.GetSuccessForObject(data);
                }
            }
            catch (Exception ex)
            {
                json.Data = JsonUtil.GetFailForString(ex.Message);
            }
            return json;
        }
    }
}