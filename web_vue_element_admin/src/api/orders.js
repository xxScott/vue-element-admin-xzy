import request from '@/utils/request'

export function getOrdersList(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function deleteOrder(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function updataOrder(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}
