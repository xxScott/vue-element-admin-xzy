import request from '@/utils/request'

export function loginByUsername(username, password) {
  const data = {
    username,
    password
  }
  return request({
    url: '/login/login',
    method: 'post',
    data
  })
}

export function logout() {
  return request({
    url: '/login/logout',
    method: 'post'
  })
}

export function getUserInfo(token) {
  return request({
    url: '/user/info',
    method: 'get',
    params: { token }
  })
}

export function getMenuNav(token) {
  return request({
    url: '/api/Api/Module/QueryAllMenu',
    method: 'get'
  })
}

export function getLogin(UserID, Password, UID) {
  const data = {
    UserID,
    Password,
    UID
  }
  return request({
    url: '/api/Api/Security/LoginCaiMoMo',
    method: 'post',
    data
  })
}
