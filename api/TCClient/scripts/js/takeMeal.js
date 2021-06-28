$(function () {
    query_data();
});

//获取id
function query_data() {
    var id = GetQueryString("id");//调用
    console.log(id);
    //$('#qrCode').attr('src', qrCodeUrl)
    var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcOrderManagerQueryListDetail&ID="+id;
    $.ajax({
        type: "get",
        url: url,
        dataType: "text",
        success: function (res) {
            var json = JSON.parse(res);
            var data = json.Data;
            if (json.ResultCode == success_code) {
                console.log(data);
                if (data.FetchStatus == 1) {
                    $('#qrCode').attr('src', '/scripts/images/userdQr.png');
                    $('#qr_tip').html('已完成取餐，二维码失效')
                } else {
                    $('#qrCode').attr('src', data.ThinkChange);
                }                
                $('#orderNum').html(data.OrderNumber);
            }
        },
        error: function () {
            console.log("数据访问错误，请检查网络！");
        }
    });
}

//获取参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]);
    return null;
}

//扫码二维码，改变状态
function update_qrcode() {
    location.href = "comment.html";
    //var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcFoodManagementQueryList";
    //$.ajax({
    //    type: "get",
    //    url: url,
    //    dataType: "text",
    //    success: function (res) {
    //        var json = JSON.parse(res);
    //        var data = json.Data;
    //        if (json.ResultCode == success_code) {
    //            location.href = "comment.html";
    //        }
    //    },
    //    error: function () {
    //        console.log("数据访问错误，请检查网络！");
    //    }
    //});
}