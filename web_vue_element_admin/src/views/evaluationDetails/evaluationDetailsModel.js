import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('evaluationDetails.allMealSeparation'),
  userType: i18n.t('evaluationDetails.allTypes'),
  orderStatus: i18n.t('evaluationDetails.allStates'),
  score: i18n.t('evaluationDetails.All'),
  department: i18n.t('evaluationDetails.allDepartments'),
  terminalWindow: i18n.t('evaluationDetails.allTerminalWindow')
}

export default {
  tagType: 'success',
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('evaluationDetails.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('evaluationDetails.breakfast')
    },
    {
      value: '2',
      label: i18n.t('evaluationDetails.lunch')
    },
    {
      value: '3',
      label: i18n.t('evaluationDetails.dinner')
    },
    {
      value: '4',
      label: i18n.t('evaluationDetails.midnightSnack')
    }
  ],
  userTypeOptions: [
    {
      value: '1',
      label: i18n.t('evaluationDetails.allTypes')
    },
    {
      value: '2',
      label: i18n.t('evaluationDetails.Leader')
    },
    {
      value: '3',
      label: i18n.t('evaluationDetails.staff')
    },
    {
      value: '4',
      label: i18n.t('evaluationDetails.customer')
    },
    {
      value: '5',
      label: i18n.t('evaluationDetails.familyMembers')
    }
  ],
  orderStatusOptions: [
    {
      value: '1',
      label: i18n.t('evaluationDetails.allStates')
    },
    {
      value: '2',
      label: i18n.t('evaluationDetails.alreadyOrdered')
    },
    {
      value: '3',
      label: i18n.t('evaluationDetails.fixedAndNotTaken')
    },
    {
      value: '4',
      label: i18n.t('evaluationDetails.undeterminedConsumption')
    }
  ],
  scoreOptions: [
    {
      value: '0',
      label: i18n.t('evaluationDetails.All')
    },
    {
      value: '1',
      label: i18n.t('evaluationDetails.points1')
    },
    {
      value: '2',
      label: i18n.t('evaluationDetails.points2')
    },
    {
      value: '3',
      label: i18n.t('evaluationDetails.points3')
    },
    {
      value: '4',
      label: i18n.t('evaluationDetails.points4')
    },
    {
      value: '5',
      label: i18n.t('evaluationDetails.points5')
    }
  ],
  departmentOptions: [
    {
      value: '1',
      label: i18n.t('evaluationDetails.allDepartments')
    }
  ],
  terminalWindowOptions: [
    {
      value: '1',
      label: i18n.t('evaluationDetails.allTerminalWindow')
    },
    {
      value: '2',
      label: i18n.t('evaluationDetails.gateBrake')
    },
    {
      value: '3',
      label: i18n.t('evaluationDetails.voiceMachine')
    }
  ],
  total: 0,
  listQuery: {
    page: 1,
    limit: 20
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  ordersList: [],
  columnList: [
    {
      columnLable: i18n.t('evaluationDetails.dataTodata'),
      columnData: 'dataTodata'
    },
    {
      columnLable: i18n.t('evaluationDetails.department'),
      columnData: 'department'
    },
    {
      columnLable: i18n.t('evaluationDetails.staff'),
      columnData: 'staff',
      width: '90px'
    }
  ],
  expandList: [
    {
      columnLable: i18n.t('evaluationDetails.mealSeparation'),
      columnData: 'mealSeparation'
    },
    {
      columnLable: i18n.t('evaluationDetails.mealName'),
      columnData: 'mealName'
    },
    {
      columnLable: i18n.t('evaluationDetails.unitPrice'),
      columnData: 'unitPrice'
    },
    {
      columnLable: i18n.t('evaluationDetails.actualPayment'),
      columnData: 'actualPayment'
    }
  ]
}
