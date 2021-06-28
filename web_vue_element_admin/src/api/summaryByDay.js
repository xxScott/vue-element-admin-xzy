import request from '@/utils/request'

export function getSummaryByDay(query) {
  return request({
    url: '/summaryByDay',
    method: 'get',
    params: query
  })
}
