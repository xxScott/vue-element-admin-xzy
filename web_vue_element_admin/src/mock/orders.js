const ordersList = {
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
        remarks: '送餐 [手机]'
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
        remarks: '送餐 [手机]'
      }
    ]
  }
}

export default {
  getOrdersList: config => {
    console.log(config)
    return ordersList
  },
  deleteOrder: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },
  updataOrder: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  }
}
