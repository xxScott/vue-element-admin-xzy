import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('orderDetails.allMealSeparation'),
  userType: i18n.t('orderDetails.allTypes'),
  orderStatus: i18n.t('orderDetails.allStates'),
  score: i18n.t('orderDetails.All'),
  department: i18n.t('orderDetails.allDepartments'),
  terminalWindow: i18n.t('orderDetails.allTerminalWindow')
}

export default {
  orderId: '',
  tableSelectList: [],
  tagType: 'success',
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('orderDetails.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('orderDetails.breakfast')
    },
    {
      value: '2',
      label: i18n.t('orderDetails.lunch')
    },
    {
      value: '3',
      label: i18n.t('orderDetails.dinner')
    },
    {
      value: '4',
      label: i18n.t('orderDetails.midnightSnack')
    }
  ],
  userTypeOptions: [
    {
      value: '1',
      label: i18n.t('orderDetails.allTypes')
    },
    {
      value: '2',
      label: i18n.t('orderDetails.Leader')
    },
    {
      value: '3',
      label: i18n.t('orderDetails.staff')
    },
    {
      value: '4',
      label: i18n.t('orderDetails.customer')
    },
    {
      value: '5',
      label: i18n.t('orderDetails.familyMembers')
    }
  ],
  orderStatusOptions: [
    {
      value: '1',
      label: i18n.t('orderDetails.allStates')
    },
    {
      value: '2',
      label: i18n.t('orderDetails.alreadyOrdered')
    },
    {
      value: '3',
      label: i18n.t('orderDetails.fixedAndNotTaken')
    }
  ],
  scoreOptions: [
    {
      value: '0',
      label: i18n.t('orderDetails.All')
    },
    {
      value: '1',
      label: i18n.t('orderDetails.points1')
    },
    {
      value: '2',
      label: i18n.t('orderDetails.points2')
    },
    {
      value: '3',
      label: i18n.t('orderDetails.points3')
    },
    {
      value: '4',
      label: i18n.t('orderDetails.points4')
    },
    {
      value: '5',
      label: i18n.t('orderDetails.points5')
    }
  ],
  departmentOptions: [
    {
      value: '1',
      label: i18n.t('orderDetails.allDepartments')
    }
  ],
  terminalWindowOptions: [
    {
      value: '1',
      label: i18n.t('orderDetails.allTerminalWindow')
    },
    {
      value: '2',
      label: i18n.t('orderDetails.gateBrake')
    },
    {
      value: '3',
      label: i18n.t('orderDetails.voiceMachine')
    }
  ],
  total: 0,
  listQuery: {
    methodName: 'TcOrderManagerQueryList',
    pageNumber: 1,
    pageSize: 20
  },
  Query: {
    methodName: 'TcOrderManagerQueryList',
    pageNumber: 1,
    pageSize: 20,
    ID: ''
  },
  deleteQuery: {
    methodName: 'TcOrderManagerDelete',
    ID: ''
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  ordersList: [],
  expandList: [
    {
      columnLable: i18n.t('orderDetails.mealSeparation'),
      columnData: 'mealSeparation'
    },
    {
      columnLable: i18n.t('orderDetails.mealName'),
      columnData: 'mealName'
    },
    {
      columnLable: i18n.t('orderDetails.unitPrice'),
      columnData: 'Price'
    },
    {
      columnLable: i18n.t('orderDetails.actualPayment'),
      columnData: 'Practical'
    }
  ],
  columnList: [
    {
      columnLable: i18n.t('orderDetails.dataTodata'),
      columnData: 'AddTime'
    },
    {
      columnLable: i18n.t('orderDetails.department'),
      columnData: 'department'
    },
    {
      columnLable: i18n.t('orderDetails.staff'),
      columnData: 'staff',
      width: '90px'
    }
  ]
}
