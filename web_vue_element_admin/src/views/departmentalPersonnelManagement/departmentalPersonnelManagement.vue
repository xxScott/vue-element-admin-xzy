<template>
  <div id="departmentalPersonnelManagementId" class="app-container">
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
        <el-form-item :label="$t('departmentalPersonnelManagement.name')" style="width: 30%;">
          <el-input
            v-model="searchData.name"
            :placeholder="$t('commonLangZh.searchPlaceHold')"
            style="width: 190px;"
          />
        </el-form-item>
        <el-form-item :label="$t('departmentalPersonnelManagement.department')" style="width: 30%;">
          <el-input
            v-model="searchData.department"
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
        <el-table
          :data="staffData"
          border
          v-loading="listLoading"
          @select="tableSelectHandler"
          @select-all="tableSelectAllHandler"
        >
          <el-table-column align="center" width="40" type="selection"/>
          <el-table-column
            v-for="(item, index) in columnList"
            :key="index"
            align="center"
            :label="item.columnLable"
            :width="item.width"
            :prop="item.columnData"
          />
          <el-table-column align="center" :label="$t('commonLangZh.operation')">
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
          @pagination="GetStaffList"
        />
      </el-col>
    </el-row>
    <el-dialog
      v-el-drag-dialog
      :visible.sync="dialogVisible"
      :title="dialogType==='edit'? $t('departmentalPersonnelManagement.modifyStaff') : $t('departmentalPersonnelManagement.addStaff')"
      @close="loginDialog=false,resetForm('dialogForm', dialogType)"
    >
      <div style="width:85%">
        <el-row :gutter="20" class="row-bg">
          <el-col :push="2">
            <el-form ref="dialogForm" :model="staff" label-width="110px" label-position="left">
              <el-form-item
                :label="$t('departmentalPersonnelManagement.name')"
                v-if="dialogType==='edit'"
              >
                <el-input v-model="staff.name" :disabled="dialogType==='edit'"/>
              </el-form-item>
              <el-form-item :label="$t('departmentalPersonnelManagement.name')" v-else>
                <el-select
                  v-model="staff.name"
                  collapse-tags
                  filterable
                  :placeholder="$t('commonLangZh.selectPlaceholder')"
                  style="width: 100%;"
                >
                  <el-option
                    v-for="(item, index) in staffList"
                    :key="index"
                    :label="item.label"
                    :value="item.value"
                  ></el-option>
                </el-select>
              </el-form-item>
              <el-form-item :label="$t('departmentalPersonnelManagement.workId')" v-if="dialogType==='edit'">
                <el-input v-model="staff.workId" :disabled="dialogType==='edit'"/>
              </el-form-item>
              <el-form-item :label="$t('departmentalPersonnelManagement.remark')">
                <el-input v-model="staff.remark"/>
              </el-form-item>
              <el-form-item :label="$t('departmentalPersonnelManagement.department')">
                <el-popover
                  placement="right-start"
                  width="300"
                  trigger="click"
                  popper-class="popper-class"
                >
                  <div class="ui-fas">
                    <el-tree
                      :data="treeData"
                      node-key="id"
                      ref="tree"
                      highlight-current
                      :props="defaultProps"
                      show-checkbox
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
                    v-model="staff.department"
                    style="cursor: pointer;"
                    @click.native="handleSelectMenu"
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

<script src="./departmentalPersonnelManagementController.js" async="async"></script>

<style lang="scss" scoped src="./departmentalPersonnelManagement.css">
</style>
