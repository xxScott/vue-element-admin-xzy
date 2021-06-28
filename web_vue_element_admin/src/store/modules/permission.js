import { constantRoutes } from '@/router'
import { getMenuNav } from '@/api/login'
import { getToken } from '@/utils/auth'
const _import = require('@/router/_import_' + process.env.NODE_ENV)// 获取组件的方法
import Layout from '@/views/layout/Layout' // Layout 是架构组件，不在后台返回，在文件里单独引入

/**
 * 通过meta.role判断是否与当前用户权限匹配
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * 递归过滤异步路由表，返回符合用户角色权限的路由表
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []

  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

function filterAsyncRouter(asyncRouterMap) { // 遍历后台传来的路由字符串，转换为组件对象
  const accessedRouters = asyncRouterMap.filter(route => {
    if (route.component) {
      if (route.component === 'Layout') { // Layout组件特殊处理
        route.component = Layout
      } else {
        route.component = _import(route.component)
      }
    }
    if (route.children && route.children.length) {
      route.children = filterAsyncRouter(route.children)
    }
    return true
  })

  return accessedRouters
}

const permission = {
  state: {
    routes: [],
    addRoutes: [],
    token: getToken()
  },
  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_ROUTES: (state, routes) => {
      state.addRoutes = routes
      state.routes = constantRoutes.concat(routes)
    }
  },
  actions: {
    GenerateRoutes({ commit, state }, data) {
      return new Promise((resolve, reject) => {
        getMenuNav(state.token).then(response => {
          commit('SET_ROUTES', filterAsyncRouter(response.data.data))
          resolve(response.data.data)
        }).catch(error => {
          reject(error)
        })
      })
    }
  }
}

export default permission
