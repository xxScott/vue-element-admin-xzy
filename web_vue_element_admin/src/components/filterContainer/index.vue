<template>
  <el-row :gutter="20">
    <el-col>
      <el-collapse accordion @change="changeFilterHandler" v-model="activeNames">
        <el-collapse-item name="1">
          <template slot="title">
            <div class="colClass" style="margin-right: 30px;">
              <i class="header-icon">
                <svg-icon :icon-class="filterIcon" style="font-size: 20px;"/>
              </i>
              <div :class="filterLabel">{{ $t('commonLangZh.filterContainer') }}</div>
            </div>
            <div class="colClass" style="padding-bottom: 4px;">
              <el-button
                size="mini"
                type="primary"
                icon="el-icon-search"
                @click.stop="handleFilter"
                :title="$t('commonLangZh.filterContainer')"
              ></el-button>
              <el-button
                size="mini"
                type="danger"
                icon="el-icon-delete"
                @click.stop="handleClear"
                :title="$t('commonLangZh.cancel')"
              ></el-button>
            </div>
          </template>
          <el-form ref="filterForm" :model="filterDataFromParent">
            <div style="padding-top:20px;border-top:1px solid #ebeef5;margin-bottom: -25px;">
              <slot></slot>
            </div>
          </el-form>
        </el-collapse-item>
      </el-collapse>
    </el-col>
  </el-row>
</template>

<script>
import i18n from "@/lang";
export default {
  name: "FilterContainer",
  props: ["filterData"],
  data() {
    return {
      filterIcon: "filterIcon",
      filterLabel: "filterLabelClass",
      activeNames: "",
      filterDataFromParent: this.filterData
    };
  },
  methods: {
    handleClear() {
      for (var [key, value] in this.filterDataFromParent) {
        console.log("key", key);
        console.log("value", value);
      }
      this.filterDataFromParent = {};
      this.$emit("selectedFilterCancel", this.filterDataFromParent);
    },
    handleFilter() {
      this.$emit("selectedFilterConfirm", this.filterDataFromParent);
    },
    changeFilterHandler(event) {
      if (event) {
        this.filterLabel = "filterLabelClassHover";
        this.filterIcon = "filterIconBlue";
      } else {
        this.filterLabel = "filterLabelClass";
        this.filterIcon = "filterIcon";
      }
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.filterLabelClass {
  font-size: 15px;
  color: #303133;
  display: inline-block;
}
.filterLabelClassHover {
  font-size: 15px;
  color: #409eff;
  display: inline-block;
}
.header-icon {
  margin-right: 5px;
  display: inline-block;
}
.colClass {
  display: inline-block;
}
</style>
