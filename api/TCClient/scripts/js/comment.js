$(function () {
    
});

//获取参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]);
    return null;
}

//保存评价
function save_order() {
    $.confirm("确认提交评论？", function () {
        //点击确认后的回调函数
        var star = $('#ratingChanged').html();
        var OrderID = GetQueryString('id');
        var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcOrderDetailedManagerComment";
        var data = {
            "Stars": star,
            "OrderID": OrderID
        }
        data = String.toSerialize(data);
        console.log(data);
        $.ajax({
            type: "post",
            url: url,
            data: data,
            dataType: "text",
            success: function (res) {
                var json = JSON.parse(res);
                var data = json.Data;
                if (json.ResultCode == success_code) {
                    console.log(data);
                    // location.href = "final.html";
                }
            },
            error: function () {
                console.log("数据访问错误，请检查网络！");
            }
        });
    }, function () {
        //点击取消后的回调函数
    });
}

