$(function () {
    //save_data();
});

//授权

//保存号码
function save_data() {
    //获取用户名、密码
    var username = $('#username').val();
    var userpsw = $('#userpsw').val();
    //验证
    if (username == "") {
        $.alert("请输入用户名");
        return;
    }else if (userpsw == "") {
        $.alert("请输入密码");
        return;
    }

    var data = {
        name: username,
        psw: userpsw
    }
    console.log(data);
    //data = String.toSerialize(data);

    //var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcFoodManagementQueryList";
    //$.ajax({
    //    type: "get",
    //    url: url,
    //    data: data,
    //    dataType: "text",
    //    success: function (res) {
    //        var json = JSON.parse(res);
    //        var data = json.Data;
    //        if (json.ResultCode == success_code) {

                //location.href = "../Pages/dayMeal.html";
    //        }
    //    },
    //    error: function () {
    //        console.log("数据访问错误，请检查网络！");
    //    }
    //});

    location.href = "../Pages/dayMeal.html";
}



