import i18n from '@/lang'

export default {
  dialogType: 'new',
  tagType: 'success',
  total: 0,
  listQuery: {
    methodName: 'TcMealManagementQueryList',
    pageNumber: 1,
    pageSize: 10
  },
  deleteQuery: {
    methodName: 'TcMealManagementDelete',
    ID: ''
  },
  listLoading: true,
  dialogTableVisible: false,
  dialogFormVisible: false,
  dialogVisible: false,
  form: {
    name: '',
    reservation: '',
    fetch: ''
  },
  formLabelWidth: '100px',
  mealManagement: [],
  columnList: [
    {
      columnLable: i18n.t('mealManagement.ID'),
      columnData: 'ID'
    },
    {
      columnLable: i18n.t('mealManagement.meal'),
      columnData: 'Name'
    },
    {
      columnLable: i18n.t('mealManagement.expiration'),
      width: 250,
      columnData: 'expiration'
    },
    {
      columnLable: i18n.t('mealManagement.fetch'),
      width: 250,
      columnData: 'fetch'
    }
  ]
}
