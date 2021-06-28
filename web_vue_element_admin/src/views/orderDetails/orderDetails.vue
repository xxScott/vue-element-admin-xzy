<template>
  <div id="orderDetailsId" class="app-container">
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
        <el-form-item :label="$t('orderDetails.dataTodata')" style="width: 60%;margin-right: 24px;">
          <el-date-picker
            v-model="searchData.dataTodata"
            type="datetimerange"
            :range-separator="$t('commonLangZh.timeTo')"
            :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
            :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
          ></el-date-picker>
        </el-form-item>
        <el-form-item :label="$t('orderDetails.mealSeparation')" style="width: 30%;">
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
        <el-form-item :label="$t('orderDetails.userType')" style="width: 30%;">
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
        <el-form-item :label="$t('orderDetails.orderStatus')" style="width: 30%;">
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
        <el-form-item :label="$t('orderDetails.appointedDate')" style="width: 30%;">
          <el-date-picker
            v-model="searchData.appointedDate"
            type="date"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          ></el-date-picker>
        </el-form-item>
        <el-form-item :label="$t('orderDetails.department')" style="width: 30%;">
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
      </el-form>
    </filter-container>

    <el-row :gutter="20" class="roles-table">
      <el-col>
        <el-form>
          <el-form-item :label="$t('orderDetails.orderId')" style="width: 30%;">
            <el-input
              v-model="orderId"
              :placeholder="$t('commonLangZh.searchPlaceHold')"
              style="width: 190px;"
              @input="orderIdChangeHandler"
            />
          </el-form-item>
        </el-form>
      </el-col>
      <el-col class="bottonClass">
        <el-button type="danger" @click="deleteHandler">{{ $t('commonLangZh.delete') }}</el-button>
        <el-button @click="handleExport">{{ $t('orderDetails.export') }}</el-button>
      </el-col>
      <el-col>
        <el-table
          :data="ordersList"
          border
          v-loading="listLoading"
          @select="tableSelectHandler"
          @select-all="tableSelectAllHandler"
        >
          <el-table-column type="expand">
            <template slot-scope="props">
              <el-form label-position="left" inline class="table-expand">
                <el-form-item :label="item.columnLable" v-for="(item, index) in expandList" :key="index">
                  <span>{{ props.row[item.columnData] }}</span>
                </el-form-item>
              </el-form>
            </template>
          </el-table-column>
          <el-table-column align="center" width="40" type="selection"/>
          <el-table-column align="center" :label="$t('orderDetails.orderId')" width="70px">
            <template slot-scope="scope">
              <span>{{ scope.row.OrderNumber }}</span>
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
          <el-table-column align="center" :label="$t('orderDetails.takeMeals')">
            <template slot-scope="scope">
              <el-button
                v-for="item in scope.row.OrderStatus"
                :type="scope.row.OrderStatus === '0' ? 'success' : 'danger'"
                :key="item.id"
                size="mini"
                @click.native="tagTakeMealsClick(scope)"
              >{{ scope.row.OrderStatus === '0' ? $t('orderDetails.ordered') : $t('orderDetails.unordered') }}</el-button>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('orderDetails.remarks')">
            <template slot-scope="scope">
              <span>{{ scope.row.remarks }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('commonLangZh.operation')">
            <template slot-scope="scope">
              <el-button
                size="small"
                type="danger"
                icon="el-icon-circle-close-outline"
                @click="handleDelete(scope)"
                :title="$t('orderDetails.cancelOrder')"
              ></el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination
          v-show="total>0"
          :total="total"
          :page.sync="listQuery.pageNumber"
          :limit.sync="listQuery.pageSize"
          @pagination="getOrders"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script src="./orderDetailsController.js" async="async"></script>

<style lang="scss" scoped src="./oderDetails.css">
</style>
