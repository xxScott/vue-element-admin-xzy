import request from '@/utils/request'

export function getDishesSummary(query) {
  return request({
    url: '/dishesSummary',
    method: 'get',
    params: query
  })
}
