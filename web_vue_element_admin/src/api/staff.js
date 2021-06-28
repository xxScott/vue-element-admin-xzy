import request from '@/utils/request'

export function getStaffList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}

export function deleteStaffList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function addStaffList(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data: data
  })
}

export function modifyStaffList(data, query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data: data
  })
}

export function DepartmentTree(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function AccountTree(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}
