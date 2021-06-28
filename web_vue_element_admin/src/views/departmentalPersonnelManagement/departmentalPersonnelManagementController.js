import {
  getStaffList,
  deleteStaffList,
  addStaffList,
  modifyStaffList,
  DepartmentTree,
  AccountTree
} from '@/api/staff'
import i18n from '@/lang'
import FilterContainer from '@/components/filterContainer'
import departmentalPersonnelManagementModel from './departmentalPersonnelManagementModel.js'
import elDragDialog from '@/directive/el-dragDialog'
import Pagination from '@/components/Pagination'
import { Message } from 'element-ui'

const defaultData = {
  department: '',
  departmentId: '',
  ID: ''
}

export default {
  components: { Pagination, FilterContainer },
  directives: { elDragDialog },
  data() {
    return departmentalPersonnelManagementModel
  },
  created() {
    this.GetStaffList()
    this.GetDepartmentList()
    this.GetStaffToSelect()
  },
  methods: {
    addHandler() {
      this.staff = Object.assign({}, defaultData)
      this.dialogType = 'new'
      this.dialogVisible = true
    },
    handleEdit(scope) {
      console.log(scope.row)
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.editID = scope.row.workId
      this.staff = Object.assign({}, defaultData)
      this.staff.UserName = scope.row.UserName
      this.staff.DepartmentCode = scope.row.DepartmentCode
      this.staff.DepartmentName = scope.row.DepartmentName
      this.staff.ID = scope.row.ID
      this.staff.remark = scope.row.remarks
      this.staff.department = scope.row.department
      // this.staff.departmentId = scope.row.departmentId
    },
    handleSelectMenu() {
      if (this.staff.departmentId) {
        this.$refs.tree.setCheckedKeys(this.staff.departmentId)
      }
    },
    popoverCancel() {
      document.querySelector('#app').click()
    },
    popoverConfirm() {
      const labels = []
      const ids = []
      const treeNodes = this.$refs.tree.getCheckedNodes()
      for (let i = 0; i < (treeNodes || []).length; i++) {
        if (treeNodes[i].children.length === 0) {
          labels.push(treeNodes[i].label)
          ids.push(treeNodes[i].id)
        }
      }
      this.staff.department = labels.join(',')
      this.staff.departmentId = ids
      document.querySelector('#app').click()
    },
    async GetDepartmentList() {
      const resMenu = await DepartmentTree({ methodName: 'TcDepartmentManagementQueryTree' })
      console.log(resMenu)
      const treeMenu = this.handleDataToTreeMenu(resMenu.data.Data)
      this.treeData = treeMenu
    },
    handleDataToTreeMenu(data) {
      const treeMenu = []
      for (let i = 0; i < (data || []).length; i++) {
        if (data[i].children) {
          treeMenu.push({
            label: data[i].Name,
            children: this.handleDataToTreeMenu(data[i].children),
            id: data[i].ID
          })
        } else {
          treeMenu.push({
            label: data[i].Name,
            children: [],
            id: data[i].ID
          })
        }
      }
      return treeMenu
    },
    async confirmHandler() {
      console.log(this.staff)
      const postData = {}
      if (this.dialogType === 'new') {
        postData.UserID = this.staff.name
        console.log(this.staff.departmentId)
        console.log(postData)
        postData.DepartmentID = []
        for (let i = 0; i < this.staff.departmentId.length; i++) {
          postData.DepartmentID.push({
            ID: this.staff.departmentId[i]
          })
        }
        await addStaffList({ methodName: 'TcDepartmentUserManagementAdd' }, postData).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            this.GetStaffList()
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.addSuccessMsg')
            })
          }
        })
      } else {
        await modifyStaffList({ methodName: 'TcDepartmentUserManagementUpdate', id: this.staff.ID }, this.staff).then(response => {
          if (response.data.data === 'success') {
            this.GetStaffList()
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.updataSuccessMsg')
            })
          } else {
            this.$message({
              type: 'error',
              message: i18n.t('commonLangZh.updataFailMsg')
            })
          }
        })
      }
    },
    resetForm(formName, dialogType) {
      if (dialogType !== 'edit') {
        this.$refs[formName].resetFields()
      }
    },
    deleteHandler(row) {
      console.log(row)
      this.$confirm(
        i18n.t('departmentalPersonnelManagement.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          this.deleteQuery.ID = row.ID
          await deleteStaffList(this.deleteQuery).then(response => {
            if (response.data.data === 'success') {
              this.GetStaffList()
              this.$message({
                type: 'success',
                message: i18n.t('commonLangZh.deleteSuccessMsg')
              })
            } else {
              this.$message({
                type: 'error',
                message: i18n.t('commonLangZh.deleteFailMsg')
              })
            }
          })
        })
        .catch(err => {
          console.error(err)
        })
    },
    tableSelectHandler(data) {
      this.treeSelectList = []
      for (let i = 0; i < data.length; i++) {
        this.treeSelectList.push(data[i].workId)
      }
    },
    tableSelectAllHandler(data) {
      this.treeSelectList = []
      for (let i = 0; i < data.length; i++) {
        this.treeSelectList.push(data[i].workId)
      }
    },
    filterHandler(event) {
      console.log(event)
      this.GetStaffList()
    },
    filteCancel(event) {
      this.searchData = event
      console.log(event)
    },
    GetStaffToSelect() {
      AccountTree({ methodName: 'TcAccountManagementQueryTree' }).then(response => {
        console.log(response.data.Data)
        const reslist = response.data.Data
        this.staffList = []
        for (let i = 0; i < (reslist || []).length; i++) {
          var obj = {}
          obj.value = reslist[i].ID
          obj.label = reslist[i].UserName + '(' + reslist[i].Phone + ')'
          this.staffList.push(obj)
        }
      })
    },
    GetStaffList() {
      this.listLoading = true
      getStaffList(this.listQuery).then(response => {
        this.listLoading = false
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          this.staffData = response.data.Data
          // this.treeData = response.data.Data
          this.total = response.data.total_count
        }
      })
    },
    handleDelete(scope) {
      this.$confirm(
        i18n.t('departmentalPersonnelManagement.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          console.log(scope)
          this.deleteQuery.ID = scope.row.ID
          await deleteStaffList(this.deleteQuery).then(response => {
            this.GetStaffList()
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.deleteSuccessMsg')
            })
          })
        })
        .catch(err => {
          console.error(err)
        })
    }
  }
}
