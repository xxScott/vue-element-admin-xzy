$(function () {
    query_data();
});

//查询菜品
function query_data() {
    var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcFoodManagementQueryList&pageNumber=1&pageSize=10";
    $.ajax({
        type: "get",
        url: url,
        dataType: "text",
        success: function (res) {
            var json = JSON.parse(res);
            var data = json.Data;
            if (json.ResultCode == success_code) {
                var index_1 = 0;
                var index_2 = 0;
                var index_3 = 0;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Type == 1) {  //大荤
                        var html = '<label class="weui-cell weui-check__label" for="x1' + index_1 + '">' +
                            '<div class="weui-cell__bd">' +
                            '<p>' + data[i].Menu + '</p>' +
                            '</div>' +
                            '<div class="weui-cell__ft">' +
                            '<input type="radio" class="weui-check" name="radio1" id="x1' + index_1 + '" value="' + data[i].ID + '">' +
                            '<span class="weui-icon-checked"></span>' +
                            '</div>' +
                            '</label>';
                        $('#type_1').append(html);
                        index_1++;
                    } else if (data[i].Type == 2) {  //小荤
                        var html = '<label class="weui-cell weui-check__label" for="s1' + index_2 + '">' +
                            '<div class="weui-cell__hd">' +
                            '<input type="checkbox" class="weui-check" name="checkbox1" id="s1' + index_2 + '" value="' + data[i].ID + '">' +
                            '<i class="weui-icon-checked"></i>' +
                            '</div>' +
                            '<div class="weui-cell__bd">' +
                            '<p>' + data[i].Menu + '</p>' +
                            '</div>' +
                            '</label>';
                        $('#type_2').append(html);
                        index_2++;
                    } else if (data[i].Type == 3) {  //素菜
                        var html = '<label class="weui-cell weui-check__label" for="y1' + index_3 + '">' +
                            '<div class="weui-cell__bd">' +
                            '<p>' + data[i].Menu + '</p>' +
                            '</div>' +
                            '<div class="weui-cell__ft">' +
                            '<input type="radio" class="weui-check" name="radio2" id="y1' + index_3 + '" value="' + data[i].ID + '">' +
                            '<span class="weui-icon-checked"></span>' +
                            '</div>' +
                            '</label>';
                        $('#type_3').append(html);
                        index_3++;
                    }
                }
            } else{
                $.alert(json.Message);
            }
            
        },
        error: function () {
            console.log("数据访问错误，请检查网络！");
        }
    });
}


//保存订单
function save_order() {
    //是否提交对话框
    $.confirm("确认提交订单？", function () {
        //点击确认后的回调函数
        //选中哪些菜
        var value_1 = $("input[name='radio1']:checked").val();  //大荤
        var value_2 = $("input[name='radio2']:checked").val();  //素菜

        //保存订单
        var time = getNowFormatDate();
        var outTradeNo = '';  //订单号
        for (var i = 0; i < 6; i++) //6位随机数，用以加在时间戳后面。
        {
            outTradeNo += Math.floor(Math.random() * 10);
        }
        outTradeNo = time + outTradeNo;  //时间戳，用来生成订单号。
        var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcOrderManagerAdd";
        var data = {
            "OrderNumber": outTradeNo,
            "OrderPrice": "8",
            "OrderStatus": "0"
        };
        data = String.toSerialize(data);
        $.ajax({
            type: "post",
            url: url,
            data: data,
            dataType: "text",
            success: function (res) {
                var json = JSON.parse(res);
                var data = json.Data;
                if (json.ResultCode == success_code) {
                    location.href = "orderList.html";
                } else if (json.ResultCode == 9) {
                    location.href = "check.html";
                } else {
                    $.alert(json.Message);
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


//获取当前时间
function getNowFormatDate() {//获取当前时间
    var date = new Date();
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var strDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var currentdate = date.getFullYear() + month + strDate + date.getHours() + date.getMinutes();
    return currentdate;
}
