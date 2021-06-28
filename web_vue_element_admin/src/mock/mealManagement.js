const mealManagement = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 3,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [{
      ID: '1',
      meal: '夜宵',
      expiration: '提前0天的16点',
      fetch: '00:00至00:00'
    }, {
      ID: '2',
      meal: '早餐',
      expiration: '提前0天的10点',
      fetch: '06:00至09:00'
    }, {
      ID: '3',
      meal: '中餐',
      expiration: '提前1天的10点',
      fetch: '11:00至13:00'
    }, {
      ID: '4',
      meal: '晚餐',
      expiration: '提前0天的14点',
      fetch: '17:00至19:00'
    }]
  }
}

// eslint-disable-next-line no-unused-vars
const mealToSub = {
  'total_count': 4,
  'ResultCode': 0,
  'Message': '查询成功',
  'Data': [
    {
      'ID': '1',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': '2019-05-06T13:50:41.617',
      'UpdateUser': '门店账户',
      'UpdateUserID': 'mendian',
      'GroupID': 0,
      'StoreID': 0,
      'Name': '宵夜',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '2',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': '2019-05-05T12:22:03.417',
      'UpdateUser': '门店账户',
      'UpdateUserID': 'mendian',
      'GroupID': 0,
      'StoreID': 0,
      'Name': '早餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '3',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': null,
      'UpdateUser': null,
      'UpdateUserID': null,
      'GroupID': 0,
      'StoreID': 0,
      'Name': '午餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    },
    {
      'ID': '4',
      'AddTime': null,
      'AddUser': null,
      'AddUserID': null,
      'UpdateTime': null,
      'UpdateUser': null,
      'UpdateUserID': null,
      'GroupID': 0,
      'StoreID': 0,
      'Name': '晚餐',
      'ReserveStart': null,
      'ReserveOver': null,
      'FetchStartTime': null,
      'FetchEndTime': null
    }
  ]
}

export default {
  getMealManagement: config => {
    console.log(config)
    return mealManagement
  },
  deleteRole: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },
  updateRole: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  }
}
