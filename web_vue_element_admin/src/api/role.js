import request from '@/utils/request'

export function getRoutes() {
  return request({
    url: '/routes',
    method: 'get'
  })
}

export function getRoles() {
  return request({
    url: '/roles',
    method: 'get'
  })
}

export function deleteRole(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function addRole(data) {
  return request({
    url: '/roles',
    method: 'post',
    data
  })
}

export function updateRole(key, data) {
  return request({
    url: `/roles/${key}`,
    method: 'put',
    data
  })
}

export function getMealToSub() {
  return request({
    url: '/mealToSub',
    method: 'get'
  })
}

export function getManagers(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}

export function getSelectRoles() {
  return request({
    url: '/selectRole',
    method: 'get'
  })
}

export function getRolesMenu() {
  return request({
    url: '/menuToRoles',
    method: 'get'
  })
}

export function getRolesList(query) {
  return request({
    url: '/listToRoles',
    method: 'get',
    params: query
  })
}

export function getMenuManagerList(query) {
  return request({
    url: '/MenuManagerList',
    method: 'get',
    params: query
  })
}

export function addManager(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function updateManager(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}
