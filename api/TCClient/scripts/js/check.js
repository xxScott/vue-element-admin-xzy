$(function () {

});

var wait = 60;
function send_code() {   //调用发送验证码
    var o = $("#getCheck");
    wait = 60;
    //验证手机号
    var phone = $('#userPhone').val();
    var TEL_REGEXP = /^1([38]\d|5[0-35-9]|7[3678])\d{8}$/;
    if (TEL_REGEXP.test(phone)) {
        //发送验证码
        var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcVerifyCodeManagementGetVerifySms&PhoneNumber=" + phone;
        $.ajax({
            type: "post",
            url: url,
            dataType: "text",
            success: function (res) {
                var json = JSON.parse(res);
                var data = json.Data;
                if (json.ResultCode == success_code) {
                    time(o);
                } else {
                    $.alert(json.Message);
                }
            },
            error: function () {
                console.log("数据访问错误，请检查网络！");
            }
        });
    } else {
        $.alert("请输入正确的手机号");
    }
}

function time(o) {   //按钮1分钟时间
    if (wait == 0) {
        $('#getCheck').removeClass("checkBtn");
        o.removeAttr("disabled");
        o.html("获取验证码");
        wait = 60;
    } else {
        $('#getCheck').addClass("checkBtn");
        o.attr("disabled", true);
        o.html("重新发送(" + wait + "s)");
        wait--;
        setTimeout(function () {
            time(o)
        },1000)
    }
} 

function get_code() {   //发送验证码接口
    var phone = $('#userPhone').val();
    var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcVerifyCodeManagementGetVerifySms&PhoneNumber=" + phone;
    $.ajax({
        type: "post",
        url: url,
        dataType: "text",
        success: function (res) {
            var json = JSON.parse(res);
            var data = json.Data;
            if (json.ResultCode == success_code) {
                console.log(data);
            }
        },
        error: function () {
            console.log("数据访问错误，请检查网络！");
        }
    });
}

function check_code() {  //验证
    var code = $('#userCode').val();
    var phone = $('#userPhone').val();
    var TEL_REGEXP = /^1([38]\d|5[0-35-9]|7[3678])\d{8}$/;
    if (!TEL_REGEXP.test(phone)) {
        $.alert("请输入正确的手机号");
        return;
    }
    if (code == '') {
        $.alert("请输入验证码");
        return;
    }
    var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcVerifyCodeManagementVerifySms&PhoneNumber=" + phone + "&VerifyCode=" + code;
    $.ajax({
        type: "post",
        url: url,
        dataType: "text",
        success: function (res) {
            var json = JSON.parse(res);
            var data = json.Data;
            if (json.ResultCode == success_code) {
                location.href = "dayMeal.html";
            }
        },
        error: function () {
            console.log("数据访问错误，请检查网络！");
        }
    });
}