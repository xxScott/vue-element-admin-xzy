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
        <el-form-item
          :label="$t('menuListImport.dataTodata')"
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
        <el-form-item :label="$t('menuListImport.mealSeparation')" style="width: 30%;">
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
        <el-form-item :label="$t('menuListImport.id')" style="width: 30%;">
          <el-input
            v-model="searchData.id"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('menuListImport.mealName')" style="width: 30%;">
          <el-input
            v-model="searchData.mealName"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('menuListImport.status')" style="width: 30%;">
          <el-select
            v-model="searchData.status"
            :placeholder="$t('commonLangZh.selectPlaceholder')"
            style="width: 190px;"
          >
            <el-option
              v-for="item in statusOptions"
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
        <router-link :to="{path: '/menuManagementTo/add/'}">
          <el-button type="primary" @click="addHandler">{{ $t('commonLangZh.add') }}</el-button>
        </router-link>
        <el-button type="danger" @click="deleteHandler">{{ $t('commonLangZh.delete') }}</el-button>
        <el-button @click="handleExport">{{ $t('commonLangZh.exportIn') }}</el-button>
      </el-col>
      <el-col>
        <el-table :data="menuList" border v-loading="listLoading">
          <el-table-column align="center" width="40" type="selection"/>
          <el-table-column align="center" :label="$t('menuListImport.id')" width="70px">
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
          <el-table-column align="center" :label="$t('menuListImport.status')" width="100">
            <template slot-scope="scope">
              <div
                v-if="chooseStatus(scope.row.status)"
                :title="$t('commonLangZh.radioLabelStatusTwo')"
              >
                <svg-icon icon-class="normal" style="font-size: 25px;"/>
              </div>
              <div
                v-if="!chooseStatus(scope.row.status)"
                :title="$t('commonLangZh.radioLabelStatusOne')"
              >
                <svg-icon icon-class="forbidden" style="font-size: 25px;"/>
              </div>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('menuListImport.remark')">
            <template slot-scope="scope">
              <span>{{ scope.row.remark }}</span>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('commonLangZh.operation')" width="170">
            <template slot-scope="scope">
              <router-link :to="{path: '/menuManagementTo/edit/', query:scope.row}">
                <el-button
                  type="primary"
                  size="small"
                  @click="handleEdit(scope)"
                >{{ $t('commonLangZh.modify') }}</el-button>
              </router-link>
              <el-button
                type="danger"
                size="small"
                @click="handleDelete(scope)"
              >{{ $t('commonLangZh.delete') }}</el-button>
            </template>
          </el-table-column>
        </el-table>
        <pagination
          v-show="total>0"
          :total="total"
          :page.sync="listQuery.pageNumber"
          :limit.sync="listQuery.pageSize"
          @pagination="getMenu"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script src="./menuListImportController" async="async"></script>

<style lang="scss" scoped src="./menuListImport.css">
</style>
