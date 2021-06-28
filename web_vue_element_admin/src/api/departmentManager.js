import request from '@/utils/request'

export function getDepartmentList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}

export function deleteDepartmentList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function addDepartmentList(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function modifyDepartmentList(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function getSingleDepartment(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}
