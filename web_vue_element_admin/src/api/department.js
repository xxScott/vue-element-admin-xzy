import request from '@/utils/request'

export function getDepartmentSummary(query) {
  return request({
    url: '/departmentSummary',
    method: 'get',
    params: query
  })
}
