<template>
  <div class="app-container">
    <el-row :gutter="20">
      <el-col :span="5">
        <el-form ref="searchForm" :model="searchData">
          <el-input v-model="searchData.input" :placeholder="$t('commonLangZh.searchPlaceHold')">
            <el-button slot="append" icon="el-icon-search" @click="handleSearchRole"/>
          </el-input>
        </el-form>
      </el-col>
      <el-col :span="8">
        <el-button type="primary" @click="handleAddRole">{{ $t('basicPersonnel.addManager') }}</el-button>
        <el-button type="danger" @click="handleDeleteRole">{{ $t('basicPersonnel.deleteManager') }}</el-button>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="roles-table">
      <el-col>
        <el-table :data="rolesList" style="width: 100%;" border v-loading="listLoading">
          <el-table-column align="center" type="index" width="30"></el-table-column>
          <el-table-column align="center" width="40" type="selection"/>
          <el-table-column
            v-for="(item, index) in columnList"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          />
          <el-table-column align="center" :label="$t('basicPersonnel.managerStatus')" width="100">
            <template slot-scope="scope">
              <div v-if="chooseStatus(scope)" :title="$t('commonLangZh.radioLabelStatusTwo')">
                <svg-icon icon-class="normal" style="font-size: 25px;"/>
              </div>
              <div v-if="!chooseStatus(scope)" :title="$t('commonLangZh.radioLabelStatusOne')">
                <svg-icon icon-class="forbidden" style="font-size: 25px;"/>
              </div>
            </template>
          </el-table-column>
          <el-table-column align="center" :label="$t('commonLangZh.operation')" width="170">
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
          @pagination="getManagers"
        />
      </el-col>
    </el-row>

    <el-dialog
      v-el-drag-dialog
      :visible.sync="dialogVisible"
      :title="dialogType==='edit'? $t('basicPersonnel.modifyManager') : $t('basicPersonnel.addManager')"
      @close="loginDialog=false,resetForm('dialogForm', dialogType)"
    >
      <div style="width:85%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-form ref="dialogForm" :model="role" label-width="80px" label-position="left">
              <el-form-item :label="$t('basicPersonnel.managerID')">
                <el-input v-model="role.workId" :placeholder="$t('basicPersonnel.managerID')"/>
              </el-form-item>
              <el-form-item :label="$t('basicPersonnel.managerName')">
                <el-input v-model="role.UserName" :placeholder="$t('basicPersonnel.managerName')"/>
              </el-form-item>
              <el-form-item :label="$t('basicPersonnel.managerWechart')">
                <el-input v-model="role.WeChart" :placeholder="$t('basicPersonnel.managerWechart')"/>
              </el-form-item>
              <el-form-item :label="$t('basicPersonnel.managerPhone')">
                <el-input v-model="role.Phone" :placeholder="$t('basicPersonnel.managerPhone')"/>
              </el-form-item>
              <el-form-item :label="$t('basicPersonnel.managerStatus')">
                <el-radio-group v-model="role.status">
                  <el-radio-button :label="$t('commonLangZh.radioLabelStatusOne')"/>
                  <el-radio-button :label="$t('commonLangZh.radioLabelStatusTwo')"/>
                </el-radio-group>
              </el-form-item>
            </el-form>
          </el-col>
        </el-row>
      </div>
      <el-row :gutter="20" class="row-bg">
        <el-col>
          <div style="text-align:right;">
            <el-button type="danger" @click="dialogVisible=false">{{ $t('commonLangZh.cancel') }}</el-button>
            <el-button type="primary" @click="confirmRole">{{ $t('commonLangZh.confirm') }}</el-button>
          </div>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script src="./basicPersonnelController.js" async="async"></script>

<style lang="scss" scoped src="./basicPersonnel.css">
</style>