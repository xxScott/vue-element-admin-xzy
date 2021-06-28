import i18n from '@/lang'

const defaultStaffForm = {
  departmentName: [],
  keyStaff: ''
}

export default {
  mealListQuery: {
    methodName: 'TcMealManagementQueryList',
    pageNumber: 1,
    pageSize: 10
  },
  mealTreeQuery: { methodName: 'TcFoodManagementQueryTree' },
  mealsActive: '',
  stepOne: true,
  stepTwo: false,
  stepThree: false,
  staffList: [],
  staffForm: Object.assign({}, defaultStaffForm),
  activeStep: 1,
  listQuery: {
    methodName: 'TcDepartmentUserManagementQueryList',
    pageNumber: 1,
    pageSize: 10
  },
  title: i18n.t('substitutionForMeals.step1'),
  mealsList: [],
  mealTypeList: [],
  departmentNameSelectList: [
    {
      value: '0',
      label: i18n.t('substitutionForMeals.allMealSeparation')
    },
    {
      value: '1',
      label: i18n.t('substitutionForMeals.breakfast')
    },
    {
      value: '2',
      label: i18n.t('substitutionForMeals.lunch')
    },
    {
      value: '3',
      label: i18n.t('substitutionForMeals.dinner')
    },
    {
      value: '4',
      label: i18n.t('substitutionForMeals.midnightSnack')
    }
  ],
  columnList: [
    {
      columnLable: i18n.t('substitutionForMeals.department'),
      columnData: 'DepartmentName'
    },
    {
      columnLable: i18n.t('substitutionForMeals.staff'),
      columnData: 'UserName'
    }
  ],
  mealsForm: {},
  orderForm: {
    modelSelect: i18n.t('substitutionForMeals.singleGive')
  },
  stepTwoForm: {},
  showMealsList: {},
  multipleSelection: [],
  selectedStaffList: [],
  multipleSelectedSelection: []
}
