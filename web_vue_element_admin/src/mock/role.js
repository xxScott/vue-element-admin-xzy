import Mock from 'mockjs'
import { deepClone } from '@/utils'
import { filterAsyncRoutes } from '@/store/modules/permission'
import { constantRoutes } from '@/router'
import { menuListToMock } from '@/mock/menuList'

const routes = deepClone([...constantRoutes])

const rolesList = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 4,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [
      {
        roleId: 8,
        roleName: 'asd',
        remark: '1',
        createUserId: 1,
        menuIdList: null,
        createTime: '2019-03-15 17:16:14'
      },
      {
        roleId: 9,
        roleName: '1',
        remark: '1',
        createUserId: 1,
        menuIdList: null,
        createTime: '2019-03-20 12:29:19'
      },
      {
        roleId: 10,
        roleName: 'test',
        remark: 'test',
        createUserId: 1,
        menuIdList: null,
        createTime: '2019-03-20 16:10:06'
      },
      {
        roleId: 11,
        roleName: '业务测试',
        remark: null,
        createUserId: 1,
        menuIdList: null,
        createTime: '2019-03-23 14:30:27'
      }
    ]
  }
}

const treeData = [
  {
    menuId: 1,
    parentId: 0,
    parentName: null,
    name: '系统管理',
    url: null,
    perms: null,
    type: 0,
    icon: 'fa fa-cog',
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 2,
    parentId: 1,
    parentName: '系统管理',
    name: '管理员列表',
    url: 'modules/sys/user.html',
    perms: null,
    type: 1,
    icon: 'fa fa-user',
    orderNum: 1,
    open: null,
    list: null
  },
  {
    menuId: 3,
    parentId: 1,
    parentName: '系统管理',
    name: '角色管理',
    url: 'modules/sys/role.html',
    perms: null,
    type: 1,
    icon: 'fa fa-user-secret',
    orderNum: 2,
    open: null,
    list: null
  },
  {
    menuId: 4,
    parentId: 1,
    parentName: '系统管理',
    name: '菜单管理',
    url: 'modules/sys/menu.html',
    perms: null,
    type: 1,
    icon: 'fa fa-th-list',
    orderNum: 3,
    open: null,
    list: null
  },
  {
    menuId: 5,
    parentId: 1,
    parentName: '系统管理',
    name: 'SQL监控',
    url: 'druid/sql.html',
    perms: null,
    type: 1,
    icon: 'fa fa-bug',
    orderNum: 4,
    open: null,
    list: null
  },
  {
    menuId: 6,
    parentId: 1,
    parentName: '系统管理',
    name: '定时任务',
    url: 'modules/job/schedule.html',
    perms: null,
    type: 1,
    icon: 'fa fa-tasks',
    orderNum: 5,
    open: null,
    list: null
  },
  {
    menuId: 7,
    parentId: 6,
    parentName: '定时任务',
    name: '查看',
    url: null,
    perms: 'sys:schedule:list,sys:schedule:info',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 8,
    parentId: 6,
    parentName: '定时任务',
    name: '新增',
    url: null,
    perms: 'sys:schedule:save',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 9,
    parentId: 6,
    parentName: '定时任务',
    name: '修改',
    url: null,
    perms: 'sys:schedule:update',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 10,
    parentId: 6,
    parentName: '定时任务',
    name: '删除',
    url: null,
    perms: 'sys:schedule:delete',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 11,
    parentId: 6,
    parentName: '定时任务',
    name: '暂停',
    url: null,
    perms: 'sys:schedule:pause',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 12,
    parentId: 6,
    parentName: '定时任务',
    name: '恢复',
    url: null,
    perms: 'sys:schedule:resume',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 13,
    parentId: 6,
    parentName: '定时任务',
    name: '立即执行',
    url: null,
    perms: 'sys:schedule:run',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 14,
    parentId: 6,
    parentName: '定时任务',
    name: '日志列表',
    url: null,
    perms: 'sys:schedule:log',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 15,
    parentId: 2,
    parentName: '管理员列表',
    name: '查看',
    url: null,
    perms: 'sys:user:list,sys:user:info',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 16,
    parentId: 2,
    parentName: '管理员列表',
    name: '新增',
    url: null,
    perms: 'sys:user:save,sys:role:select',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 17,
    parentId: 2,
    parentName: '管理员列表',
    name: '修改',
    url: null,
    perms: 'sys:user:update,sys:role:select',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 18,
    parentId: 2,
    parentName: '管理员列表',
    name: '删除',
    url: null,
    perms: 'sys:user:delete',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 19,
    parentId: 3,
    parentName: '角色管理',
    name: '查看',
    url: null,
    perms: 'sys:role:list,sys:role:info',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 20,
    parentId: 3,
    parentName: '角色管理',
    name: '新增',
    url: null,
    perms: 'sys:role:save,sys:menu:list',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 21,
    parentId: 3,
    parentName: '角色管理',
    name: '修改',
    url: null,
    perms: 'sys:role:update,sys:menu:list',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 22,
    parentId: 3,
    parentName: '角色管理',
    name: '删除',
    url: null,
    perms: 'sys:role:delete',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 23,
    parentId: 4,
    parentName: '菜单管理',
    name: '查看',
    url: null,
    perms: 'sys:menu:list,sys:menu:info',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 24,
    parentId: 4,
    parentName: '菜单管理',
    name: '新增',
    url: null,
    perms: 'sys:menu:save,sys:menu:select',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 25,
    parentId: 4,
    parentName: '菜单管理',
    name: '修改',
    url: null,
    perms: 'sys:menu:update,sys:menu:select',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 26,
    parentId: 4,
    parentName: '菜单管理',
    name: '删除',
    url: null,
    perms: 'sys:menu:delete',
    type: 2,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 27,
    parentId: 1,
    parentName: '系统管理',
    name: '参数管理',
    url: 'modules/sys/config.html',
    perms: 'sys:config:list,sys:config:info,sys:config:save,sys:config:update,sys:config:delete',
    type: 1,
    icon: 'fa fa-sun-o',
    orderNum: 6,
    open: null,
    list: null
  },
  {
    menuId: 29,
    parentId: 1,
    parentName: '系统管理',
    name: '系统日志',
    url: 'modules/sys/log.html',
    perms: 'sys:log:list',
    type: 1,
    icon: 'fa fa-file-text-o',
    orderNum: 7,
    open: null,
    list: null
  },
  {
    menuId: 30,
    parentId: 1,
    parentName: '系统管理',
    name: '文件上传',
    url: 'modules/oss/oss.html',
    perms: 'sys:oss:all',
    type: 1,
    icon: 'fa fa-file-image-o',
    orderNum: 6,
    open: null,
    list: null
  },
  {
    menuId: 31,
    parentId: 0,
    parentName: null,
    name: '网站',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 32,
    parentId: 31,
    parentName: '网站',
    name: '栏目管理',
    url: 'modules/bids/programa.html',
    perms: null,
    type: 1,
    icon: 'fa fa-th-list',
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 33,
    parentId: 31,
    parentName: '网站',
    name: '内容管理',
    url: 'modules/bids/content.html',
    perms: null,
    type: 1,
    icon: 'fa fa-th-list',
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 34,
    parentId: 31,
    parentName: '网站',
    name: '基本设置',
    url: 'modules/bids/basic.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 35,
    parentId: 0,
    parentName: null,
    name: '招标',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 36,
    parentId: 42,
    parentName: '合同',
    name: '合同管理',
    url: 'modules/server/agreement.html',
    perms: '#',
    type: 1,
    icon: 'fa fa-th-list',
    orderNum: 20,
    open: null,
    list: null
  },
  {
    menuId: 40,
    parentId: 44,
    parentName: '统计',
    name: '项目管理',
    url: 'modules/server/project.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 30,
    open: null,
    list: null
  },
  {
    menuId: 42,
    parentId: 0,
    parentName: null,
    name: '合同',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 1,
    open: null,
    list: null
  },
  {
    menuId: 43,
    parentId: 0,
    parentName: null,
    name: '监理业务',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 3,
    open: null,
    list: null
  },
  {
    menuId: 44,
    parentId: 0,
    parentName: null,
    name: '统计',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 4,
    open: null,
    list: null
  },
  {
    menuId: 45,
    parentId: 35,
    parentName: '招标',
    name: '项目信息',
    url: 'modules/zhaobiao/info.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 46,
    parentId: 35,
    parentName: '招标',
    name: '招标立项',
    url: 'modules/zhaobiao/lixiang.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 47,
    parentId: 35,
    parentName: '招标',
    name: '招标文件',
    url: 'modules/zhaobiao/file.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 48,
    parentId: 35,
    parentName: '招标',
    name: '公告发布',
    url: 'modules/zhaobiao/publish.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 49,
    parentId: 35,
    parentName: '招标',
    name: '开标记录',
    url: 'modules/zhaobiao/kbrecord.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 50,
    parentId: 35,
    parentName: '招标',
    name: '评标记录',
    url: 'modules/zhaobiao/pbrecord.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 51,
    parentId: 35,
    parentName: '招标',
    name: '公示发布',
    url: 'modules/zhaobiao/gspublish.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 52,
    parentId: 35,
    parentName: '招标',
    name: '中标通知书',
    url: 'modules/zhaobiao/bidannounce.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 53,
    parentId: 35,
    parentName: '招标',
    name: '质疑答复',
    url: 'modules/zhaobiao/reply.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 54,
    parentId: 35,
    parentName: '招标',
    name: '合同备案',
    url: 'modules/zhaobiao/backup.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 55,
    parentId: 35,
    parentName: '招标',
    name: '其他事项',
    url: 'modules/zhaobiao/others.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 56,
    parentId: 43,
    parentName: '监理业务',
    name: '监理进展',
    url: 'modules/server/supervisionProgress.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 57,
    parentId: 43,
    parentName: '监理业务',
    name: '监理细则',
    url: 'modules/server/supervisionRules.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 58,
    parentId: 43,
    parentName: '监理业务',
    name: '项目通讯录',
    url: 'modules/server/supervisionContactList.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 59,
    parentId: 43,
    parentName: '监理业务',
    name: '监理交底',
    url: 'modules/server/handOver.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 60,
    parentId: 43,
    parentName: '监理业务',
    name: '开工令',
    url: 'modules/server/Start.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 61,
    parentId: 43,
    parentName: '监理业务',
    name: '会议管理',
    url: 'modules/server/conference.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 62,
    parentId: 43,
    parentName: '监理业务',
    name: '监理日志',
    url: 'modules/server/supervisionLog.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 63,
    parentId: 43,
    parentName: '监理业务',
    name: '监理月报',
    url: 'modules/server/supervisionMonthly.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 64,
    parentId: 43,
    parentName: '监理业务',
    name: '检查报告',
    url: 'modules/server/supCheckReport.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 65,
    parentId: 43,
    parentName: '监理业务',
    name: '专题报告',
    url: 'modules/server/suphematicReport.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 66,
    parentId: 43,
    parentName: '监理业务',
    name: '投资控制',
    url: 'modules/server/Investment control.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 67,
    parentId: 43,
    parentName: '监理业务',
    name: '监理总结',
    url: 'modules/server/supervisionSummary.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 68,
    parentId: 43,
    parentName: '监理业务',
    name: '验收报告',
    url: 'modules/server/supAcceptanceReport.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 69,
    parentId: 43,
    parentName: '监理业务',
    name: '用户评价',
    url: 'modules/server/supUserEvaluation.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 70,
    parentId: 43,
    parentName: '监理业务',
    name: '其他事项',
    url: 'modules/server/otherMatters.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 71,
    parentId: 0,
    parentName: null,
    name: '政府采购业务',
    url: null,
    perms: null,
    type: 0,
    icon: null,
    orderNum: 2,
    open: null,
    list: null
  },
  {
    menuId: 72,
    parentId: 71,
    parentName: '政府采购业务',
    name: '采购进展',
    url: 'modules/purchase/purchaseProgress.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 73,
    parentId: 71,
    parentName: '政府采购业务',
    name: '招标文件',
    url: 'modules/purchase/bidsfiles.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 74,
    parentId: 71,
    parentName: '政府采购业务',
    name: '公告发布',
    url: 'modules/purchase/publish.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 75,
    parentId: 71,
    parentName: '政府采购业务',
    name: '开标记录',
    url: 'modules/purchase/kbrecord.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 76,
    parentId: 71,
    parentName: '政府采购业务',
    name: '评标记录',
    url: 'modules/purchase/pbrecord.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 77,
    parentId: 71,
    parentName: '政府采购业务',
    name: '结果公示',
    url: 'modules/purchase/result.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 78,
    parentId: 71,
    parentName: '政府采购业务',
    name: '中标通知书',
    url: 'modules/purchase/bidsannounce.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 79,
    parentId: 71,
    parentName: '政府采购业务',
    name: '质疑答复',
    url: 'modules/purchase/reply.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 80,
    parentId: 71,
    parentName: '政府采购业务',
    name: '合同备案',
    url: 'modules/purchase/backup.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  },
  {
    menuId: 81,
    parentId: 71,
    parentName: '政府采购业务',
    name: '其他事项',
    url: 'modules/purchase/others.html',
    perms: null,
    type: 1,
    icon: null,
    orderNum: 0,
    open: null,
    list: null
  }
]

const mealToSub = {
  'total_count': 4,
  'ResultCode': 0,
  'Message': '查询成功',
  'Data': [
    {
      'ID': '1',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': '2019-05-06T13:50:41.617',
      'UpdateUser': '门店账户',
      'UpdateUserID': 'mendian',
      'GroupID': 0,
      'StoreID': 0,
      'Name': '宵夜',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '2',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': '2019-05-05T12:22:03.417',
      'UpdateUser': '门店账户',
      'UpdateUserID': 'mendian',
      'GroupID': 0,
      'StoreID': 0,
      'Name': '早餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '3',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': null,
      'UpdateUser': null,
      'UpdateUserID': null,
      'GroupID': 0,
      'StoreID': 0,
      'Name': '午餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '4',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': null,
      'UpdateUser': null,
      'UpdateUserID': null,
      'GroupID': 0,
      'StoreID': 0,
      'Name': '晚餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    }
  ]
}

// eslint-disable-next-line no-unused-vars
const managers = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 3,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [{
      userId: 1,
      username: 'admin',
      password: '9ec9750e709431dad22365cabc5c625482e574c74adaebba7dd02f1129e4ce1d',
      salt: 'YzcmCZNvbXocrsz9dm8e',
      email: 'root@renren.io',
      mobile: '13612345678',
      status: 1,
      roleIdList: null,
      createUserId: 1,
      createTime: '2016-11-11 11:11:11',
      role: ['admin']
    }, {
      userId: 4,
      username: 'rmhhttp',
      password: '054460372d1c97e68b6b82edc8ac7b70e8eb6ce4300d73fddc03b4ff9f9a8ee3',
      salt: 'b9b6ab71-438b-4dfd-',
      email: null,
      mobile: '18378097809',
      status: 1,
      roleIdList: null,
      createUserId: 1,
      createTime: '2019-03-14 14:07:37',
      role: ['user']
    }, {
      userId: 5,
      username: 'test',
      password: '2cf9e47f052b50c7263c246da1c3e6832f4de43e5e1b59c6a1ce073fee909786',
      salt: '9RNEmSuWTCCbLR24Z15t',
      email: 'test@qq.com',
      mobile: null,
      status: 0,
      roleIdList: [8, 10],
      createUserId: 1,
      createTime: '2019-03-23 14:30:59',
      role: ['user', 'admin']
    }]
  }
}

const selectRoles = {
  msg: 'success',
  code: 0,
  list: [{
    roleId: 8,
    roleName: 'asd',
    remark: '1',
    createUserId: 1,
    menuIdList: null,
    createTime: '2019-03-15 17:16:14'
  }, {
    roleId: 9,
    roleName: '1',
    remark: '1',
    createUserId: 1,
    menuIdList: null,
    createTime: '2019-03-20 12:29:19'
  }, {
    roleId: 10,
    roleName: 'test',
    remark: 'test',
    createUserId: 1,
    menuIdList: null,
    createTime: '2019-03-20 16:10:06'
  }, {
    roleId: 11,
    roleName: '业务测试',
    remark: null,
    createUserId: 1,
    menuIdList: null,
    createTime: '2019-03-23 14:30:27'
  }]
}

const roles = [
  {
    key: 'admin',
    name: 'admin',
    description: 'Super Administrator. Have access to view all pages.',
    routes: routes
  },
  {
    key: 'editor',
    name: 'editor',
    description: 'Normal Editor. Can see all pages except permission page',
    routes: filterAsyncRoutes(routes, ['editor'])
  },
  {
    key: 'visitor',
    name: 'visitor',
    description: 'Just a visitor. Can only see the home page and the document page',
    routes: [{
      path: '',
      redirect: 'dashboard',
      children: [
        {
          path: 'dashboard',
          name: 'Dashboard',
          meta: { title: 'dashboard', icon: 'dashboard' }
        }
      ]
    }]
  }
]

export default {
  getRoutes() {
    return routes
  },
  getRoles() {
    return roles
  },
  addRole() {
    return Mock.mock('@integer(300, 5000)')
  },
  updateRole() {
    const res = {
      data: 'success'
    }
    return res
  },
  getMealToSub() {
    return mealToSub
  },
  deleteRole() {
    const res = {
      data: 'success'
    }
    return res
  },
  getManagers: config => {
    const res = {
      data: 'success'
    }
    return res
  },
  getSelectRoles() {
    return selectRoles
  },
  getRolesMenu() {
    return treeData
  },
  getRolesList: config => {
    console.log(config)
    return rolesList
  },
  getMenuManagerList: config => {
    console.log(config)
    return menuListToMock
  }
}
