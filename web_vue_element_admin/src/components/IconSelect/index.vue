<template>
  <el-popover
    placement="right-start"
    width="300"
    trigger="click"
    @show="reset()"
    popper-class="popper-class"
  >
    <div class="ui-fas">
      <el-input
        v-model="searchName"
        @input.native="filterIcons"
        suffix-icon="el-icon-search"
        :placeholder="$t('commonLangZh.searchPlaceHold')"
      ></el-input>
      <ul class="fas-icon-list">
        <li v-for="(item, index) in iconList" :key="index" @click="selectedIcon(item)" class="icon-li-list">
          <svg-icon :icon-class="item"/>
          <span>{{item}}</span>
        </li>
      </ul>
    </div>
    <el-input slot="reference" readonly v-model="value" style="cursor: pointer;">
      <template slot="append">
        <svg-icon :icon-class="value"/>
      </template>
    </el-input>
  </el-popover>
</template>

<script>
import { icons } from "./icon.js";
import i18n from '@/lang'
export default {
  name: "IconSelect",
  props: ["value"],
  data() {
    return {
      searchName: "",
      iconList: icons
    };
  },
  methods: {
    filterIcons() {
      if (this.searchName) {
        this.iconList = this.iconList.filter(item =>
          (item.toLowerCase()).includes(this.searchName)
        );
      } else {
        this.iconList = icons;
      }
    },
    selectedIcon(name) {
      document.querySelector("#app").click();
      this.$emit('selected', name)
    },
    reset() {
      this.searchName = "";
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.ui-fas {
  height: 300px;
  overflow: auto;
}
.fas-icon-list {
  list-style: none;
  padding-left: 10px;
}
.icon-li-list {
  cursor: pointer;
}
</style>
