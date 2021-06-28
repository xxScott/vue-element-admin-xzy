const evaluationDetailsList = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 1,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [
      {
        id: '1',
        dataTodata: '2019/4/15(一)',
        department: '业务二部',
        staff: '小李',
        mealSeparation: '晚餐',
        mealName: '北京烤鸭 × 1',
        unitPrice: '￥8',
        actualPayment: '￥8',
        takeMeals: '1',
        starRating: [1],
        remarks: '送餐 [手机]',
        evaluation: '挺好'
      }, {
        id: '2',
        dataTodata: '2019/4/15(一)',
        department: '业务二部',
        staff: '小赵',
        mealSeparation: '早餐',
        mealName: '北京烤鸭 × 1',
        unitPrice: '￥8',
        actualPayment: '￥8',
        takeMeals: '0',
        starRating: [2],
        remarks: '送餐 [手机]',
        evaluation: '差评'
      }
    ]
  }
}

const mealsListFromServer = {
  'ResultCode': 0,
  'Message': '查询成功',
  'Data': [{
    'ID': '4',
    'AddTime': null,
    'AddUser': null,
    'AddUserID': null,
    'UpdateTime': null,
    'UpdateUser': null,
    'UpdateUserID': null,
    'CategoryName': '晚餐',
    'Type': [{
      'ID': '8',
      'Name': '素菜',
      'Value': null,
      'Food': null
    }, {
      'ID': '7',
      'Name': '小炒',
      'Value': null,
      'Food': null
    }, {
      'ID': '6',
      'Name': '大荤',
      'Value': null,
      'Food': null
    }, {
      'ID': '5',
      'Name': '套餐',
      'Value': null,
      'Food': null
    }]
  }, {
    'ID': '3',
    'AddTime': null,
    'AddUser': null,
    'AddUserID': null,
    'UpdateTime': null,
    'UpdateUser': null,
    'UpdateUserID': null,
    'CategoryName': '午餐',
    'Type': [{
      'ID': '8',
      'Name': '素菜',
      'Value': null,
      'Food': null
    }, {
      'ID': '7',
      'Name': '小炒',
      'Value': null,
      'Food': null
    }, {
      'ID': '6',
      'Name': '大荤',
      'Value': null,
      'Food': null
    }, {
      'ID': '5',
      'Name': '套餐',
      'Value': null,
      'Food': null
    }]
  }, {
    'ID': '2',
    'AddTime': null,
    'AddUser': null,
    'AddUserID': null,
    'UpdateTime': null,
    'UpdateUser': null,
    'UpdateUserID': null,
    'CategoryName': '早餐',
    'Type': [{
      'ID': '8',
      'Name': '素菜',
      'Value': null,
      'Food': [{
        'ID': 'cae6a684-b4d4-4f0f-80f0-6ace4b0552fa',
        'Name': '酸菜鱼',
        'Price': '38',
        'AddTime': null,
        'AddUser': null,
        'AddUserID': null,
        'UpdateTime': null,
        'UpdateUser': null,
        'UpdateUserID': null
      }, {
        'ID': '246af1e9-595d-4477-a2fa-482cd4f2db87',
        'Name': '土豆丝',
        'Price': '15',
        'AddTime': null,
        'AddUser': null,
        'AddUserID': null,
        'UpdateTime': null,
        'UpdateUser': null,
        'UpdateUserID': null
      }]
    }, {
      'ID': '7',
      'Name': '小炒',
      'Value': null,
      'Food': null
    }, {
      'ID': '6',
      'Name': '大荤',
      'Value': null,
      'Food': null
    }, {
      'ID': '5',
      'Name': '套餐',
      'Value': null,
      'Food': null
    }]
  }, {
    'ID': '1',
    'AddTime': null,
    'AddUser': null,
    'AddUserID': null,
    'UpdateTime': null,
    'UpdateUser': null,
    'UpdateUserID': null,
    'CategoryName': '宵夜',
    'Type': [{
      'ID': '8',
      'Name': '素菜',
      'Value': null,
      'Food': null
    }, {
      'ID': '7',
      'Name': '小炒',
      'Value': null,
      'Food': null
    }, {
      'ID': '6',
      'Name': '大荤',
      'Value': null,
      'Food': [{
        'ID': '6031dc50-13f4-4dd2-baaa-adc31f95e043',
        'Name': '大排',
        'Price': '15',
        'AddTime': null,
        'AddUser': null,
        'AddUserID': null,
        'UpdateTime': null,
        'UpdateUser': null,
        'UpdateUserID': null
      }, {
        'ID': '48495302-b8c8-4930-b105-47aed68a0fdb',
        'Name': '宫保鸡丁',
        'Price': '16',
        'AddTime': null,
        'AddUser': null,
        'AddUserID': null,
        'UpdateTime': null,
        'UpdateUser': null,
        'UpdateUserID': null
      }]
    }, {
      'ID': '5',
      'Name': '套餐',
      'Value': null,
      'Food': null
    }]
  }]
}

export default {
  getEvaluationDetailsList: config => {
    console.log(config)
    return evaluationDetailsList
  },
  getMealsList() {
    return mealsListFromServer
  }
}
