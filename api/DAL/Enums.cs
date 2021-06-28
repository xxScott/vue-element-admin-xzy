using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL
{
    /// <summary>
    /// 错误码定义
    /// </summary>
    public enum Enum_StatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [EnumDisplayName("成功")]
        Success = 0,

        /// <summary>
        /// 必填项为空
        /// </summary>
        [EnumDisplayName("必填项为空")]
        Error_Required = 1001,

        /// <summary>
        /// 日期格式错误
        /// </summary>
        [EnumDisplayName("日期格式错误")]
        Error_DateFormatError = 1002,

        /// <summary>
        /// 数值格式错误
        /// </summary>
        [EnumDisplayName("数值格式错误")]
        Error_NumberFormatError = 1003,

        /// <summary>
        /// 数据不存在
        /// </summary>
        [EnumDisplayName("数据不存在")]
        Error_DataNotExist = 1004,

        /// <summary>
        /// AppKey错误
        /// </summary>
        [EnumDisplayName("AppKey错误")]
        Error_AppKey = 2001,

        /// <summary>
        /// LoginCode错误
        /// </summary>
        [EnumDisplayName("LoginCode错误")]
        Error_LoginCode = 2002,

        /// <summary>
        /// KeyNum错误
        /// </summary>
        [EnumDisplayName("KeyNum错误")]
        Error_KeyNum = 2003,

        /// <summary>
        /// 超期
        /// </summary>
        [EnumDisplayName("超期")]
        Error_Expired = 3001,

        /// <summary>
        /// 签名错误
        /// </summary>
        [EnumDisplayName("签名错误")]
        Error_Token = 3002,

        /// <summary>
        /// 文件上传失败
        /// </summary>
        [EnumDisplayName("文件上传失败")]
        Error_FileUploadFailed = 9006,



        /// <summary>
        /// 访问被拒绝
        /// </summary>
        [EnumDisplayName("访问被拒绝")]
        Error_AccessDeny = 9007,

        /// <summary>
        /// 请求参数异常
        /// </summary>
        [EnumDisplayName("请求参数异常")]
        Error_Request = 9008,

        /// <summary>
        /// 失败
        /// </summary>
        [EnumDisplayName("失败")]
        Fail = 9009
    }

    /// <summary>
    /// 数据结构（适配）属性
    /// </summary>
    public enum Enum_Adapter
    {
        /// <summary>
        /// 无
        /// </summary>
        [EnumDisplayName("无")]
        None = 1,

        /// <summary>
        /// 树形列表结构
        /// </summary>
        [EnumDisplayName("树形列表结构")]
        TreeGrid = 2,

        /// <summary>
        /// 树形下拉框结构
        /// </summary>
        [EnumDisplayName("树形下拉框结构")]
        ComboTree = 3,

        /// <summary>
        /// 数据列表结构
        /// </summary>
        [EnumDisplayName("数据列表结构")]
        DataList = 4,

        /// <summary>
        /// 树形结构
        /// </summary>
        [EnumDisplayName("树形结构")]
        Tree = 5,

        /// <summary>
        /// 下拉框
        /// </summary>
        [EnumDisplayName("下拉框")]
        Combobox = 6
    }


    /// <summary>
    /// 休息方式
    /// </summary>
    public enum Enum_RestType
    {
        /// <summary>
        /// 按周休
        /// </summary>
        [EnumDisplayName("按周休")]
        Week = 1,

        /// <summary>
        /// 按月休
        /// </summary>
        [EnumDisplayName("按月休")]
        Month = 2,

        /// <summary>
        /// 全年无休
        /// </summary>
        [EnumDisplayName("全年无休")]
        NoRestYearround = 3,

    }

    /// <summary>
    /// 薪资项目属性
    /// </summary>
    public enum Enum_SalaryProjectProperty
    {
        /// <summary>
        /// 累加项目
        /// </summary>
        [EnumDisplayName("累加项目")]
        Increase = 1,

        /// <summary>
        /// 扣款项目
        /// </summary>
        [EnumDisplayName("扣款项目")]
        Deduct = 2
    }


    /// <summary>
    /// 薪资项目来源
    /// </summary>
    public enum Enum_SalaryProjectSource
    {
        /// <summary>
        /// 预设
        /// </summary>
        [EnumDisplayName("预设")]
        Setting = 1,

        /// <summary>
        /// 填写
        /// </summary>
        [EnumDisplayName("填写")]
        Input = 2,

        /// <summary>
        /// 选择
        /// </summary>
        [EnumDisplayName("选择")]
        Select = 3,

        /// <summary>
        /// 自动
        /// </summary>
        [EnumDisplayName("自动")]
        Auto = 4
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Enum_Sex
    {
        /// <summary>
        /// 男
        /// </summary>
        [EnumDisplayName("男")]
        Male = 1,

        /// <summary>
        /// 女
        /// </summary>
        [EnumDisplayName("女")]
        Female = 2,
    }

    /// <summary>
    /// 民族
    /// </summary>
    public enum Enum_Nation
    {
        /// <summary>
        /// 汉
        /// </summary>
        [EnumDisplayName("汉")]
        Han = 1,

        /// <summary>
        /// 少数民族
        /// </summary>
        [EnumDisplayName("少数民族")]
        Other = 2,
    }

    /// <summary>
    /// 政治面貌
    /// </summary>
    public enum Enum_PoliticalStatus
    {
        /// <summary>
        /// 群众
        /// </summary>
        [EnumDisplayName("群众")]
        Masses = 1,

        /// <summary>
        /// 党员
        /// </summary>
        [EnumDisplayName("党员")]
        Party = 2,
    }

    /// <summary>
    /// 最高学历
    /// </summary>
    public enum Enum_EducationDegree
    {
        /// <summary>
        /// 初中
        /// </summary>
        [EnumDisplayName("初中")]
        Junion = 1,

        /// <summary>
        /// 高中
        /// </summary>
        [EnumDisplayName("高中")]
        Senior = 2,
    }

    /// <summary>
    /// 婚姻状况
    /// </summary>
    public enum Enum_MarriageStatus
    {
        /// <summary>
        /// 未婚
        /// </summary>
        [EnumDisplayName("未婚")]
        Unmarried = 1,

        /// <summary>
        /// 已婚
        /// </summary>
        [EnumDisplayName("已婚")]
        Married = 2,
    }

    /// <summary>
    /// 员工信息状态
    /// </summary>
    public enum Enum_EmployeeState
    {
        /// <summary>
        /// 预录
        /// </summary>
        [EnumDisplayName("预录")]
        Ready = -9999,
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumDisplayName("待审核")]
        Wait = -1,
        /// <summary>
        /// 审核通过
        /// </summary>
        [EnumDisplayName("审核通过")]
        Pass = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [EnumDisplayName("不合格")]
        Fail = 0,

    }

    /// <summary>
    /// 员工合同类型
    /// </summary>
    public enum Enum_ContractType
    {
        /// <summary>
        /// 无
        /// </summary>
        [EnumDisplayName("无")]
        Nothing = 0,
        /// <summary>
        /// 劳动合同
        /// </summary>
        [EnumDisplayName("劳动合同")]
        Labor = 1,
        /// <summary>
        /// 兼职合同
        /// </summary>
        [EnumDisplayName("兼职合同")]
        PartJob = 2,
        /// <summary>
        /// 不合格
        /// </summary>
        [EnumDisplayName("实习合同")]
        Practice = 3,
        /// <summary>
        /// 派遣合同
        /// </summary>
        [EnumDisplayName("派遣合同")]
        Dispatch = 4,
        /// <summary>
        /// 劳动合同
        /// </summary>
        [EnumDisplayName("特约合同")]
        Special = 5,
        /// <summary>
        /// 假期合同
        /// </summary>
        [EnumDisplayName("假期合同")]
        Holiday = 6
    }


    /// <summary>
    /// 员工奖惩类型
    /// </summary>
    public enum Enum_AwardType
    {
        /// <summary>
        /// 嘉奖
        /// </summary>
        [EnumDisplayName("嘉奖")]
        Cite = 0,
        /// <summary>
        /// 记小功
        /// </summary>
        [EnumDisplayName("记小功")]
        MinCite = 1,
        /// <summary>
        /// 记大功
        /// </summary>
        [EnumDisplayName("记大功")]
        MaxCite = 2,
        /// <summary>
        /// 晋升
        /// </summary>
        [EnumDisplayName("晋升")]
        Promote = 3,
        /// <summary>
        /// 警告
        /// </summary>
        [EnumDisplayName("警告")]
        Warning = 4,
        /// <summary>
        /// 记小过
        /// </summary>
        [EnumDisplayName("记小过")]
        MinFault = 5,
        /// <summary>
        /// 记大过
        /// </summary>
        [EnumDisplayName("记大过")]
        MaxFault = 6,
        /// <summary>
        /// 辞退
        /// </summary>
        [EnumDisplayName("辞退")]
        Dismiss = -1
    }

    /// <summary>
    /// 申请离职状态
    /// </summary>
    public enum Enum_LeavelStatus
    {
        /// <summary>
        /// 在职
        /// </summary>
        [EnumDisplayName("在职")]
        BeOnJob = 0,
        /// <summary>
        /// 兼职
        /// </summary>
        [EnumDisplayName("兼职")]
        PartJob = 1,
        /// <summary>
        /// 离职
        /// </summary>
        [EnumDisplayName("离职")]
        LeaveJob = 2,
        /// <summary>
        /// 自离
        /// </summary>
        [EnumDisplayName("自离")]
        OneSelf = 3,
        /// <summary>
        /// 申请离职
        /// </summary>
        [EnumDisplayName("申请离职")]
        Applyfor = 4,
        /// <summary>
        /// 开除
        /// </summary>
        [EnumDisplayName("开除")]
        Dismiss = 5,
        /// <summary>
        /// 待离职
        /// </summary>
        [EnumDisplayName("待离职")]
        Wait = 6

    }

    /// <summary>
    /// 离职信息状态
    /// </summary>
    public enum Enum_LevelState
    {
        /// <summary>
        /// 预录
        /// </summary>
        [EnumDisplayName("预录")]
        Ready = -9999,
        /// <summary>
        /// 等待审核
        /// </summary>
        [EnumDisplayName("等待审核")]
        Wait = -1,
        /// <summary>
        /// 通过
        /// </summary>
        [EnumDisplayName("审核通过")]
        Pass = 1,
        /// <summary>
        /// 审核拒绝
        /// </summary>
        [EnumDisplayName("审核拒绝")]
        Reject = 0,
    }

    /// <summary>
    /// 排班信息状态
    /// </summary>
    public enum Enum_MakeClassState
    {
        /// <summary>
        /// 待排班
        /// </summary>
        [EnumDisplayName("待排班")]
        Plan = -9999,
        /// <summary>
        /// 已排班
        /// </summary>
        [EnumDisplayName("已排班")]
        MakeOver = 9999,
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumDisplayName("待审核")]
        Wait = -1,
        /// <summary>
        /// 通过
        /// </summary>
        [EnumDisplayName("审核通过")]
        Pass = 1,
        /// <summary>
        /// 审核拒绝
        /// </summary>
        [EnumDisplayName("审核拒绝")]
        Reject = 0,
    }
    /// <summary>
    /// 考评信息状态
    /// </summary>
    public enum Enum_AssessState
    {
        /// <summary>
        /// 已考评
        /// </summary>
        [EnumDisplayName("已考评")]
        Plan = -9999,
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumDisplayName("待审核")]
        Wait = -1,
        /// <summary>
        /// 通过
        /// </summary>
        [EnumDisplayName("审核通过")]
        Pass = 1,
        /// <summary>
        /// 审核拒绝
        /// </summary>
        [EnumDisplayName("审核拒绝")]
        Reject = 0,
    }

    /// <summary>
    /// 考勤参数
    /// </summary>
    public enum Enum_AttendenceParameter
    {
        /// <summary>
        /// 迟到
        /// </summary>
        [EnumDisplayName("迟到")]
        Late = 1,

        /// <summary>
        /// 早退
        /// </summary>
        [EnumDisplayName("早退")]
        Earlier = 2,

        /// <summary>
        /// 旷工
        /// </summary>
        [EnumDisplayName("旷工")]
        StayAway = 3
    }

    /// <summary>
    /// 考勤参数
    /// </summary>
    public enum Enum_ParamType
    {
        /// <summary>
        /// 允许迟到最大分钟数
        /// </summary>
        [EnumDisplayName("允许迟到最大分钟数")]
        LateMaxMinutes = 1,

        /// <summary>
        /// 允许早退最大分钟数
        /// </summary>
        [EnumDisplayName("允许早退最大分钟数")]
        EarlierMaxMinutes = 2,

        /// <summary>
        /// 存储方式
        /// </summary>
        [EnumDisplayName("存储方式")]
        StorageMode = 3,

        /// <summary>
        /// 数据库地址
        /// </summary>
        [EnumDisplayName("数据库地址")]
        Server = 4,

        /// <summary>
        /// 数据库用户
        /// </summary>
        [EnumDisplayName("数据库用户")]
        UserId = 5,

        /// <summary>
        /// 数据库密码
        /// </summary>
        [EnumDisplayName("数据库密码")]
        UserPwd = 6,
    }
    /// <summary>
    /// 考勤数据类型
    /// </summary>
    public enum Enum_StorageType
    {
        /// <summary>
        /// 月份
        /// </summary>
        [EnumDisplayName("月份")]
        Month = 1,

        /// <summary>
        /// 年份
        /// </summary>
        [EnumDisplayName("年份")]
        Year = 2
    }

    /// <summary>
    /// 考勤数据存储模式
    /// </summary>
    public enum Enum_StorageMode
    {
        /// <summary>
        /// SQLSERVER数据库
        /// </summary>
        [EnumDisplayName("SQLSERVER数据库")]
        SqlServer = 1,

        /// <summary>
        /// 文件
        /// </summary>
        [EnumDisplayName("文件")]
        File = 2
    }

    /// <summary>
    /// 考勤二维码数据类型
    /// </summary>
    public enum Enum_StartOrOver
    {
        /// <summary>
        /// 上班
        /// </summary>
        [EnumDisplayName("上班")]
        Start = 1,

        /// <summary>
        /// 下班
        /// </summary>
        [EnumDisplayName("下班")]
        Over = -1
    }

    /// <summary>
    /// 验证码状态
    /// </summary>
    public enum Enum_VerifyCodeState
    {
        /// <summary>
        /// 未使用
        /// </summary>
        [EnumDisplayName("未使用")]
        Unused = 0,

        /// <summary>
        /// 已使用
        /// </summary>
        [EnumDisplayName("已使用")]
        Used = 1
    }
    /// <summary>
    /// 签到状态
    /// </summary>
    public enum Enum_AttendStatus
    {
        /// <summary>
        /// 迟到
        /// </summary>
        [EnumDisplayName("迟到")]
        Late = 0,

        /// <summary>
        /// 早退
        /// </summary>
        [EnumDisplayName("早退")]
        LeaveEarly = 1,

        /// <summary>
        /// 旷工
        /// </summary>
        [EnumDisplayName("旷工")]
        Absenteeism = 2,

        /// <summary>
        /// 休息
        /// </summary>
        [EnumDisplayName("休息")]
        Rest = 3,

        /// <summary>
        /// 迟到+早退
        /// </summary>
        [EnumDisplayName("迟到+早退")]
        LateAndLeave = 4,

        /// <summary>
        /// 异常
        /// </summary>
        [EnumDisplayName("异常")]
        Exception = -1,
        /// <summary>
        /// 正常
        /// </summary>
        [EnumDisplayName("正常")]
        Normal = 9,
        /// <summary>
        /// 未开始
        /// </summary>
        [EnumDisplayName("未开始")]
        Wait = -9,

    }
}
