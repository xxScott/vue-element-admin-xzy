<template>
  <div id="mealManagementId" class="app-container">
    <el-row :gutter="20">
      <el-col :span="8">
        <el-button type="primary" @click="handleAddRole">{{ $t('mealManagement.add') }}</el-button>
        <el-button type="danger" @click="handleDeleteRole">{{ $t('mealManagement.delete') }}</el-button>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="roles-table" style="width: 100%">
      <el-col>
        <el-table v-loading="listLoading" :data="mealManagement" width="100%" border>
          <el-table-column align="center" type="index" width="30"/>
          <el-table-column align="center" width="40" type="selection"/>
          <el-table-column
            v-for="(item, index) in columnList"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          />
          <el-table-column align="center" :label="$t('commonLangZh.operation')" width="200px">
            <template slot-scope="scope">
              <el-button
                type="primary"
                size="small"
                @click="handleEdit(scope)"
              >{{ $t('commonLangZh.modify') }}</el-button>
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
          @pagination="getMealManagement"
        />
      </el-col>
    </el-row>
    <el-dialog
      v-el-drag-dialog
      :visible.sync="dialogFormVisible"
      :title="dialogType==='edit'? $t('mealManagement.editMeal') : $t('mealManagement.addMeal')"
      @close="loginDialog=false,resetForm('dialogForm', dialogType)"
    >
      <div style="width:100%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-form :model="form" label-width="120px" label-position="left" ref="dialogForm">
              <el-form-item :label="$t('mealManagement.meal')">
                <el-input v-model="form.Name" autocomplete="off" style="width: 350px"></el-input>
              </el-form-item>
              <el-form-item :label="$t('mealManagement.expiration')">
                <div class="block">
                  <el-date-picker
                    v-model="form.reservation"
                    type="daterange"
                    range-separator="-"
                    :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
                    :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
                  ></el-date-picker>
                </div>
              </el-form-item>
              <el-form-item :label="$t('mealManagement.fetch')">
                <div class="block">
                  <el-date-picker
                    v-model="form.fetch"
                    type="daterange"
                    range-separator="-"
                    :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
                    :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
                  ></el-date-picker>
                </div>
              </el-form-item>
            </el-form>
          </el-col>
          <div style="text-align: right;">
            <el-button type="primary" @click="confirmRole">{{ $t('commonLangZh.confirm') }}</el-button>
            <el-button type="danger" @click="dialogFormVisible = false">{{ $t('commonLangZh.cancel') }}</el-button>
          </div>
        </el-row>
      </div>
    </el-dialog>
  </div>
</template>

<script src="./mealManagementController.js" async="async"></script>

<style lang="scss" scoped src="./mealManagement.css">
</style>
