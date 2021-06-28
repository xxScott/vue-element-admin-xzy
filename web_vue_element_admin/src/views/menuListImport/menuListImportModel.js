import i18n from '@/lang'

const searchData = {
  mealSeparation: i18n.t('menuListImport.allMealSeparation'),
  status: i18n.t('menuListImport.allStatus')
}

export default {
  mealSeparationOptions: [
    {
      value: '0',
      label: i18n.t('menuListImport.allMealSeparation')
    },
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
  statusOptions: [
    {
      value: '1',
      label: i18n.t('menuListImport.allStatus')
    },
    {
      value: '2',
      label: i18n.t('menuListImport.start')
    },
    {
      value: '3',
      label: i18n.t('menuListImport.stop')
    }
  ],
  total: 0,
  listQuery: {
    methodName: 'TcFoodManagementQueryList',
    pageNumber: 1,
    pageSize: 10
  },
  deleteQuery: {
    methodName: 'TcFoodManagementDelete',
    ID: ''
  },
  listLoading: true,
  searchData: Object.assign({}, searchData),
  menuList: [],
  columnList: [
    {
      columnLable: i18n.t('menuListImport.dataTodata'),
      columnData: 'Date'
    },
    {
      columnLable: i18n.t('menuListImport.mealSeparation'),
      columnData: 'mealSeparation'
    },
    {
      columnLable: i18n.t('menuListImport.mealName'),
      columnData: 'Menu',
      width: '160px'
    },
    {
      columnLable: i18n.t('menuListImport.packageName'),
      columnData: 'SetMeal',
      width: '160px'
    },
    {
      columnLable: i18n.t('menuListImport.price'),
      columnData: 'Price'
    }
  ]
}
