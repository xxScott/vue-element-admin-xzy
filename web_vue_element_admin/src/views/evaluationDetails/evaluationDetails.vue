<template>
  <div id="evaluationDetailsId" class="app-container">
    <filter-container
      :filterData="searchData"
      @selectedFilterConfirm="filterHandler"
      @selectedFilterCancel="filteCancel"
    >
      <el-form
        ref="dialogForm"
        :model="searchData"
        label-width="110px"
        label-position="left"
        :inline="true"
      >
        <el-form-item
          :label="$t('evaluationDetails.dataTodata')"
          style="width: 60%;margin-right: 24px;"
        >
          <el-date-picker
            v-model="searchData.dataTodata"
            type="datetimerange"
            :range-separator="$t('commonLangZh.timeTo')"
            :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
            :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
          ></el-date-picker>
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.mealSeparation')" style="width: 30%;">
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
        <el-form-item :label="$t('evaluationDetails.userType')" style="width: 30%;">
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
        <el-form-item :label="$t('evaluationDetails.score')" style="width: 30%;">
          <el-select
            v-model="searchData.score"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in scoreOptions"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.appointedDate')" style="width: 30%;">
          <el-date-picker
            v-model="searchData.appointedDate"
            type="date"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          ></el-date-picker>
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.department')" style="width: 30%;">
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
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.fullName')" style="width: 30%;">
          <el-input
            v-model="searchData.fullName"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.nameOfPackage')" style="width: 30%;">
          <el-input
            v-model="searchData.nameOfPackage"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.evaluation')" style="width: 30%;">
          <el-input
            v-model="searchData.evaluation"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.terminalWindow')" style="width: 30%;">
          <el-select
            v-model="searchData.terminalWindow"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in terminalWindowOptions"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            ></el-option>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('evaluationDetails.orderStatus')" style="width: 30%;">
          <el-select
            v-model="searchData.orderStatus"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in orderStatusOptions"
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
        <el-button @click="handleExport">{{ $t('orderDetails.export') }}</el-button>
      </el-col>
      <el-col>
        <el-table :data="ordersList" border v-loading="listLoading">
          <el-table-column type="expand">
            <template slot-scope="props">
              <el-form label-position="left" inline class="table-expand">
                <el-form-item :label="item.columnLable" v-for="(item, index) in expandList" :key="index">
                  <span>{{ props.row[item.columnData] }}</span>
                </el-form-item>
                <el-form-item :label="$t('evaluationDetails.img')">
                  <img :src="props.row.img"  :alt="$t('evaluationDetails.img')" />
                </el-form-item>
              </el-form>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('evaluationDetails.orderId')" width="70px">
            <template slot-scope="scope">
              <span>{{ scope.row.id }}</span>
            </template>
          </el-table-column>
          <el-table-column
            v-for="(item, index) in columnList"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          />
          <el-table-column :label="$t('evaluationDetails.starRating')">
            <template slot-scope="scope">
              <svg-icon
                v-for="n in +scope.row.starRating"
                :key="n"
                icon-class="star"
                class="meta-item__icon"
              />
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('evaluationDetails.evaluation')">
            <template slot-scope="scope">
              <span>{{ scope.row.evaluation }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('evaluationDetails.remarks')">
            <template slot-scope="scope">
              <span>{{ scope.row.remarks }}</span>
            </template>
          </el-table-column>
        </el-table>
        <pagination
          v-show="total>0"
          :total="total"
          :page.sync="listQuery.page"
          :limit.sync="listQuery.limit"
          @pagination="getEvaluationDetailsList"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script src="./evaluationDetailsController.js" async="async"></script>

<style lang="scss" src="./evaluationDetails.css">
</style>
