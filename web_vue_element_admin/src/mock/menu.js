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
        mealSeparation: '早餐',
        mealName: '韭菜盒子',
        stock: '0',
        ordered: '0',
        limitOrder: '0',
        price: '5￥',
        status: 'true'
      }, {
        id: '2',
        dataTodata: '2019/4/15(一)',
        mealSeparation: '早餐',
        mealName: '菜叶蛋',
        stock: '0',
        ordered: '0',
        limitOrder: '0',
        price: '5￥',
        status: 'false'
      }
    ]
  }
}

export default {
  getMenuList: config => {
    console.log(config)
    return ordersList
  },
  deleteMenu: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },
  updataMenu: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  },
  addMenu: config => {
    console.log(config)
    const res = {
      data: 'success'
    }
    return res
  }
}
