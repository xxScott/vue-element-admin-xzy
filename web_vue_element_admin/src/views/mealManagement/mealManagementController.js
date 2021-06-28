import {
  getMealManagement,
  deleteMeal,
  updateMeal,
  addMeal
} from '@/api/mealManagement'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import mealManagementModel from './mealManagementModel'
import elDragDialog from '@/directive/el-dragDialog'
import i18n from '@/lang'
import { Message } from 'element-ui'

const defaultFrom = {
  Name: '',
  reservation: '',
  fetch: ''
}

export default {
  components: { Pagination, FilterContainer },
  directives: { elDragDialog },
  data() {
    return mealManagementModel
  },
  created() {
    this.getMealManagement()
  },
  methods: {
    resetForm(formName, dialogType) {
      if (dialogType !== 'edit') {
        this.$refs[formName].resetFields()
      }
    },
    handleEdit(scope) {
      this.dialogType = 'edit'
      console.log(scope.row)
      this.dialogFormVisible = true
      this.form = Object.assign({}, defaultFrom)
      this.form.Name = scope.row.Name
      this.form.ID = scope.row.ID
    },
    handleExport(scope) {
      this.form = Object.assign({}, defaultFrom)
      this.dialogFormVisible = true
      console.log('1')
    },
    handleDeleteRole() {
      console.log(1)
    },
    handleAddRole() {
      this.form = Object.assign({}, defaultFrom)
      this.dialogType = 'new'
      this.dialogFormVisible = true
      console.log('1')
    },
    handleDelete({ $index, row }) {
      this.$confirm(
        i18n.t('mealManagement.deleteMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          this.deleteQuery.ID = row.ID
          await deleteMeal(this.deleteQuery).then(response => {
            this.getMealManagement()
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
    getMealManagement() {
      this.listLoading = true
      getMealManagement(this.listQuery).then(response => {
        this.listLoading = false
        this.mealManagement = response.data.Data
        // eslint-disable-next-line no-empty
        this.total = response.data.total_count
      })
    },
    async confirmRole() {
      const isEdit = this.dialogType === 'edit'
      const postData = Object.assign({}, this.form)
      console.log(this.form)
      if (isEdit) {
        await updateMeal({ methodName: 'TcMealManagementUpdate', id: this.form.ID }, postData).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            const { Message } = response.data
            console.log(Message)
            this.dialogFormVisible = false
            this.$notify({
              title: i18n.t('commonLangZh.success'),
              dangerouslyUseHTMLString: true,
              message: Message,
              type: 'success'
            })
            this.getMealManagement()
          }
        })
      } else {
        await addMeal({ methodName: 'TcMealManagementAdd' }, postData).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            const { Message } = response.data
            console.log(Message)
            this.dialogFormVisible = false
            this.$notify({
              title: i18n.t('commonLangZh.success'),
              dangerouslyUseHTMLString: true,
              message: Message,
              type: 'success'
            })
            this.getMealManagement()
          }
        })
      }
    }
  }
}
