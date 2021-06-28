<template>
  <div id="dishesSummaryId" class="app-container">
    <filter-container
      :filterData="searchData"
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
        <el-form-item :label="$t('dishesSummary.dataTodata')">
          <el-date-picker
            v-model="searchData.dataTodata"
            type="datetimerange"
            :range-separator="$t('commonLangZh.timeTo')"
            :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
            :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
          ></el-date-picker>
        </el-form-item>
        <el-form-item :label="$t('dishesSummary.mealSeparation')" style="width: 30%;">
          <el-select
            v-model="searchData.mealSeparation"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in mealSeparationOptions"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('dishesSummary.userType')" style="width: 30%;">
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
        <el-button @click="handleExport">{{ $t('dishesSummary.export') }}</el-button>
      </el-col>
      <el-col>
        <el-table :data="dishesSummary" border v-loading="listLoading" :default-sort = "{prop: 'evaluate', order: 'descending'}">
          <el-table-column
            v-for="(item, index) in columnList"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          />
          <el-table-column
            v-for="(item, index) in columnList1"
            :key="index + 10"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
            sortable
          />
        </el-table>
        <pagination
          v-show="total>0"
          :total="total"
          :page.sync="listQuery.page"
          :limit.sync="listQuery.limit"
          @pagination="getDishesSummary"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script src="./dishesSummaryController.js" async="async"></script>

<style lang="scss" scoped src="./dishesSummary.css">
</style>
