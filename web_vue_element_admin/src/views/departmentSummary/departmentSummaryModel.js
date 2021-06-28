import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('departmentSummary.allMealSeparation'),
  userType: i18n.t('departmentSummary.allTypes'),
  orderStatus: i18n.t('departmentSummary.allStates'),
  score: i18n.t('departmentSummary.All'),
  department: i18n.t('departmentSummary.allDepartments'),
  terminalWindow: i18n.t('departmentSummary.allTerminalWindow')
}

export default {
  tagType: 'success',
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('departmentSummary.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('departmentSummary.breakfast')
    },
    {
      value: '2',
      label: i18n.t('departmentSummary.lunch')
    },
    {
      value: '3',
      label: i18n.t('departmentSummary.dinner')
    },
    {
      value: '4',
      label: i18n.t('departmentSummary.midnightSnack')
    }
  ],
  userTypeOptions: [
    {
      value: '1',
      label: i18n.t('departmentSummary.allTypes')
    },
    {
      value: '2',
      label: i18n.t('departmentSummary.Leader')
    },
    {
      value: '3',
      label: i18n.t('departmentSummary.staff')
    },
    {
      value: '4',
      label: i18n.t('departmentSummary.customer')
    },
    {
      value: '5',
      label: i18n.t('departmentSummary.familyMembers')
    }
  ],
  orderStatusOptions: [
    {
      value: '1',
      label: i18n.t('departmentSummary.allStates')
    },
    {
      value: '2',
      label: i18n.t('departmentSummary.alreadyOrdered')
    },
    {
      value: '3',
      label: i18n.t('departmentSummary.fixedAndNotTaken')
    },
    {
      value: '4',
      label: i18n.t('departmentSummary.undeterminedConsumption')
    }
  ],
  scoreOptions: [
    {
      value: '0',
      label: i18n.t('departmentSummary.All')
    },
    {
      value: '1',
      label: i18n.t('departmentSummary.points1')
    },
    {
      value: '2',
      label: i18n.t('departmentSummary.points2')
    },
    {
      value: '3',
      label: i18n.t('departmentSummary.points3')
    },
    {
      value: '4',
      label: i18n.t('departmentSummary.points4')
    },
    {
      value: '5',
      label: i18n.t('departmentSummary.points5')
    }
  ],
  departmentOptions: [
    {
      value: '1',
      label: i18n.t('departmentSummary.allDepartments')
    }
  ],
  terminalWindowOptions: [
    {
      value: '1',
      label: i18n.t('departmentSummary.allTerminalWindow')
    },
    {
      value: '2',
      label: i18n.t('departmentSummary.gateBrake')
    },
    {
      value: '3',
      label: i18n.t('departmentSummary.voiceMachine')
    }
  ],
  total: 0,
  listQuery: {
    page: 1,
    limit: 20
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  departmentSummary: [],
  columnList1: [
    {
      columnLable: i18n.t('departmentSummary.midnightSnack'),
      width: 70,
      columnData: 'midnightSnack'
    },
    {
      columnLable: i18n.t('departmentSummary.breakfast'),
      width: 70,
      columnData: 'breakfast'
    },
    {
      columnLable: i18n.t('departmentSummary.lunch'),
      width: 70,
      columnData: 'lunch'
    },
    {
      columnLable: i18n.t('departmentSummary.dinner'),
      width: 70,
      columnData: 'dinner'
    }
  ],
  columnList2: [
    { columnLable: i18n.t('departmentSummary.number'),
      width: 70,
      columnData: 'number'
    },
    { columnLable: i18n.t('departmentSummary.money'),
      width: 70,
      columnData: 'money'
    },
    { columnLable: i18n.t('departmentSummary.fetch'),
      width: 70,
      columnData: 'fetch'
    }
  ]
}
