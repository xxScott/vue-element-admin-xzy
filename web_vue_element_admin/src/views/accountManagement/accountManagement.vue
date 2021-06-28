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
        <el-button type="primary" @click="handleAddRole">{{ $t('managersList.addManager') }}</el-button>
        <el-button type="danger" @click="handleDeleteRole">{{ $t('managersList.deleteManager') }}</el-button>
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
          <el-table-column align="center" :label="$t('managersList.managerStatus')" width="100">
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
      :title="dialogType==='edit'? $t('managersList.modifyManager') : $t('managersList.addManager')"
      @close="loginDialog=false,resetForm('dialogForm', dialogType)"
    >
      <el-row :gutter="20" class="row-bg">
        <el-col>
          <el-steps :active="active" finish-status="success" simple>
            <el-step :title="$t('managersList.basic')"/>
            <el-step :title="$t('managersList.roles')"/>
          </el-steps>
        </el-col>
      </el-row>
      <div v-show="stepOne" style="width:85%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-form ref="dialogForm" :model="role" label-width="80px" label-position="left">
              <el-form-item :label="$t('managersList.UserID')">
                <el-input v-model="role.UserID" :placeholder="$t('managersList.UserID')"/>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerName')">
                <el-input v-model="role.TrueName" :placeholder="$t('managersList.managerName')"/>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerPassword')">
                <el-input :type="passForm.show.new?'text':'password'" v-model="role.Password">
                  <svg-icon
                    :icon-class="passForm.show.new? 'passwordOK':'passwordNO'"
                    slot="suffix"
                    alt
                    style="margin: 5px 0;font-size: 25px;"
                    @click="passForm.show.new=!passForm.show.new"
                  />
                </el-input>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerConfirmPassword')">
                <el-input
                  :type="passForm.show.check?'text':'password'"
                  v-model="role.confirmPassword"
                >
                  <svg-icon
                    :icon-class="passForm.show.check? 'passwordOK':'passwordNO'"
                    slot="suffix"
                    alt
                    style="margin: 5px 0;font-size: 25px;"
                    @click="passForm.show.check=!passForm.show.check"
                  />
                </el-input>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerStatus')">
                <el-radio-group v-model="role.status">
                  <el-radio-button :label="$t('commonLangZh.radioLabelStatusOne')"/>
                  <el-radio-button :label="$t('commonLangZh.radioLabelStatusTwo')"/>
                </el-radio-group>
              </el-form-item>
            </el-form>
          </el-col>
        </el-row>
      </div>
      <el-row v-show="stepOne" :gutter="20" class="row-bg">
        <el-col>
          <div style="text-align:right;">
            <el-button type="danger" @click="dialogVisible=false">{{ $t('commonLangZh.cancel') }}</el-button>
            <el-button type="primary" @click="next">{{ $t('commonLangZh.stepNext') }}</el-button>
          </div>
        </el-col>
      </el-row>
      <div v-show="stepTwo" style="width:85%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-transfer
              v-model="selectValue"
              :data="transferData"
              :titles="[$t('managersList.transferOne'), $t('managersList.transferTwo')]"
            />
          </el-col>
        </el-row>
      </div>
      <el-row v-show="stepTwo" :gutter="20" class="row-bg">
        <el-col>
          <div style="text-align:right;">
            <el-button type="danger" @click="dialogVisible=false">{{ $t('commonLangZh.cancel') }}</el-button>
            <el-button type="primary" @click="back">{{ $t('commonLangZh.stepBack') }}</el-button>
            <el-button type="primary" @click="confirmRole">{{ $t('commonLangZh.confirm') }}</el-button>
          </div>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script src="./accountManagementController.js" async="async"></script>

<style lang="scss" scoped src="./accountManagement.css">
</style>
