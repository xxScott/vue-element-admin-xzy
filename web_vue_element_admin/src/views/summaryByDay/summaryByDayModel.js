import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('summaryByDay.allMealSeparation'),
  userType: i18n.t('summaryByDay.allTypes'),
  orderStatus: i18n.t('summaryByDay.allStates'),
  score: i18n.t('summaryByDay.All'),
  department: i18n.t('summaryByDay.allDepartments'),
  terminalWindow: i18n.t('summaryByDay.allTerminalWindow')
}

export default {
  tagType: 'success',
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('summaryByDay.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('summaryByDay.breakfast')
    },
    {
      value: '2',
      label: i18n.t('summaryByDay.lunch')
    },
    {
      value: '3',
      label: i18n.t('summaryByDay.dinner')
    },
    {
      value: '4',
      label: i18n.t('summaryByDay.midnightSnack')
    }
  ],
  userTypeOptions: [
    {
      value: '1',
      label: i18n.t('summaryByDay.allTypes')
    },
    {
      value: '2',
      label: i18n.t('summaryByDay.Leader')
    },
    {
      value: '3',
      label: i18n.t('summaryByDay.staff')
    },
    {
      value: '4',
      label: i18n.t('summaryByDay.customer')
    },
    {
      value: '5',
      label: i18n.t('summaryByDay.familyMembers')
    }
  ],
  orderStatusOptions: [
    {
      value: '1',
      label: i18n.t('summaryByDay.allStates')
    },
    {
      value: '2',
      label: i18n.t('summaryByDay.alreadyOrdered')
    },
    {
      value: '3',
      label: i18n.t('summaryByDay.fixedAndNotTaken')
    },
    {
      value: '4',
      label: i18n.t('summaryByDay.undeterminedConsumption')
    }
  ],
  scoreOptions: [
    {
      value: '0',
      label: i18n.t('summaryByDay.All')
    },
    {
      value: '1',
      label: i18n.t('summaryByDay.points1')
    },
    {
      value: '2',
      label: i18n.t('summaryByDay.points2')
    },
    {
      value: '3',
      label: i18n.t('summaryByDay.points3')
    },
    {
      value: '4',
      label: i18n.t('summaryByDay.points4')
    },
    {
      value: '5',
      label: i18n.t('summaryByDay.points5')
    }
  ],
  departmentOptions: [
    {
      value: '1',
      label: i18n.t('summaryByDay.allDepartments')
    }
  ],
  terminalWindowOptions: [
    {
      value: '1',
      label: i18n.t('summaryByDay.allTerminalWindow')
    },
    {
      value: '2',
      label: i18n.t('summaryByDay.gateBrake')
    },
    {
      value: '3',
      label: i18n.t('summaryByDay.voiceMachine')
    }
  ],
  total: 0,
  listQuery: {
    page: 1,
    limit: 20
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  summaryByDay: [],
  columnList1: [
    {
      columnLable: i18n.t('summaryByDay.midnightSnack'),
      width: 70,
      columnData: 'midnightSnack'
    },
    {
      columnLable: i18n.t('summaryByDay.breakfast'),
      width: 70,
      columnData: 'breakfast'
    },
    {
      columnLable: i18n.t('summaryByDay.lunch'),
      width: 70,
      columnData: 'lunch'
    },
    {
      columnLable: i18n.t('summaryByDay.dinner'),
      width: 70,
      columnData: 'dinner'
    }
  ],
  columnList2: [
    { columnLable: i18n.t('summaryByDay.number'),
      width: 70,
      columnData: 'number'
    },
    { columnLable: i18n.t('summaryByDay.money'),
      width: 70,
      columnData: 'money'
    },
    { columnLable: i18n.t('summaryByDay.fetch'),
      width: 70,
      columnData: 'fetch'
    }
  ]
}
