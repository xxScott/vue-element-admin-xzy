const departmentSummary = {
  msg: 'success',
  code: 0,
  page: {
    totalCount: 3,
    pageSize: 10,
    totalPage: 1,
    currPage: 1,
    list: [{
      serial: '1',
      department: '业务部',
      number: '10',
      money: '99',
      fetch: ' 10'
    }, {
      serial: '2',
      department: '业务部',
      number: '20',
      money: '49',
      fetch: ' 60'
    }, {
      serial: '3',
      department: '研发部',
      number: '40',
      money: '23',
      fetch: ' 14'
    }, {
      serial: '4',
      department: '研发部',
      number: '20',
      money: '59',
      fetch: ' 50'
    }, {
      serial: '5',
      department: '人事部',
      number: '40',
      money: '39',
      fetch: ' 30'
    }, {
      serial: '6',
      department: '人事部',
      number: '40',
      money: '69',
      fetch: ' 30'
    }, {
      serial: '7',
      department: '业务二部',
      number: '20',
      money: '29',
      fetch: ' 13'
    }]
  }
}

export default {
  getDepartmentSummary: config => {
    console.log(config)
    return departmentSummary
  }
}
