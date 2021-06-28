import request from '@/utils/request'

export function getEvaluationDetailsList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}
export function getEvaluationList(query) {
  return request({
    url: '/evaluationDetailsList',
    method: 'get',
    params: query
  })
}

export function getMealsList(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}
export function addOrder(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}
