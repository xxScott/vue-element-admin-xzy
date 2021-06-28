import request from '@/utils/request'

export function getMenuList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}

export function deleteMenu(id) {
  return request({
    url: `/menu/${id}`,
    method: 'delete'
  })
}

export function updataMenu(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function addMenu(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}
