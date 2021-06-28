<template>
  <div id="departmentManagementId" class="app-container">
    <filter-container
      :filterData="searchData"
      @selectedFilterConfirm="filterHandler"
      @selectedFilterCancel="filteCancel"
    >
      <el-form
        ref="dialogForm"
        :model="searchData"
        label-width="100px"
        label-position="left"
        :inline="true"
      >
        <el-form-item :label="$t('departmentManagement.departmentName')" style="width: 30%;">
          <el-input
            v-model="searchData.departmentName"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('departmentManagement.departmentPerson')" style="width: 30%;">
          <el-input
            v-model="searchData.departmentPerson"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
      </el-form>
    </filter-container>

    <el-row :gutter="20" class="roles-table">
      <el-col class="bottonClass">
        <el-button type="primary" @click="addHandler">{{ $t('commonLangZh.add') }}</el-button>
        <el-button type="danger" @click="deleteHandler">{{ $t('commonLangZh.delete') }}</el-button>
      </el-col>
      <el-col>
        <tree-table
          :data="departmentList"
          :columns="columnList"
          v-loading="listLoading"
          border
          @select="tableSelectHandler"
          @select-all="tableSelectAllHandler"
        >
          <template slot="selection">
            <el-table-column type="selection" align="center" width="40"/>
          </template>
          <template slot="operation" slot-scope="{scope}">
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
        </tree-table>
      </el-col>
    </el-row>
    <el-dialog
      v-el-drag-dialog
      :visible.sync="dialogVisible"
      :title="dialogType==='edit'? $t('departmentManagement.modifyDepartment') : $t('departmentManagement.addDepartment')"
      @close="loginDialog=false,resetForm('dialogForm', dialogType)"
    >
      <div style="width:85%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-form ref="dialogForm" :model="department" label-width="100px" label-position="left">
              <el-form-item :label="$t('departmentManagement.departmentName')">
                <el-input v-model="department.Name"/>
              </el-form-item>
              <el-form-item :label="$t('departmentManagement.departmentCode')">
                <el-input v-model="department.Code"/>
              </el-form-item>
              <el-form-item :label="$t('departmentManagement.departmentPerson')">
                <el-input v-model="department.departmentPerson"/>
              </el-form-item>
              <el-form-item :label="$t('departmentManagement.remark')">
                <el-input v-model="department.remark"/>
              </el-form-item>
              <el-form-item
                :label="$t('departmentManagement.superiorDepartment')"
                v-show="!editNotShow"
              >
                <el-popover
                  placement="right-start"
                  width="300"
                  trigger="click"
                  @show="reset()"
                  popper-class="popper-class"
                >
                  <div class="ui-fas">
                    <el-tree
                      :data="treeData"
                      node-key="id"
                      ref="tree"
                      highlight-current
                      :props="defaultProps"
                      :default-expanded-keys="defaultExpandedList"
                    ></el-tree>
                    <el-button
                      style="margin-top: 20px;margin-left:60px"
                      type="primary"
                      @click="popoverConfirm"
                    >{{ $t('commonLangZh.confirm') }}</el-button>
                    <el-button type="danger" @click="popoverCancel">{{ $t('commonLangZh.cancel') }}</el-button>
                  </div>
                  <el-input
                    slot="reference"
                    readonly
                    v-model="department.superiorDepartment"
                    style="cursor: pointer;"
                  >
                    <el-button slot="append" icon="el-icon-menu" @click="handleSelectMenu"/>
                  </el-input>
                </el-popover>
              </el-form-item>
            </el-form>
          </el-col>
        </el-row>
      </div>
      <el-row :gutter="20" class="row-bg">
        <el-col>
          <div style="text-align:right;">
            <el-button type="primary" @click="confirmHandler">{{ $t('commonLangZh.confirm') }}</el-button>
            <el-button type="danger" @click="dialogVisible=false">{{ $t('commonLangZh.cancel') }}</el-button>
          </div>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script src="./departmentManagementController.js" async="async"></script>

<style lang="scss" scoped src="./departmentManagement.css">
</style>
