import i18n from '@/lang'

const defaultRole = {
  UserID: '',
  TrueName: '',
  Password: '',
  confirmPassword: '',
  status: i18n.t('commonLangZh.radioLabelStatusOne')
}

const searchData = {
  input: ''
}

export default {
  passForm: {
    show: {
      new: false,
      check: false
    }
  },
  total: 0,
  listQuery: {
    methodName: 'SysGroupUserManagementQueryList',
    pageNumber: 1,
    pageSize: 10,
    key: ''
  },
  deleteQuery: {
    methodName: 'SysGroupUserManagementDelete',
    ID: ''
  },
  listLoading: true,
  selectValue: [],
  transferData: [],
  role: Object.assign({}, defaultRole),
  searchData: Object.assign({}, searchData),
  rolesList: [],
  dialogVisible: false,
  dialogType: 'new',
  active: 0,
  stepOne: true,
  stepTwo: false,
  defaultProps: {
    children: 'children',
    label: 'title'
  },
  columnList: [
    {
      columnLable: i18n.t('managersList.UserID'),
      width: 70,
      columnData: 'UserID'
    },
    {
      columnLable: i18n.t('managersList.TrueName'),
      columnData: 'TrueName'
    },
    {
      columnLable: i18n.t('managersList.UpdateTime'),
      columnData: 'UpdateTime'
    },
    {
      columnLable: i18n.t('managersList.role'),
      columnData: 'role'
    }
  ]
}
