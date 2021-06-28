import {
  getEvaluationList
} from '@/api/evaluationDetails'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import evaluationDetailsModel from './evaluationDetailsModel.js'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return evaluationDetailsModel
  },
  created() {
    this.getEvaluationDetailsList()
  },
  methods: {
    handleExport() {
      console.log('1')
    },
    filterHandler(event) {
      console.log(event)
    },
    filteCancel(event) {
      this.searchData = event
      console.log(event)
    },
    getEvaluationDetailsList() {
      this.listLoading = true
      getEvaluationList(this.listQuery).then(response => {
        this.ordersList = response.data.page.list
        // eslint-disable-next-line no-empty
        this.total = response.data.page.totalCount

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    }
  }
}
