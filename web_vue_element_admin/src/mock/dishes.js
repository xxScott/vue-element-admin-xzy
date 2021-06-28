const dishesSummary = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 3,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [{
      ID: '1',
      date: '2019-3-12',
      category: '早餐',
      name: '鱼香肉丝',
      evaluate: '390',
      average: '9',
      price: '14',
      number: '2',
      money: '28'
    }, {
      ID: '2',
      date: '2019-3-19',
      category: '早餐',
      name: '宫保鸡丁',
      evaluate: '390',
      average: '9',
      price: '14',
      number: '2',
      money: '28'
    }, {
      ID: '3',
      date: '2019-4-12',
      category: '早餐',
      name: '水煮鱼',
      evaluate: '390',
      average: '9',
      price: '34',
      number: '2',
      money: '28'
    }]
  }
}

export default {
  getDishesSummary: config => {
    console.log(config)
    return dishesSummary
  }
}
