import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('dishesSummary.allMealSeparation'),
  userType: i18n.t('dishesSummary.allTypes'),
  orderStatus: i18n.t('dishesSummary.allStates'),
  score: i18n.t('dishesSummary.All'),
  department: i18n.t('dishesSummary.allDepartments'),
  terminalWindow: i18n.t('dishesSummary.allTerminalWindow')
}

export default {
  tagType: 'success',
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('dishesSummary.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('dishesSummary.breakfast')
    },
    {
      value: '2',
      label: i18n.t('dishesSummary.lunch')
    },
    {
      value: '3',
      label: i18n.t('dishesSummary.dinner')
    },
    {
      value: '4',
      label: i18n.t('dishesSummary.midnightSnack')
    }
  ],
  userTypeOptions: [
    {
      value: '1',
      label: i18n.t('dishesSummary.allTypes')
    },
    {
      value: '2',
      label: i18n.t('dishesSummary.Leader')
    },
    {
      value: '3',
      label: i18n.t('dishesSummary.staff')
    },
    {
      value: '4',
      label: i18n.t('dishesSummary.customer')
    },
    {
      value: '5',
      label: i18n.t('dishesSummary.familyMembers')
    }
  ],
  orderStatusOptions: [
    {
      value: '1',
      label: i18n.t('dishesSummary.allStates')
    },
    {
      value: '2',
      label: i18n.t('dishesSummary.alreadyOrdered')
    },
    {
      value: '3',
      label: i18n.t('dishesSummary.fixedAndNotTaken')
    },
    {
      value: '4',
      label: i18n.t('dishesSummary.undeterminedConsumption')
    }
  ],
  scoreOptions: [
    {
      value: '0',
      label: i18n.t('dishesSummary.All')
    },
    {
      value: '1',
      label: i18n.t('dishesSummary.points1')
    },
    {
      value: '2',
      label: i18n.t('dishesSummary.points2')
    },
    {
      value: '3',
      label: i18n.t('dishesSummary.points3')
    },
    {
      value: '4',
      label: i18n.t('dishesSummary.points4')
    },
    {
      value: '5',
      label: i18n.t('dishesSummary.points5')
    }
  ],
  departmentOptions: [
    {
      value: '1',
      label: i18n.t('dishesSummary.allDepartments')
    }
  ],
  terminalWindowOptions: [
    {
      value: '1',
      label: i18n.t('dishesSummary.allTerminalWindow')
    },
    {
      value: '2',
      label: i18n.t('dishesSummary.gateBrake')
    },
    {
      value: '3',
      label: i18n.t('dishesSummary.voiceMachine')
    }
  ],
  total: 0,
  listQuery: {
    page: 1,
    limit: 20
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  dishesSummary: [],
  columnList: [
    {
      columnLable: i18n.t('dishesSummary.ID'),
      width: 110,
      columnData: 'ID'
    },
    {
      columnLable: i18n.t('dishesSummary.date'),
      width: 110,
      columnData: 'date'
    },
    {
      columnLable: i18n.t('dishesSummary.category'),
      width: 170,
      columnData: 'category'
    },
    {
      columnLable: i18n.t('dishesSummary.name'),
      width: 170,
      columnData: 'name'
    },
    {
      columnLable: i18n.t('dishesSummary.packageName'),
      width: 170,
      columnData: 'packageName'
    }
  ],
  columnList1: [
    {
      columnLable: i18n.t('dishesSummary.evaluate'),
      width: 110,
      columnData: 'evaluate'
    },
    {
      columnLable: i18n.t('dishesSummary.average'),
      width: 110,
      columnData: 'average'
    },
    {
      columnLable: i18n.t('dishesSummary.price'),
      width: 110,
      columnData: 'price'
    },
    {
      columnLable: i18n.t('dishesSummary.number'),
      width: 110,
      columnData: 'number'
    },
    {
      columnLable: i18n.t('dishesSummary.money'),
      width: 110,
      columnData: 'money'
    }
  ]
}
