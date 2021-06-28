<template>
  <div id="summaryByDayId" class="app-container">
    <filter-container
      :filter-data="searchData"
      @selectedFilterConfirm="filterHandler"
      @selectedFilterCancel="filteCancel"
    >
      <el-form
        ref="dialogForm"
        :model="searchData"
        label-width="80px"
        label-position="left"
        :inline="true"
      >
        <el-form-item :label="$t('summaryByDay.dataTodata')" >
          <el-date-picker
            v-model="searchData.dataTodata"
            type="datetimerange"
            :range-separator="$t('commonLangZh.timeTo')"
            :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
            :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
          />
        </el-form-item>
        <el-form-item :label="$t('summaryByDay.department')">
          <el-select
            v-model="searchData.department"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in departmentOptions"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('summaryByDay.appointedDate')">
          <el-date-picker
            v-model="searchData.appointedDate"
            type="date"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('summaryByDay.userType')">
          <el-select
            v-model="searchData.userType"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in userTypeOptions"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>
      </el-form>
    </filter-container>

    <el-row :gutter="20" class="roles-table">
      <el-col class="bottonClass">
        <el-button @click="handleExport">{{ $t('summaryByDay.export') }}</el-button>
      </el-col>
      <el-col>
        <el-table
          :data="summaryByDay"
          style="width: 100%"
        >
          <el-table-column
            prop="serial"
            align="center"
            label="序号"
            width="100"
          />
          <el-table-column
            prop="date"
            align="center"
            label="日期"
            width="100"
          />
          <el-table-column
            v-for="(item, index) in columnList1"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          >
            <el-table-column
              v-for="(item, index) in columnList2"
              :key="index"
              align="center"
              :label="item.columnLable"
              :width="item.width"
              :prop="item.columnData"
            />
          </el-table-column>
          <el-table-column label="小计" align="center">
            <el-table-column
              prop="number"
              align="center"
              label="数量"
              width="70"
            />
            <el-table-column
              prop="money"
              align="center"
              label="金额"
              width="70"
            />
            <el-table-column
              prop="fetch"
              align="center"
              label="已领"
              width="70"
            />
          </el-table-column>
        </el-table>
        <pagination
          v-show="total>0"
          :total="total"
          :page.sync="listQuery.page"
          :limit.sync="listQuery.limit"
          @pagination="getSummaryByDay"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script src="./summaryByDayController.js" async="async"></script>

<style lang="scss" scoped src="./summaryByDay.css">
</style>
