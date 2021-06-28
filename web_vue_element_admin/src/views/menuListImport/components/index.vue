<template>
  <div class="edit-container">
    <el-row :gutter="20" class="row-bg">
      <el-col :push="1">
        <el-form ref="menuEditForm" :model="postForm" label-width="100px" label-position="left">
          <el-form-item v-show="!editFlag" :label="$t('menuListImport.isBatch')">
            <el-radio-group v-model="postForm.mode" @change="modeChangeHandler">
              <el-radio-button :label="$t('menuListImport.modeOne')" />
              <el-radio-button :label="$t('menuListImport.modeTwo')" />
            </el-radio-group>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.mealName')">
            <el-input v-model="postForm.Menu" style="width: 300px;" />
          </el-form-item>
          <el-form-item :label="$t('menuListImport.packageName')">
            <el-input v-model="postForm.SetMeal" style="width: 300px;" />
          </el-form-item>
          <el-form-item :label="$t('menuListImport.mealSeparation')">
            <el-select
              v-model="postForm.mealSeparation"
              :placeholder="$t('commonLangZh.selectPlaceholder')"
              style="width: 300px;"
              multiple
            >
              <el-option
                v-for="item in mealSeparationOptions"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.type')">
            <el-select
              v-model="postForm.Type"
              :placeholder="$t('commonLangZh.selectPlaceholder')"
              style="width: 300px;"
            >
              <el-option
                v-for="item in typeOptions"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.mealPic')">
            <el-upload
              action="https://jsonplaceholder.typicode.com/posts/"
              list-type="picture-card"
              :on-success="handleAvatarSuccess"
              :before-upload="beforeAvatarUpload"
              :on-preview="handlePictureCardPreview"
            >
              <i class="el-icon-plus" />
            </el-upload>
            <el-dialog :visible.sync="dialogVisible">
              <img width="100%" :src="postForm.imageUrl" alt>
            </el-dialog>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.dataTodata')">
            <el-date-picker
              v-show="!isBatch"
              v-model="postForm.Date"
              type="date"
              :placeholder="$t('commonLangZh.dataPlaceHolder')"
              style="width: 300px;"
              value-format="yyyy-M-d"
            />
            <el-date-picker
              v-show="isBatch"
              v-model="postForm.dataTodataRange"
              style="width: 300px;"
              type="daterange"
              range-separator="-"
              :start-placeholder="$t('commonLangZh.timeStartPlaceholder')"
              :end-placeholder="$t('commonLangZh.timeEndPlaceholder')"
            />
          </el-form-item>
          <el-form-item :label="$t('menuListImport.price')">
            <el-input v-model="postForm.Price" style="width: 300px;">
              <template slot="append">￥</template>
            </el-input>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.status')">
            <el-radio-group v-model="postForm.Status">
              <el-radio-button :label="$t('menuListImport.start')" />
              <el-radio-button :label="$t('menuListImport.stop')" />
            </el-radio-group>
          </el-form-item>
          <el-form-item :label="$t('menuListImport.remark')">
            <el-input v-model="postForm.Remark" style="width: 300px;" type="textarea" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="confirmHandler">{{ $t('commonLangZh.confirm') }}</el-button>
          </el-form-item>
        </el-form>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import i18n from '@/lang'
import { Message } from 'element-ui'
import { updataMenu, addMenu } from '../../../api/menu'

const defaultForm = {
  ID: undefined,
  Date: '',
  Menu: '',
  SetMeal: '',
  Type: '',
  Price: '',
  Status: '',
  Remark: '',
  mode: i18n.t('menuListImport.modeTwo')
}

export default {
  data() {
    return {
      dialogVisible: false,
      editFlag: false,
      isBatch: false,
      active: 0,
      postForm: Object.assign({}, defaultForm),
      tempRoute: {},
      mealSeparationOptions: [
        {
          value: '1',
          label: i18n.t('menuListImport.breakfast')
        },
        {
          value: '2',
          label: i18n.t('menuListImport.lunch')
        },
        {
          value: '3',
          label: i18n.t('menuListImport.dinner')
        },
        {
          value: '4',
          label: i18n.t('menuListImport.midnightSnack')
        }
      ],
      typeOptions: [
        {
          value: '1',
          label: i18n.t('menuListImport.package')
        },
        {
          value: '2',
          label: i18n.t('menuListImport.meats')
        },
        {
          value: '3',
          label: i18n.t('menuListImport.stirFried')
        },
        {
          value: '4',
          label: i18n.t('menuListImport.vegetables')
        }
      ]
    }
  },
  created() {
    const ID = this.$route.query && this.$route.query.ID
    console.log(this.$route.query)
    if (ID !== undefined) {
      this.editFlag = true
      const object = Object.assign({}, this.$route.query)
      this.postForm = Object.assign({}, object)
      this.tempRoute = Object.assign({}, this.$route)
      this.fetchData(ID)
    }
  },
  methods: {
    handlePictureCardPreview(file) {
      this.postForm.imageUrl = file.url
      // this.postForm.ID = file.ID
      // this.postForm.Date = file.Date
      // this.postForm.Menu = file.Menu
      // this.postForm.SetMeal = file.SetMeal
      // this.postForm.Type = file.Type
      // this.postForm.Status = file.Status
      // this.postForm.Remark = file.Remark
      // this.postForm.Price = file.Price
      this.dialogVisible = true
    },
    modeChangeHandler(id) {
      if (id === i18n.t('menuListImport.modeOne')) {
        this.isBatch = true
      } else {
        this.isBatch = false
      }
    },
    async confirmHandler() {
      const ID = this.$route.query && this.$route.query.ID
      console.log(this.$route.query)
      if (ID === undefined) {
        await addMenu({ methodName: 'TcFoodManagementAdd' }, this.postForm).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.addSuccessMsg')
            })
          }
        })
      } else {
        console.log(this.postForm)
        // delete this.department.superiorDepartment
        // delete this.department.superiorDepartmentId
        await updataMenu({ methodName: 'TcFoodManagementUpdate', id: this.postForm.ID }, this.postForm).then(response => {
          if (response.data.ResultCode !== 0) {
            Message.error(response.data.Message)
          } else {
            this.dialogVisible = false
            this.$message({
              type: 'success',
              message: i18n.t('commonLangZh.updataSuccessMsg')
            })
          }
        })
      }
    },
    handleAvatarSuccess(res, file) {
      this.postForm.imageUrl = URL.createObjectURL(file.raw)
    },
    beforeAvatarUpload(file) {
      // const isJPG = file.type === "image/jpeg";
      const isLt2M = file.size / 1024 / 1024 < 2

      // if (!isJPG) {
      //   this.$message.error("上传头像图片只能是 JPG 格式!");
      // }
      if (!isLt2M) {
        this.$message.error('上传头像图片大小不能超过 2MB!')
      }
      // return isJPG && isLt2M;
      return isLt2M
    },
    fetchData(id) {
      this.setTagsViewTitle()
    },
    setTagsViewTitle() {
      const title = i18n.t('route.editMenu')
      const route = Object.assign({}, this.tempRoute, {
        title: `${title}-${this.postForm.id}`
      })
      this.$store.dispatch('updateVisitedView', route)
    }
  }
}
</script>

<style lang="scss">
.edit-container {
  margin-top: 30px;
}
</style>
