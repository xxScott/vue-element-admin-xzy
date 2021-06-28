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
          :page.sync="listQuery.page"
          :limit.sync="listQuery.limit"
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
              <el-form-item :label="$t('managersList.managerName')">
                <el-input v-model="role.username" :placeholder="$t('managersList.managerName')"/>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerPassword')">
                <el-input
                  :type="passForm.show.new?'text':'password'"
                  v-model="role.password"
                >
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
              <el-form-item :label="$t('managersList.managerEmail')">
                <el-input v-model="role.email" :placeholder="$t('managersList.managerEmail')"/>
              </el-form-item>
              <el-form-item :label="$t('managersList.managerPhone')">
                <el-input v-model="role.mobile" :placeholder="$t('managersList.managerPhone')"/>
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

<script>
import path from "path";
import { deepClone } from "@/utils";
import {
  addRole,
  deleteRole,
  updateRole,
  getManagers,
  getSelectRoles
} from "@/api/role";
import i18n from "@/lang";
import Pagination from "@/components/Pagination";
import elDragDialog from '@/directive/el-dragDialog'

const defaultRole = {
  name: "",
  password: "",
  confirmPassword: "",
  email: "",
  phone: "",
  status: i18n.t("commonLangZh.radioLabelStatusOne")
};

const searchData = {
  input: ""
};

export default {
  components: { Pagination },
  directives: { elDragDialog },
  data() {
    return {
      passForm: {
        show: {
          new: false,
          check: false
        }
      },
      total: 0,
      listQuery: {
        page: 1,
        limit: 20
      },
      listLoading: true,
      selectValue: [],
      transferData: [],
      role: Object.assign({}, defaultRole),
      searchData: Object.assign({}, searchData),
      rolesList: [],
      dialogVisible: false,
      dialogType: "new",
      active: 0,
      stepOne: true,
      stepTwo: false,
      defaultProps: {
        children: "children",
        label: "title"
      },
      columnList: [
        {
          columnLable: this.$t("managersList.managerID"),
          width: 70,
          columnData: "userId"
        },
        {
          columnLable: this.$t("managersList.managerName"),
          width: 110,
          columnData: "username"
        },
        {
          columnLable: this.$t("managersList.managerEmail"),
          width: 170,
          columnData: "email"
        },
        {
          columnLable: this.$t("managersList.managerPhone"),
          width: 170,
          columnData: "mobile"
        },
        {
          columnLable: this.$t("managersList.managerCreateTime"),
          columnData: "createTime"
        }
      ]
    };
  },
  created() {
    this.getManagers();
    this.getSelectRoles();
  },
  methods: {
    async getSelectRoles() {
      const res = await getSelectRoles();
      console.log(res.data.list);
      for (const item of res.data.list) {
        const obj = {};
        obj.key = item.roleId;
        obj.label = item.roleName;
        this.transferData.push(obj);
      }
    },
    getManagers() {
      this.listLoading = true;
      getManagers(this.listQuery).then(response => {
        this.rolesList = response.data.page.list;
        this.total = response.data.page.totalCount;

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false;
        }, 1.5 * 1000);
      });
    },
    resetForm(formName, dialogType) {
      if (dialogType !== "edit") {
        this.$refs[formName].resetFields();
        this.selectValue = [];
        this.stepOne = true;
        this.stepTwo = false;
        this.passForm.show.check = false;
        this.passForm.show.new = false;
      }
    },
    next() {
      if (this.active < 1) {
        this.active++;
      }
      if (this.active === 1) {
        this.stepOne = false;
        this.stepTwo = true;
      }
    },
    back() {
      if (this.active > 0) {
        this.active--;
      }
      if (this.active === 0) {
        this.stepOne = true;
        this.stepTwo = false;
      }
    },
    chooseStatus(scope) {
      return scope.row.status === 1;
    },
    handleAddRole() {
      this.role = Object.assign({}, defaultRole);
      this.dialogType = "new";
      this.dialogVisible = true;
      this.active = 0;
      this.stepOne = true;
      this.stepTwo = false;
    },
    handleDeleteRole() {
      console.log(1);
    },
    handleSearchRole() {
      console.log(2);
    },
    handleEdit(scope) {
      this.dialogType = "edit";
      this.dialogVisible = true;
      this.checkStrictly = true;
      this.passForm.show.check = false;
      this.passForm.show.new = false;
      this.role = deepClone(scope.row);
      this.role.password = null;
      this.role.status =
        this.role.status === 1
          ? i18n.t("commonLangZh.radioLabelStatusTwo")
          : i18n.t("commonLangZh.radioLabelStatusOne");
      this.active = 0;
      this.stepOne = true;
      this.stepTwo = false;
      this.selectValue = this.role.roleIdList ? this.role.roleIdList : [];
    },
    handleDelete({ $index, row }) {
      this.$confirm(
        i18n.t("managersList.deleteMsg"),
        i18n.t("commonLangZh.warning"),
        {
          confirmButtonText: i18n.t("commonLangZh.confirm"),
          cancelButtonText: i18n.t("commonLangZh.cancel"),
          type: "warning"
        }
      )
        .then(async () => {
          await deleteRole(row.id);
          this.rolesList.splice($index, 1);
          this.$message({
            type: "success",
            message:  i18n.t("commonLangZh.deleteSuccessMsg"),
          });
        })
        .catch(err => {
          console.error(err);
        });
    },
    async confirmRole() {
      const isEdit = this.dialogType === "edit";
      this.role.status =
        this.role.status === i18n.t("commonLangZh.radioLabelStatusTwo") ? 1 : 0;

      if (isEdit) {
        await updateRole(this.role.id, this.role);
        for (let index = 0; index < this.rolesList.length; index++) {
          if (this.rolesList[index].id === this.role.id) {
            this.rolesList.splice(index, 1, Object.assign({}, this.role));
            break;
          }
        }
      } else {
        const { data } = await addRole(this.role);
        this.role.id = data;
        this.rolesList.push(this.role);
      }

      const { createTime, userId, username } = this.role;
      this.dialogVisible = false;
      this.$notify({
        title: i18n.t("commonLangZh.success"),
        dangerouslyUseHTMLString: true,
        message: `
            <div>Id: ${userId}</div>
            <div>Name: ${username}</div>
            <div>Create Time: ${createTime}</div>
          `,
        type: "success"
      });
    }
  }
};
</script>

<style lang="scss" scoped>
.app-container {
  .roles-table {
    margin-top: 30px;
  }
  .row-bg {
    padding: 10px 0;
  }
}
</style>
