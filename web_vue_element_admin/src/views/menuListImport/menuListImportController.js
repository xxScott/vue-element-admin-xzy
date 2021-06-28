import {
  deleteOrder,
  updataOrder
} from '@/api/orders'
import { getMenuList } from '@/api/menu'
import i18n from '@/lang'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import menuListImportModel from './menuListImportModel.js'
import { Message } from 'element-ui'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return menuListImportModel
  },
  created() {
    this.getMenu()
  },
  methods: {
    addHandler() {
    },
    deleteHandler() {},
    chooseStatus(status) {
      if (status === 'true') {
        return true
      } else {
        return false
      }
    },
    handleExport() {
      console.log('1')
    },
    async tagTakeMealsClick(scope) {
      await updataOrder(scope.row.id).then(response => {
        if (response.data.data === 'success') {
          for (let i = 0; i < (this.menuList || []).length; i++) {
            if (this.menuList[i].id === scope.row.id) {
              this.menuList[i].takeMeals = this.menuList[i].takeMeals === '0' ? '1' : '0'
            }
          }
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
    },
    filterHandler(event) {
      console.log(event)
    },
    filteCancel(event) {
      this.searchData = event
      console.log(event)
    },
    getMenu() {
      this.listLoading = true
      getMenuList(this.listQuery).then(response => {
        this.listLoading = false
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          for (let i = 0; i < (response.data.Data || []).length; i++) {
          }
          this.menuList = response.data.Data
          this.total = response.data.total_count
        }
      })
    },
    handleEdit() {},
    handleDelete({ $index, row }) {
      this.$confirm(
        i18n.t('orderDetails.orderCancelMsg'),
        i18n.t('commonLangZh.warning'),
        {
          confirmButtonText: i18n.t('commonLangZh.confirm'),
          cancelButtonText: i18n.t('commonLangZh.cancel'),
          type: 'warning'
        }
      )
        .then(async() => {
          this.deleteQuery.ID = row.ID
          await deleteOrder(this.deleteQuery).then(response => {
            this.getMenu()
            if (response.data.data === 'success') {
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
    }
  }
}
