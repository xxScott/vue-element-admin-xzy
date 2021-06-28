import {
  getDepartmentList,
  deleteDepartmentList,
  addDepartmentList,
  modifyDepartmentList
} from '@/api/departmentManager'
import { deepClone } from '@/utils'
import i18n from '@/lang'
import FilterContainer from '@/components/filterContainer'
import departmentManagementModel from './departmentManagementModel.js'
import treeTable from '@/components/TreeTable'
import elDragDialog from '@/directive/el-dragDialog'
import { Message } from 'element-ui'

const defaultData = {
  superiorDepartment: i18n.t('departmentManagement.firstDepartment'),
  superiorDepartmentId: 'firstMenu',
  ID: '',
  Name: '',
  Code: '',
  Loc: ''
}

export default {
  components: { FilterContainer, treeTable },
  directives: { elDragDialog },
  data() {
    return departmentManagementModel
  },
  created() {
    this.getDepartments()
    this.GetDepartmentList()
  },
  methods: {
    async handleEdit(scope) {
      console.log(scope.row)
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.editNotShow = true
      this.editID = scope.row.menuId
      this.department = Object.assign({}, defaultData)
      this.department.Name = scope.row.Name
      this.department.Code = scope.row.Code
      this.department.departmentPerson = scope.row.departmentPerson
      this.department.remark = scope.row.remark
      this.department.ID = scope.row.ID
    },
    reset() {
      this.defaultExpandedList = ['firstMenu']
    },
    handleSelectMenu() {
      console.log('handleSelectMenu')
    },
    popoverCancel() {
      document.querySelector('#app').click()
    },
    popoverConfirm() {
      this.department.superiorDepartment = this.$refs.tree.getCurrentNode().label
      this.department.superiorDepartmentId = this.$refs.tree.getCurrentNode().id
      document.querySelector('#app').click()
    },
    async GetDepartmentList() {
      this.treeData = [
        {
          label: this.$t('departmentManagement.firstDepartment'),
          id: 'firstMenu',
          children: []
        }
      ]
      await getDepartmentList(this.searchData).then(response => {
        console.log(response)
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          console.log(response)
          const treeMenu = this.handleDataToTreeMenu(response.data.Data)
          this.treeData[0].children = treeMenu
        }
      })
    },
    handleDataToTreeMenu(data) {
      const treeMenu = []
      for (let i = 0; i < (data || []).length; i++) {
        if (data[i].list) {
          treeMenu.push({
            label: data[i].name,
            children: this.handleDataToTreeMenu(data[i].list),
            id: data[i].menuId
          })
        } else {
          treeMenu.push({
            label: data[i].name,
            children: [],
            id: data[i].menuId
          })
        }
      }
      return treeMenu
    },
    addHandler() {
      this.department = Object.assign({}, defaultData)
      this.dialogType = 'new'
      this.dialogVisible = true
      this.editNotShow = false
    },
    async confirmHandler() {
      console.log(this.department)
      if (this.dialogType === 'new') {
        await addDepartmentList({ methodName: 'TcDepartmentManagementAdd' }, this.department).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            this.getDepartments()
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.addSuccessMsg')
            })
          }
        })
      } else {
        delete this.department.superiorDepartment
        delete this.department.superiorDepartmentId
        await modifyDepartmentList({ methodName: 'TcDepartmentManagementUpdate', id: this.department.ID }, this.department).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            this.getDepartments()
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.updataSuccessMsg')
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
    deleteHandler() {
      this.$confirm(
        i18n.t('departmentManagement.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          const deleteList = { ids: this.treeSelectList }
          await deleteDepartmentList(deleteList).then(response => {
            if (response.data.ResultCode !== 0) {
              Message.error(response.data.Message)
            } else {
              this.getDepartments()
              this.$message({
                type: 'success',
                message: i18n.t('commonLangZh.deleteSuccessMsg')
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
        this.treeSelectList.push(data[i].menuId)
      }
    },
    tableSelectAllHandler(data) {
      this.treeSelectList = []
      for (let i = 0; i < data.length; i++) {
        this.treeSelectList.push(data[i].menuId)
      }
    },
    filterHandler(event) {
      console.log(event)
      this.getDepartments()
    },
    filteCancel(event) {
      this.searchData = event
      console.log(event)
    },
    getDepartments() {
      this.listLoading = true
      getDepartmentList(this.searchData).then(response => {
        console.log(response)
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          this.departmentList = this.setTableData(response.data.Data)
          this.listLoading = false
          this.total = response.data.total_count
        }
      })
    },
    setTableData(data) {
      const menuTableList = []
      for (let i = 0; i < (data || []).length; i++) {
        const tempObj = deepClone(data[i])
        if ((tempObj.list || []).length !== 0) {
          tempObj.children = this.setTableData(tempObj.list)
        }
        delete tempObj.list
        menuTableList.push(tempObj)
      }
      return menuTableList
    },
    handleDelete({ scope, row }) {
      this.$confirm(
        i18n.t('departmentManagement.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          console.log(this.deleteQuery)
          this.deleteQuery.ID = row.ID
          await deleteDepartmentList(this.deleteQuery).then(response => {
            this.getDepartments()
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
