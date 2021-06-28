import {
  getEvaluationDetailsList,
  getMealsList,
  addOrder
} from '@/api/evaluationDetails'
import {
  getMealManagement
} from '@/api/mealManagement'
import Pagination from '@/components/Pagination'
import FilterContainer from '@/components/filterContainer'
import substitutionForMealsModel from './substitutionForMealsModel.js'
import i18n from '@/lang'
import { Message } from 'element-ui'

const defaultStaffForm = {
  departmentName: [],
  keyStaff: ''
}

export default {
  components: { Pagination, FilterContainer },
  data() {
    return substitutionForMealsModel
  },
  created() {
    this.getMealManagement()
    this.getEvaluationDetailsList()
    this.allcheckedHandler()
  },
  methods: {
    getMealManagement() {
      getMealManagement(this.mealListQuery).then(response => {
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          response.data.Data.map((item) => {
            const obj = {
              value: item.ID,
              label: item.Name
            }
            this.mealTypeList.push(obj)
          })
          this.stepTwoForm.mealType = (this.mealTypeList || []).length !== 0 ? this.mealTypeList[0].value : ''
          this.getMealsList()
        }
      })
    },
    mealTypeChangeHandler(event) {
      this.getMealsList()
    },
    upHandler() {
      const num = []
      const context = this
      this.multipleSelectedSelection.map(function(item) {
        for (let i = 0; i < (context.selectedStaffList || []).length; i++) {
          if (item.ID === context.selectedStaffList[i].ID) {
            num.push(i)
          }
        }
      })
      this.selectedStaffList.map(function(item, index) {
        if (num.indexOf(index) !== -1) {
          context.staffList.push(item)
        }
      })
      this.selectedStaffList = this.selectedStaffList.filter(el => !this.multipleSelectedSelection.includes(el))
    },
    downHandler() {
      const num = []
      const context = this
      const staffListClone = []
      this.multipleSelection.map(function(item) {
        for (let i = 0; i < (context.staffList || []).length; i++) {
          if (item.ID === context.staffList[i].ID) {
            num.push(i)
          }
        }
      })
      this.staffList.map(function(item, index) {
        if (num.indexOf(index) === -1) {
          staffListClone.push(item)
        }
      })
      this.staffList = staffListClone
      this.selectedStaffList = this.duplicateRemovalHandler(this.selectedStaffList.concat(this.multipleSelection))
    },
    duplicateRemovalHandler(list) {
      const x = new Set(list)
      return [...x]
    },
    selectedChangeFun(value) {
      this.multipleSelectedSelection = value
    },
    changeFun(value) {
      this.multipleSelection = value
      console.log(this.multipleSelection)
    },
    filterHandler(event) {
      console.log(event)
    },
    filteCancel(event) {
      this.staffForm = Object.assign({}, defaultStaffForm)
      this.allcheckedHandler()
      console.log(event)
    },
    allcheckedHandler() {
      const selectAll = []
      for (let i = 0; i < (this.departmentNameSelectList || []).length; i++) {
        selectAll.push(this.departmentNameSelectList[i].value)
      }
      this.staffForm.departmentName = selectAll
      console.log(selectAll)
    },
    clickStepHandler(id) {
      this.activeStep = id
      this.title = this.$refs['step' + id].title
      if (this.activeStep === 1) {
        this.stepOne = true
        this.stepTwo = false
        this.stepThree = false
      }
      if (this.activeStep === 2) {
        this.stepOne = false
        this.stepTwo = true
        this.stepThree = false
      }
      if (this.activeStep === 3) {
        console.log(this.staffList)
        console.log(this.mealsNameHandler(this.mealsForm))
        this.stepOne = false
        this.stepTwo = false
        this.stepThree = true
        this.showMealsList = Object.assign({}, this.mealsNameHandler(this.mealsForm))
        console.log(this.showMealsList)
      }
    },
    totalCountShow() {
      let total = 0
      for (const i in this.showMealsList) {
        console.log(this.showMealsList[i])
        total = total + parseInt(i.split('/')[1].split('￥')[0]) * parseInt(this.showMealsList[i])
      }
      return total
    },
    mealsNameHandler(data) {
      const list = {}
      for (const i in data) {
        if (data[i] > 0) {
          list[i] = data[i]
        }
      }
      return list
    },
    onSubmit() {
      console.log('submit!')
      this.staffForm = Object.assign({}, defaultStaffForm)
      this.allcheckedHandler()
      this.getEvaluationDetailsList()
      const postData = {
        AccountManagementID: [],
        meal: []
      }
      for (let i = 0; i < this.selectedStaffList.length; i++) {
        postData.AccountManagementID.push({
          ID: this.selectedStaffList[i].ID
        })
      }
      console.log(this.selectedStaffList)
      for (const label in this.showMealsList) {
        postData.meal.push({
          mealName: label,
          count: this.showMealsList[label]
        })
      }
      console.log(this.showMealsList)
      postData.mealType = this.stepTwoForm.mealType
      postData.ThinkChange = this.orderForm.modelSelect
      postData.selectName = this.orderForm.selectName
      postData.fullName = this.orderForm.fullName
      addOrder({ methodName: 'TcOrderManagerAdd' }, postData).then(response => {
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          this.dialogVisible = false
          this.selectedStaffList = []
          this.showMealsList = {}
          this.orderForm = {}
          // this.orderForm.modelSelect = i18n.t('substitutionForMeals.singleGive')
          for (let i = 0; i < (this.mealsList || []).length; i++) {
            this.setMealsCss(this.mealsList[i].ref)
          }
          this.stepOne = true
          this.stepTwo = false
          this.stepThree = false
          this.activeStep = 1
          this.$message({
            type: 'success',
            message: i18n.t('commonLangZh.addSuccessMsg')
          })
        }
      })
    },
    getMealsList() {
      getMealsList(this.mealTreeQuery).then(response => {
        if (response.data.ResultCode !== 0) {
          Message.error(response.data.Message)
        } else {
          const res = response.data.Data
          for (let i = 0; i < (res || []).length; i++) {
            if (res[i].ID === this.stepTwoForm.mealType) {
              this.mealsList = res[i].Type.map((item) => {
                const obj = {
                  title: item.Name,
                  name: parseInt(item.ID),
                  ref: item.ID,
                  model: this.mealsForm,
                  list: (item.Food || []).map((item) => {
                    return {
                      key: item.ID,
                      label: item.Name + '/' + item.Price + '￥'
                    }
                  })
                }
                return obj
              })
              break
            }
          }
        }
      }).then(() => {
        for (let i = 0; i < this.mealsList.length; i++) {
          this.mealsActive = this.mealsList[0].name
          this.setMealsCss(this.mealsList[i].ref)
        }
      })
    },
    getEvaluationDetailsList() {
      this.listLoading = true
      getEvaluationDetailsList(this.listQuery).then(response => {
        this.staffList = response.data.Data
        // eslint-disable-next-line no-empty
        this.total = response.data.total_count

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    setMealsCss(id) {
      for (let index = 0; index < (this.$refs[id][0].$el.children || []).length; index++) {
        this.$refs[id][0].$el.children[index].children[1].style.verticalAlign = 'middle'
        this.$refs[id][0].$el.children[index].style.marginRight = '100px'
      }
      for (const label in this.mealsForm) {
        this.mealsForm[label] = 0
      }
    }
  },
  mounted() {
    this.$refs.step1.$el.children[0].children[1].style.cursor = 'pointer'
    this.$refs.step1.$el.children[1].children[0].style.cursor = 'pointer'
    this.$refs.step2.$el.children[0].children[1].style.cursor = 'pointer'
    this.$refs.step2.$el.children[1].children[0].style.cursor = 'pointer'
    this.$refs.step3.$el.children[0].children[1].style.cursor = 'pointer'
    this.$refs.step3.$el.children[1].children[0].style.cursor = 'pointer'
  }
}
