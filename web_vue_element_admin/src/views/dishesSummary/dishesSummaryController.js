import {
  getDishesSummary
} from '@/api/dishes'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import dishesSummaryModel from './dishesSummaryModel'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return dishesSummaryModel
  },
  created() {
    this.getDishesSummary()
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
    getDishesSummary() {
      this.listLoading = true
      getDishesSummary(this.listQuery).then(response => {
        this.dishesSummary = response.data.page.list
        // eslint-disable-next-line no-empty
        this.total = response.data.page.totalCount

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    formatter(row, column) {
      return row.address
    }
  }
}
