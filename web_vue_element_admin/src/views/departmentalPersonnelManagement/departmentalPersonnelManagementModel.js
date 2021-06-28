import i18n from '@/lang'

const searchData = {
  name: '',
  department: ''
}

const defaultData = {
  department: i18n.t('departmentManagement.firstDepartment'),
  departmentId: 'firstMenu',
  ID: '',
  remark: ''
}

export default {
  total: 0,
  listQuery: {
    methodName: 'TcDepartmentUserManagementQueryList',
    pageNumber: 1,
    pageSize: 20
  },
  deleteQuery: {
    methodName: 'TcDepartmentUserManagementDelete',
    ID: ''
  },
  treeSelectList: [],
  editID: '',
  defaultProps: {
    children: 'children',
    label: 'label'
  },
  staffData: [],
  staff: Object.assign({}, defaultData),
  listLoading: true,
  searchData: Object.assign({}, searchData),
  dialogVisible: false,
  dialogType: 'new',
  treeData: [],
  columnList: [
    {
      columnLable: i18n.t('departmentalPersonnelManagement.workId'),
      width: 100,
      columnData: 'DepartmentCode'
    },
    {
      columnLable: i18n.t('departmentalPersonnelManagement.name'),
      width: 250,
      columnData: 'UserName'
    },
    {
      columnLable: i18n.t('departmentalPersonnelManagement.department'),
      columnData: 'DepartmentName'
    },
    {
      columnLable: i18n.t('departmentalPersonnelManagement.remark'),
      columnData: 'remarks'
    }
  ],
  staffList: []
}
