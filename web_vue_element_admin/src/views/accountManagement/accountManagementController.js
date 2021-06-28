import { deepClone } from '@/utils'
import {
  addManager,
  deleteRole,
  updateManager,
  getManagers,
  getSelectRoles
} from '@/api/role'
import i18n from '@/lang'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import accountManagementModel from './accountManagementModel.js'
import { Message } from 'element-ui'

const defaultRole = {
  UserID: '',
  TrueName: '',
  Password: '',
  confirmPassword: '',
  status: i18n.t('commonLangZh.radioLabelStatusOne')
}

export default {
  components: { Pagination },
  directives: { elDragDialog },
  data() {
    return accountManagementModel
  },
  created() {
    this.getManagers()
    this.getSelectRoles()
  },
  methods: {
    async getSelectRoles() {
      const res = await getSelectRoles()
      console.log(res.data.list)
      for (const item of res.data.list) {
        const obj = {}
        obj.key = item.roleId
        obj.label = item.roleName
        this.transferData.push(obj)
      }
    },
    getManagers() {
      this.listLoading = true
      getManagers(this.listQuery).then(response => {
        this.listLoading = false
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
          console.log(response.data.Message)
        } else {
          for (let i = 0; i < (response.data.Data || []).length; i++) {
            const roles = []
            // if (response.data.Data[i].role.indexOf('admin') !== -1) {
            //   roles.push(i18n.t('managersList.admin'))
            // }
            // if (response.data.Data[i].role.indexOf('user') !== -1) {
            //   roles.push(i18n.t('managersList.user'))
            // }
            response.data.Data[i].role = roles.join(',')
          }
          this.rolesList = response.data.Data
          this.total = response.data.total_count
        }
      })
    },
    resetForm(formName, dialogType) {
      if (dialogType !== 'edit') {
        this.$refs[formName].resetFields()
        this.selectValue = []
        this.stepOne = true
        this.stepTwo = false
        this.passForm.show.check = false
        this.passForm.show.new = false
      }
    },
    next() {
      if (this.role.Password !== this.role.confirmPassword) {
        Message.error(i18n.t('commonLangZh.comfirmPasswordMsg'))
        return
      }
      if (this.active < 1) {
        this.active++
      }
      if (this.active === 1) {
        this.stepOne = false
        this.stepTwo = true
      }
    },
    back() {
      if (this.active > 0) {
        this.active--
      }
      if (this.active === 0) {
        this.stepOne = true
        this.stepTwo = false
      }
    },
    chooseStatus(scope) {
      return scope.row.status === 1
    },
    handleAddRole() {
      this.role = Object.assign({}, defaultRole)
      this.dialogType = 'new'
      this.dialogVisible = true
      this.active = 0
      this.stepOne = true
      this.stepTwo = false
    },
    handleDeleteRole() {
      console.log(1)
    },
    handleSearchRole() {
      console.log(this.searchData.input)
      this.listQuery.key = this.searchData.input
      this.getManagers()
    },
    handleEdit(scope) {
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.checkStrictly = true
      this.passForm.show.check = false
      this.passForm.show.new = false
      this.role = deepClone(scope.row)
      this.role.Password = null
      this.role.status =
        this.role.status === 1
          ? i18n.t('commonLangZh.radioLabelStatusTwo')
          : i18n.t('commonLangZh.radioLabelStatusOne')
      this.active = 0
      this.stepOne = true
      this.stepTwo = false
      this.selectValue = this.role.roleIdList ? this.role.roleIdList : []
    },
    handleDelete({ $index, row }) {
      this.$confirm(
        i18n.t('managersList.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          this.deleteQuery.ID = row.UID
          await deleteRole(this.deleteQuery).then(response => {
            this.getManagers()
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.deleteSuccessMsg')
            })
          })
        })
        .catch(err => {
          console.error(err)
        })
    },
    async confirmRole() {
      const isEdit = this.dialogType === 'edit'
      const postData = Object.assign({}, this.role)
      postData.Jurisdiction = this.role.status === i18n.t('commonLangZh.radioLabelStatusTwo') ? 1 : 0
      postData.OpenId = this.selectValue[0]
      delete postData.status
      delete postData.confirmPassword
      if (isEdit) {
        await updateManager({ methodName: 'SysGroupUserManagementUpdate', id: this.role.UserID }, postData).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            const { Message } = response.data
            console.log(Message)
            this.dialogVisible = false
            this.$notify({
              title: i18n.t('commonLangZh.success'),
              dangerouslyUseHTMLString: true,
              message: Message,
              type: 'success'
            })
            this.getManagers()
          }
        })
      } else {
        await addManager({ methodName: 'SysGroupUserManagementAdd' }, postData).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            const { Message } = response.data
            console.log(Message)
            this.dialogVisible = false
            this.$notify({
              title: i18n.t('commonLangZh.success'),
              dangerouslyUseHTMLString: true,
              message: Message,
              type: 'success'
            })
            this.getManagers()
          }
        })
      }
    }
  }
}
