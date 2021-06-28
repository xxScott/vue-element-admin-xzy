import i18n from '@/lang'

const searchData = {
  methodName: 'TcDepartmentManagementQueryTree',
  pageNumber: 1,
  pageSize: 20
}

const defaultData = {
  superiorDepartment: i18n.t('departmentManagement.firstDepartment'),
  superiorDepartmentId: 'firstMenu',
  ID: '',
  Name: '',
  Code: '',
  Loc: ''
}

export default {
  treeSelectList: [],
  editNotShow: false,
  editID: '',
  defaultExpandedList: ['firstMenu'],
  defaultProps: {
    children: 'children',
    label: 'label'
  },
  deleteQuery: {
    methodName: 'TcDepartmentManagementDelete',
    ID: ''
  },
  department: Object.assign({}, defaultData),
  tagType: 'success',
  listLoading: true,
  searchData: Object.assign({}, searchData),
  dialogVisible: false,
  dialogType: 'new',
  departmentList: [],
  treeData: [],
  columnList: [
    {
      label: i18n.t('departmentManagement.departmentID'),
      width: 120,
      key: 'ID'
    },
    {
      label: i18n.t('departmentManagement.departmentCode'),
      width: 120,
      key: 'Code'
    },
    {
      label: i18n.t('departmentManagement.departmentName'),
      width: 400,
      key: 'Name',
      expand: true,
      align: 'left'
    },
    {
      label: i18n.t('departmentManagement.departmentPerson'),
      key: 'parentName'
    },
    {
      label: i18n.t('departmentManagement.remark'),
      key: 'Loc'
    },
    {
      label: i18n.t('commonLangZh.operation'),
      key: 'operation',
      width: 170
    }
  ]
}
