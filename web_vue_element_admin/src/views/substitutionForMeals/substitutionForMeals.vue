<template>
  <div id="substitutionForMealsId" class="app-container">
    <el-container>
      <el-aside width="200px">
        <div class="stepClass">
          <el-steps direction="vertical" :active="activeStep">
            <el-step
              @click.native="clickStepHandler(1)"
              :title="$t('substitutionForMeals.step1')"
              ref="step1"
            ></el-step>
            <el-step
              @click.native="clickStepHandler(2)"
              :title="$t('substitutionForMeals.step2')"
              ref="step2"
            ></el-step>
            <el-step
              @click.native="clickStepHandler(3)"
              :title="$t('substitutionForMeals.step3')"
              ref="step3"
            ></el-step>
          </el-steps>
        </div>
      </el-aside>
      <el-container class="shadowToBox" v-show="stepOne">
        <el-header>
          <span style="font-size:	18px;">{{title}}</span>
        </el-header>
        <el-main>
          <el-row style="width:42%">
            <filter-container
              :filterData="staffForm"
              @selectedFilterConfirm="filterHandler"
              @selectedFilterCancel="filteCancel"
            >
              <el-form
                ref="staffForm"
                :model="staffForm"
                label-width="80px"
                size="mini"
                :inline="true"
              >
                <el-form-item :label="$t('substitutionForMeals.departmentSelect')">
                  <el-select
                    v-model="staffForm.departmentName"
                    collapse-tags
                    multiple
                    filterable
                    :placeholder="$t('substitutionForMeals.plDepartmentSelect')"
                  >
                    <el-option
                      v-for="(item, index) in departmentNameSelectList"
                      :key="index"
                      :label="item.label"
                      :value="item.value"
                    ></el-option>
                  </el-select>
                </el-form-item>
                <el-form-item :label="$t('substitutionForMeals.staff')">
                  <el-input
                    v-model="staffForm.keyStaff"
                    :placeholder="$t('commonLangZh.searchPlaceHold')"
                    style="width: 179px;"
                  />
                </el-form-item>
              </el-form>
            </filter-container>
          </el-row>
          <el-row>
            <el-col :span="10">
              <div class="tableClassCanSelect">
                <el-table :data="staffList" border @selection-change="changeFun" height="250">
                  <el-table-column type="selection" width="40"></el-table-column>
                  <el-table-column
                    v-for="(item, index) in columnList"
                    :key="index"
                    align="center"
                    :label="item.columnLable"
                    :width="item.width"
                    :prop="item.columnData"
                  />
                </el-table>
              </div>
            </el-col>
            <el-col :span="4">
              <div class="tableClassCanSelect" style="text-align: center;margin-top:100px;">
                <div>
                  <el-button icon="el-icon-arrow-right" circle @click="downHandler"></el-button>
                </div>
                <div style="margin-top:20px;">
                  <el-button icon="el-icon-arrow-left" circle @click="upHandler"></el-button>
                </div>
              </div>
            </el-col>
            <el-col :span="10">
              <div class="tableClassCanSelect">
                <el-table
                  :data="selectedStaffList"
                  border
                  @selection-change="selectedChangeFun"
                  height="250"
                >
                  <el-table-column type="selection" width="40"></el-table-column>
                  <el-table-column
                    v-for="(item, index) in columnList"
                    :key="index"
                    align="center"
                    :label="item.columnLable"
                    :width="item.width"
                    :prop="item.columnData"
                  />
                </el-table>
              </div>
            </el-col>
          </el-row>
        </el-main>
      </el-container>
      <el-container class="shadowToBox" v-show="stepTwo">
        <el-header>
          <span style="font-size:	18px;">{{title}}</span>
        </el-header>
        <el-main>
          <el-form :model="stepTwoForm">
            <el-form-item :label="$t('substitutionForMeals.mealType')">
              <el-select
                v-model="stepTwoForm.mealType"
                :placeholder="$t('substitutionForMeals.plsSelectMealType')"
                @change="mealTypeChangeHandler"
              >
                <el-option
                  v-for="(item, index) in mealTypeList"
                  :key="index"
                  :label="item.label"
                  :value="item.value"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-form>
          <el-collapse v-model="mealsActive">
            <el-collapse-item
              v-for="(item, index) in mealsList"
              :key="index"
              :title="item.title"
              :name="item.name"
            >
              <div class="collapseClass">
                <el-form
                  :ref="item.ref"
                  :model="item.model"
                  label-width="150px"
                  size="mini"
                  :inline="true"
                  label-position="left"
                >
                  <el-form-item
                    v-for="(itemSub, indexSub) in item.list"
                    :key="indexSub"
                    :label="itemSub.label"
                  >
                    <el-input-number v-model="item.model[itemSub.label]" :min="0" :max="10"></el-input-number>
                  </el-form-item>
                </el-form>
              </div>
            </el-collapse-item>
          </el-collapse>
        </el-main>
      </el-container>
      <el-container class="shadowToBox" v-show="stepThree">
        <el-header>
          <span style="font-size:	18px;">{{title}}</span>
        </el-header>
        <el-main>
          <el-form
            ref="orderForm"
            :model="orderForm"
            label-width="150px"
            size="mini"
            label-position="left"
          >
            <el-form-item :label="$t('substitutionForMeals.selectedPerson')">
              <el-table :data="selectedStaffList" border height="250">
                <el-table-column
                  v-for="(item, index) in columnList"
                  :key="index"
                  align="center"
                  :label="item.columnLable"
                  :width="item.width"
                  :prop="item.columnData"
                />
              </el-table>
            </el-form-item>
            <el-form-item :label="$t('substitutionForMeals.selectedMeals')">
              <el-card class="box-card">
                <div v-if="this.totalCountShow() === 0">{{$t('substitutionForMeals.noMealsMsg')}}</div>
                <div v-else>
                  <div
                    v-for="(item, index) in showMealsList"
                    :key="index"
                    class="text item"
                  >{{ index.split("/")[0] + '×' + item + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + $t('substitutionForMeals.equalAcount') + parseInt(index.split("/")[1].split("￥")[0])*parseInt(item) + '￥' }}</div>
                  <div>
                    <span>{{$t('substitutionForMeals.total') + this.totalCountShow() + '￥'}}</span>
                  </div>
                </div>
              </el-card>
            </el-form-item>
            <el-form-item :label="$t('substitutionForMeals.giveMode')">
              <el-radio-group v-model="orderForm.modelSelect">
                <el-radio-button :label="$t('substitutionForMeals.allGive')"></el-radio-button>
                <el-radio-button :label="$t('substitutionForMeals.singleGive')"></el-radio-button>
              </el-radio-group>
            </el-form-item>
            <el-form-item
              :label="$t('substitutionForMeals.givePersonSelect')"
              v-show="orderForm.modelSelect === $t('substitutionForMeals.singleGive')"
            >
              <el-select
                v-model="orderForm.selectName"
                filterable
                :placeholder="$t('substitutionForMeals.plSelectGiveSelect')"
              >
                <el-option
                  v-for="(item, index) in multipleSelection"
                  :key="index"
                  :label="item.UserName"
                  :value="item.id"
                  filterable
                ></el-option>
              </el-select>
              {{ $t('commonLangZh.and') }}
              <el-input
                v-model="orderForm.fullName"
                :placeholder="$t('substitutionForMeals.plsInputPhone')"
                style="width: 190px;"
              />
            </el-form-item>
            <el-form-item label>
              <el-button type="primary" @click="onSubmit">{{$t('substitutionForMeals.submit')}}</el-button>
            </el-form-item>
          </el-form>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script src="./substitutionForMealsController.js" async="async"></script>

<style lang="scss" scoped src="./substitutionForMeals.css">
</style>
