import {
  getOrdersList,
  deleteOrder,
  updataOrder
} from '@/api/orders'
import i18n from '@/lang'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import orderDetailsModel from './orderDetailsModel.js'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return orderDetailsModel
  },
  created() {
    this.getOrders()
  },
  methods: {
    orderIdChangeHandler() {
      const context = this
      let timer = null
      clearTimeout(timer)
      timer = setTimeout(function() {
        console.log(context.orderId)
        context.Query.ID = context.orderId
        getOrdersList(context.Query).then(response => {
          context.ordersList = response.data.Data
          // eslint-disable-next-line no-empty
          context.total = response.data.total_count
        })
      }, 1000)
    },
    tableSelectHandler(data) {
      this.tableSelectList = []
      for (let i = 0; i < data.length; i++) {
        this.tableSelectList.push(data[i].id)
      }
    },
    tableSelectAllHandler(data) {
      this.tableSelectList = []
      for (let i = 0; i < data.length; i++) {
        this.tableSelectList.push(data[i].id)
      }
    },
    deleteHandler(scope) {
      if (this.tableSelectList.length === 0) {
        this.$alert(
          i18n.t('commonLangZh.beforeDeleteMsg'),
          {
            confirmButtonText: i18n.t('commonLangZh.confirm'),
            type: 'info'
          }
        )
        return
      }
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
          this.deleteQuery.ID = scope.row.ID
          await deleteOrder(this.deleteQuery).then(response => {
            if (response.data.data === 'success') {
              this.getOrders()
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
    handleExport() {
      console.log('1')
    },
    async tagTakeMealsClick(scope) {
      await updataOrder({ methodName: 'TcOrderManagerQRRead', id: scope.row.ID }).then(response => {
        if (response.data.ResultCode === 0) {
          for (let i = 0; i < (this.ordersList || []).length; i++) {
            if (this.ordersList[i].id === scope.row.ID) {
              this.ordersList[i].OrderStatus = this.ordersList[i].OrderStatus === '0' ? '1' : '0'
            }
          }
          this.$message({
            type: 'success',
            message: i18n.t('commonLangZh.updataSuccessMsg')
          })
          this.getOrders()
        } else {
          this.$message({
            type: 'error',
            message: response.data.Message
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
    getOrders() {
      this.listLoading = true
      getOrdersList(this.listQuery).then(response => {
        this.listLoading = false
        this.ordersList = response.data.Data
        // eslint-disable-next-line no-empty
        this.total = response.data.total_count
        // Just to simulate the time of the request1.5 * 1000)
      })
    },
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
            this.getOrders()
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
