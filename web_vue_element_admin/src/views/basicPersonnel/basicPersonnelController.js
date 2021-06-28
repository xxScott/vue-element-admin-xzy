import { deepClone } from '@/utils'
import {
  addManager,
  deleteRole,
  updateManager,
  getManagers
} from '@/api/role'
import i18n from '@/lang'
import Pagination from '@/components/Pagination'
import elDragDialog from '@/directive/el-dragDialog'
import basicPersonnelModel from './basicPersonnelModel.js'
import { Message } from 'element-ui'

const defaultRole = {
  UserName: '',
  WeChart: '',
  Phone: '',
  status: i18n.t('commonLangZh.radioLabelStatusTwo')
}

export default {
  components: { Pagination },
  directives: { elDragDialog },
  data() {
    return basicPersonnelModel
  },
  created() {
    this.getManagers()
  },
  methods: {
    getManagers() {
      this.listLoading = true
      getManagers(this.listQuery).then(response => {
        this.listLoading = false
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          for (let i = 0; i < (response.data.Data || []).length; i++) {
            const roles = []
            // if (response.data.Data[i].role.indexOf('admin') !== -1) {
            //   roles.push(i18n.t('basicPersonnel.admin'))
            // }
            // if (response.data.Data[i].role.indexOf('user') !== -1) {
            //   roles.push(i18n.t('basicPersonnel.user'))
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
      }
    },
    chooseStatus(scope) {
      return scope.row.status === 1
    },
    handleAddRole() {
      this.role = Object.assign({}, defaultRole)
      this.dialogType = 'new'
      this.dialogVisible = true
    },
    handleDeleteRole() {
      console.log(1)
    },
    handleSearchRole() {
      console.log(2)
    },
    handleEdit(scope) {
      this.dialogType = 'edit'
      this.dialogVisible = true
      this.checkStrictly = true
      this.role = deepClone(scope.row)
      this.role.status =
        this.role.status === 1
          ? i18n.t('commonLangZh.radioLabelStatusTwo')
          : i18n.t('commonLangZh.radioLabelStatusOne')
    },
    handleDelete({ $index, row }) {
      this.$confirm(
        i18n.t('basicPersonnel.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          this.deleteQuery.ID = row.ID
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
      delete postData.status
      if (isEdit) {
        await updateManager({ methodName: 'TcAccountManagementUpdate', id: this.role.ID }, postData).then(response => {
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
        await addManager({ methodName: 'TcAccountManagementAdd' }, postData).then(response => {
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
