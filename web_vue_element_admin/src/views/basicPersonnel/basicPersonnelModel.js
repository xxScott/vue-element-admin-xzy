import i18n from '@/lang'

const defaultRole = {
  UserName: '',
  WeChart: '',
  Phone: '',
  status: i18n.t('commonLangZh.radioLabelStatusTwo')
}

const searchData = {
  input: ''
}

export default {
  total: 0,
  listQuery: {
    methodName: 'TcAccountManagementQueryList',
    pageNumber: 1,
    pageSize: 10
  },
  deleteQuery: {
    methodName: 'TcAccountManagementDelete',
    ID: ''
  },
  listLoading: true,
  role: Object.assign({}, defaultRole),
  searchData: Object.assign({}, searchData),
  rolesList: [],
  dialogVisible: false,
  dialogType: 'new',
  defaultProps: {
    children: 'children',
    label: 'title'
  },
  columnList: [
    {
      columnLable: i18n.t('basicPersonnel.managerID'),
      width: 70,
      columnData: 'Code'
    },
    {
      columnLable: i18n.t('basicPersonnel.managerName'),
      width: 100,
      columnData: 'UserName'
    },
    {
      columnLable: i18n.t('basicPersonnel.managerWechart'),
      width: 120,
      columnData: 'WeChat'
    },
    {
      columnLable: i18n.t('basicPersonnel.managerPhone'),
      width: 120,
      columnData: 'Phone'
    },
    {
      columnLable: i18n.t('basicPersonnel.managerCreateTime'),
      columnData: 'AddTime',
      width: 160
    },
    {
      columnLable: i18n.t('basicPersonnel.role'),
      columnData: 'Role'
    }
  ]
}
