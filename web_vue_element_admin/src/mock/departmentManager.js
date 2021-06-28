import { menuListToMock } from '@/mock/menuList'

export default {
  getDepartmentList: config => {
    console.log(config)
    return menuListToMock
  },

  deleteDepartmentList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },

  addDepartmentList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },

  modifyDepartmentList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  }
}
