import { param2Obj } from '@/utils'

const userMap = {
  admin: {
    roles: ['admin'],
    token: 'admin',
    introduction: '我是超级管理员',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Super Admin'
  },
  editor: {
    roles: ['editor'],
    token: 'editor',
    introduction: '我是编辑',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Normal Editor'
  }
}

export const asyncRoutes = [
  {
    path: '',
    component: 'Layout',
    redirect: 'dashboard',
    children: [
      {
        path: 'dashboard',
        component: 'dashboard/index',
        name: 'Dashboard',
        meta: { title: '首页', icon: 'dashboard', noCache: true, affix: true }
      }
    ]
  },
  {
    path: '/accountManagement',
    component: 'Layout',
    children: [
      {
        path: 'index',
        component: 'accountManagement/accountManagement',
        name: 'accountManagement',
        meta: { title: '账号管理', icon: 'accountManagement' }
      }
    ]
  },
  {
    path: '/orderDetailedManagement',
    component: 'Layout',
    redirect: '/orderDetailedManagement/orderDetails',
    alwaysShow: true, // will always show the root menu
    meta: {
      title: '订单明细管理',
      icon: 'orderDetailedManagement'
    },
    children: [
      {
        path: 'orderDetails',
        component: 'orderDetails/orderDetails',
        name: 'orderDetails',
        meta: {
          title: '订单明细',
          icon: 'orderDetails'
        }
      },
      {
        path: 'evaluationDetails',
        component: 'evaluationDetails/evaluationDetails',
        name: 'evaluationDetails',
        meta: {
          title: '评价明细',
          icon: 'evaluationDetails'
        }
      }
    ]
  },
  {
    path: '/substitutionForMeals',
    component: 'Layout',
    children: [
      {
        path: 'index',
        component: 'substitutionForMeals/substitutionForMeals',
        name: 'substitutionForMeals',
        meta: { title: '批量代订餐', icon: 'substitutionForMeals' }
      }
    ]
  },
  {
    path: '/menuManagementTo',
    component: 'Layout',
    redirect: '/menuManagementTo/menuListImport',
    name: 'menuManagementTo',
    meta: {
      title: '菜单管理',
      icon: 'menuManagementTo'
    },
    children: [
      {
        path: 'menuListImport',
        component: 'menuListImport/menuListImport',
        name: 'menuListImport',
        meta: { title: '菜单列表&导入', icon: 'menuListImport' }
      },
      {
        path: 'edit',
        component: 'menuListImport/components/index',
        name: 'EditMenu',
        meta: { title: '编辑菜单', noCache: true },
        hidden: true
      },
      {
        path: 'add',
        component: 'menuListImport/components/index',
        name: 'AddMenu',
        meta: { title: '添加菜单', noCache: true },
        hidden: true
      }
    ]
  },
  {
    path: '/orderingMeals',
    component: 'Layout',
    redirect: '/orderingMeals/departmentSummary',
    name: 'orderingMeals',
    meta: {
      title: '汇总',
      icon: 'orderingMeals'
    },
    children: [
      {
        path: 'departmentSummary',
        component: 'departmentSummary/departmentSummary',
        name: 'departmentSummary',
        meta: { title: '部门汇总', icon: 'departmentSummary' }
      },
      {
        path: 'summaryByDay',
        component: 'summaryByDay/summaryByDay',
        name: 'summaryByDay',
        meta: { title: '按天汇总', icon: 'summaryByDay' }
      },
      {
        path: 'dishesSummary',
        component: 'dishesSummary/dishesSummary',
        name: 'dishesSummary',
        meta: { title: '菜品汇总', icon: 'dishesSummary' }
      }
    ]
  },
  {
    path: '/mealManagement',
    component: 'Layout',
    children: [
      {
        path: 'index',
        component: 'mealManagement/mealManagement',
        name: 'mealManagement',
        meta: { title: '餐别管理', icon: 'mealManagement' }
      }
    ]
  },
  {
    path: '/basic',
    component: 'Layout',
    redirect: '/basic/basic',
    name: 'basic',
    meta: {
      title: '基础管理',
      icon: 'basic'
    },
    children: [
      {
        path: 'departmentManagement',
        component: 'departmentManagement/departmentManagement',
        name: 'departmentManagement',
        meta: { title: '部门管理', icon: 'departmentManagement' }
      },
      {
        path: 'basicPersonnel',
        component: 'basicPersonnel/basicPersonnel',
        name: 'basicPersonnel',
        meta: { title: '人员管理', icon: 'personnelManagement' }
      },
      {
        path: 'departmentalPersonnelManagement',
        component: 'departmentalPersonnelManagement/departmentalPersonnelManagement',
        name: 'departmentalPersonnelManagement',
        meta: { title: '部门人员管理', icon: 'departmentalPersonnelManagement' }
      }
    ]
  },
  { path: '*', redirect: '/404', hidden: true }
]

export default {
  loginByUsername: config => {
    const { username } = JSON.parse(config.body)
    return userMap[username]
  },
  getUserInfo: config => {
    return userMap['admin']
  },
  logout: () => 'success',
  getMenuNav: config => {
    const { token } = param2Obj(config.url)
    if (token) {
      return asyncRoutes
    } else {
      return false
    }
  }
}
