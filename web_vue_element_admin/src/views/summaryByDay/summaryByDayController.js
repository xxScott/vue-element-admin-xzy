import {
  getSummaryByDay
} from '@/api/summaryByDay'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import summaryByDayModel from './summaryByDayModel'

export default {
  components: { Pagination, FilterContainer },
  data() {
    return summaryByDayModel
  },
  created() {
    this.getSummaryByDay()
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
    getSummaryByDay() {
      this.listLoading = true
      getSummaryByDay(this.listQuery).then(response => {
        this.summaryByDay = response.data.page.list
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
