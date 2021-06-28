import request from '@/utils/request'

export function getMealManagement(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'get',
    params: query
  })
}

export function deleteMeal(query) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query
  })
}

export function addMeal(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}

export function updateMeal(query, data) {
  return request({
    url: '/api/AjaxHandler.ashx',
    method: 'post',
    params: query,
    data
  })
}
