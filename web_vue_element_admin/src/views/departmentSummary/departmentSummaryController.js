import {
  getDepartmentSummary
} from '@/api/department'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import departmentSummaryModel from './departmentSummaryModel'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return departmentSummaryModel
  },
  created() {
    this.getDepartmentSummary()
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
    getDepartmentSummary() {
      this.listLoading = true
      getDepartmentSummary(this.listQuery).then(response => {
        this.departmentSummary = response.data.page.list
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
