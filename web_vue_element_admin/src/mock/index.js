import Mock from 'mockjs'
import loginAPI from './login'
import articleAPI from './article'
import remoteSearchAPI from './remoteSearch'
import transactionAPI from './transaction'
import roleAPI from './role'
import ordersAPI from './orders'
import evaluationDetailsAPI from './evaluationDetails'
import DishesAPI from './dishes'
import DepartmentAPI from './department'
import SummaryByDayAPI from './summaryByDay'
import menuAPI from './menu'
import MealManagementAPI from './mealManagement'
import departmentManagerAPI from './departmentManager'
import staffAPT from './staff.js'

// 修复在使用 MockJS 情况下，设置 withCredentials = true，且未被拦截的跨域请求丢失 Cookies 的问题
// https://github.com/nuysoft/Mock/issues/300
Mock.XHR.prototype.proxy_send = Mock.XHR.prototype.send
Mock.XHR.prototype.send = function() {
  if (this.custom.xhr) {
    this.custom.xhr.withCredentials = this.withCredentials || false
  }
  this.proxy_send(...arguments)
}

// Mock.setup({
//   timeout: '350-600'
// })

// 登录相关
Mock.mock(/\/login\/login/, 'post', loginAPI.loginByUsername)
Mock.mock(/\/login\/logout/, 'post', loginAPI.logout)
Mock.mock(/\/user\/info\.*/, 'get', loginAPI.getUserInfo)
Mock.mock(/\/user\/menu\.*/, 'get', loginAPI.getMenuNav)

// 角色相关
Mock.mock(/\/routes/, 'get', roleAPI.getRoutes)
Mock.mock(/\/roles/, 'get', roleAPI.getRoles)
Mock.mock(/\/roles$/, 'post', roleAPI.addRole)
Mock.mock(/\/roles\/[A-Za-z0-9]+/, 'put', roleAPI.updateRole)
Mock.mock(/\/roles\/[A-Za-z0-9]+/, 'delete', roleAPI.deleteRole)
Mock.mock(/\/managers/, 'get', roleAPI.getManagers)
Mock.mock(/\/selectRole/, 'get', roleAPI.getSelectRoles)
Mock.mock(/\/menuToRoles/, 'get', roleAPI.getRolesMenu)
Mock.mock(/\/listToRoles/, 'get', roleAPI.getRolesList)
Mock.mock(/\/MenuManagerList/, 'get', roleAPI.getMenuManagerList)
Mock.mock(/\/mealToSub/, 'get', roleAPI.getMealToSub)

// 文章相关
Mock.mock(/\/article\/list/, 'get', articleAPI.getList)
Mock.mock(/\/article\/detail/, 'get', articleAPI.getArticle)
Mock.mock(/\/article\/pv/, 'get', articleAPI.getPv)
Mock.mock(/\/article\/create/, 'post', articleAPI.createArticle)
Mock.mock(/\/article\/update/, 'post', articleAPI.updateArticle)

// 搜索相关
Mock.mock(/\/search\/user/, 'get', remoteSearchAPI.searchUser)

// 账单相关
Mock.mock(/\/transaction\/list/, 'get', transactionAPI.getList)

// 订单相关
Mock.mock(/\/orderList/, 'get', ordersAPI.getOrdersList)
Mock.mock(/\/order\/[A-Za-z0-9]+/, 'delete', ordersAPI.deleteOrder)
Mock.mock(/\/order\/[A-Za-z0-9]+/, 'put', ordersAPI.updataOrder)

// 评价相关
Mock.mock(/\/evaluationDetails/, 'get', evaluationDetailsAPI.getEvaluationDetailsList)
Mock.mock(/\/mealsList/, 'get', evaluationDetailsAPI.getMealsList)

// 菜品汇总
Mock.mock(/\/dishesSummary/, 'get', DishesAPI.getDishesSummary)
// 部门汇总
Mock.mock(/\/departmentSummary/, 'get', DepartmentAPI.getDepartmentSummary)
// 按天汇总
Mock.mock(/\/summaryByDay/, 'get', SummaryByDayAPI.getSummaryByDay)

// 菜单相关
Mock.mock(/\/menuList/, 'get', menuAPI.getMenuList)
Mock.mock(/\/menu\/[A-Za-z0-9]+/, 'delete', menuAPI.deleteMenu)
Mock.mock(/\/menu\/[A-Za-z0-9]+/, 'put', menuAPI.updataMenu)
Mock.mock(/\/menu/, 'post', menuAPI.addMenu)

// 餐别列表
Mock.mock(/\/mealManagement/, 'get', MealManagementAPI.getMealManagement)
Mock.mock(/\/mealManagement\/[A-Za-z0-9]+/, 'delete', MealManagementAPI.deleteRole)
Mock.mock(/\/mealManagement/, 'post', MealManagementAPI.addRole)
Mock.mock(/\/mealManagement\/[A-Za-z0-9]+/, 'put', MealManagementAPI.updateRole)

// 部门管理相关
Mock.mock(/\/departmentList/, 'get', departmentManagerAPI.getDepartmentList)
Mock.mock(/\/departmentList\/create/, 'post', departmentManagerAPI.addDepartmentList)
Mock.mock(/\/departmentList\/delete/, 'delete', departmentManagerAPI.deleteDepartmentList)
Mock.mock(/\/departmentList\/updata\/[A-Za-z0-9]+/, 'put', departmentManagerAPI.modifyDepartmentList)

// 部门人员相关
Mock.mock(/\/staffList/, 'get', staffAPT.getStaffList)
Mock.mock(/\/staffList\/create/, 'post', staffAPT.addStaffList)
Mock.mock(/\/staffList\/delete/, 'delete', staffAPT.deleteStaffList)
Mock.mock(/\/staffList\/updata\/[A-Za-z0-9]+/, 'put', staffAPT.modifyStaffList)

export default Mock
