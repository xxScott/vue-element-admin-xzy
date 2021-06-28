const summaryByDay = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 3,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [{
      serial: '1',
      date: '2019-02-03',
      number: '10',
      money: '99',
      fetch: ' 10'
    }, {
      serial: '2',
      date: '2019-03-03',
      number: '20',
      money: '49',
      fetch: ' 60'
    }, {
      serial: '3',
      date: '2019-04-03',
      number: '40',
      money: '23',
      fetch: ' 14'
    }, {
      serial: '4',
      date: '2019-03-02',
      number: '20',
      money: '59',
      fetch: ' 50'
    }, {
      serial: '5',
      date: '2019-04-23',
      number: '40',
      money: '39',
      fetch: ' 30'
    }, {
      serial: '6',
      date: '2019-03-13',
      number: '40',
      money: '69',
      fetch: ' 30'
    }, {
      serial: '7',
      date: '2019-01-03',
      number: '20',
      money: '29',
      fetch: ' 13'
    }]
  }
}

export default {
  getSummaryByDay: config => {
    console.log(config)
    return summaryByDay
  }
}
