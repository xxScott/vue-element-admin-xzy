<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Store.ascx.cs" Inherits="HrApp.Controls.Store" %>
<script type="text/javascript">
    function query_group_store() {
        var url = "../../Api/Group/Query?Adapter=" + tree;
       
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
                    groupId = json.data[0].groudid;
                    storeId = json.data[0].storeid;
                    $("#groupStores").tree('loadData', data);
                    $("#groupStores li:eq(1)").find("div").addClass("tree-node-selected");   //设置第一个节点高亮  
                    var n = $("#groupStores").tree("getSelected");
                    if (n != null) {
                        $("#groupStores").tree("select", n.target);
                      
                        if ($('#groupStores').tree('isLeaf', n.target)) {
                            var father = $("#groupStores").tree('getParent', n.target);
                            if (father != null) {
                                groupId = father.id;
                                if (groupId == 0) {
                                    groupId = -1;
                                }
                                storeId = n.id;
                                query_data();
                            }
                        }
                    }  //相当于默认点击了一下第一个节点，执行onSelect方法
                }
                else {
                    var message = json.message;
                    $.messager.alert('提示信息', message, 'error');
                }
            }
        });
    }
</script>
<div id="groupStores" class="easyui-tree" style="width: 100%; height: auto" border="0"
    data-options="
				  checkbox: false,
                    dnd: true,
                    url: '',
                    id: 'id',
                    text:'name',
                onClick: function (node) {
    
                        if ($('#groupStores').tree('isLeaf', node.target))
                        {
                         var father = $(this).tree('getParent',node.target);
    if(father!=null){
                            groupId=father.id;
                           if(groupId==0){
                                groupId=-1;
    }
                            storeId=node.id;
                            query_data();}
                        }    
                    }
			">
</div>
