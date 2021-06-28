const staffList = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 1,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [
      {
        workId: '1',
        department: '管理部一',
        departmentId: [36],
        name: '小李',
        remarks: '送餐 [手机]'
      }, {
        workId: '2',
        department: '行政部一',
        departmentId: [32],
        name: '小赵',
        remarks: '送餐 [手机]'
      }
    ]
  }
}

export default {
  getStaffList: config => {
    console.log(config)
    return staffList
  },

  deleteStaffList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },

  addStaffList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },

  modifyStaffList: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  }
}
