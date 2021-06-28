<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Group.ascx.cs" Inherits="HrApp.Controls.Group" %>

<script type="text/javascript">
    function query_group_nopreset() {
        var url = "../../Api/Group/Query?Adapter=" + datalist;
        
        $.ajax({
            type: 'POST',
            url: url,
            data: null,
            beforeSend: loading,
            success: function (res) {
                loaded();

                var json = JSON.parse(res);
                var code = json.code;

                if (code == success_code) {
                    var data = json.data;
                  
                    $("#groups").datalist('loadData', data);

                    delete_preset();
                    $('#groups').datalist('selectRow', 0);
                }
                else {
                    var message = json.message;
                    $.messager.alert('提示信息', message, 'error');
                }
            }
        });
    }

    function query_group_onlyStore() {
        var url = "../../Api/Group/QueryAdmin";
        $.ajax({
            type: 'POST',
            url: url,
            data: null,
            beforeSend: loading,
            success: function (res) {
                loaded();
                var json = JSON.parse(res);
                var code = json.code;
                if (code == success_code) {
                    var data = json.data;
                    $("#groups").datalist('loadData', data);
                    $('#groups').datalist('selectRow', 0);
                }
                else {
                    var message = json.message;
                    $.messager.alert('提示信息', message, 'error');
                }
            }
        });
    }
    function query_group_onlyStore_nopreset() {
        var url = "../../Api/Group/QueryAdmin";
        $.ajax({
            type: 'POST',
            url: url,
            data: null,
            beforeSend: loading,
            success: function (res) {
                loaded();
                var json = JSON.parse(res);
                var code = json.code;
                if (code == success_code) {
                    var data = json.data;
                    $("#groups").datalist('loadData', data);
                    delete_preset();
                    $('#groups').datalist('selectRow', 0);
                }
                else {
                    var message = json.message;
                    $.messager.alert('提示信息', message, 'error');
                }
            }
        });
    }
    function query_group_data() {
        var url = "../../Api/Group/Query?Adapter=" + datalist;

        $.ajax({
            type: 'POST',
            url: url,
            data: null,
            beforeSend: loading,
            success: function (res) {
                loaded();

                var json = JSON.parse(res);
                var code = json.code;

                if (code == success_code) {
                    var data = json.data;
      
                    $("#groups").datalist('loadData', data);
                    $('#groups').datalist('selectRow', 0);
                }
                else {
                    var message = json.message;
                    $.messager.alert('提示信息', message, 'error');
                }
            }
        });
    }
    function delete_preset() {
        var rows = $('#groups').datalist('getRows');
        for (var i = 0; i < rows.length; i++) {

            if (rows[i].id == 0) {
                var rowIndex = $('#groups').datagrid('getRowIndex', rows[i]);

                $('#groups').datagrid('deleteRow', rowIndex);
                $('#groups').datagrid('reload');
            }
        }
    }
</script>


<div class="easyui-datalist" id="groups" title="" style="width: 100%; height: 100%;"
    data-options="
			        url: '',
			        method: 'get',
			        checkbox: false,
			        selectOnCheck: false,
			        onSelect: function(){
                    var row = $('#groups').datagrid('getSelected');
                    if (row) {
                           <%--if(row.text.indexOf('-(单独门店)')>-1){
                                 groupId=-1;
                                 storeId=row.id;
                            }else{
                                  groupId=row.id;
                                  storeId=-1;
                            }--%>
                            groupId=row.groudid;
                            storeId=row.storeid;
                            query_data();
                    }
                }
			    ">
</div>
