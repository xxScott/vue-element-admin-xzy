/*对象序列化为字符串*/
String.toSerialize = function (obj) {
    var transferCharForJavascript = function (s) {
        var newStr = s.replace(
        /[\x26\x27\x3C\x3E\x0D\x0A\x22\x2C\x5C\x00]/g,
        function (c) {
            ascii = c.charCodeAt(0)
            return '\\u00' + (ascii < 16 ? '0' + ascii.toString(16) : ascii.toString(16))
        }
        );
        return newStr;
    };
    if (obj == null) {
        return null;
    }
    else if (obj.constructor == Array) {
        var builder = [];
        builder.push("[");
        for (var index in obj) {
            if (typeof obj[index] == "function") continue;
            if (index > 0) builder.push(",");
            builder.push(String.toSerialize(obj[index]));
        }
        builder.push("]");
        return builder.join("");
    }
    else if (obj.constructor == Object) {
        var builder = [];
        builder.push("{");
        var index = 0;
        for (var key in obj) {
            if (typeof obj[key] == "function") continue;
            if (index > 0) builder.push(",");
            builder.push(key + ":" + String.toSerialize(obj[key]));
            index++;
        }
        builder.push("}");
        return builder.join("");
    }
    else if (obj.constructor == Boolean) {
        return obj.toString();
    }
    else if (obj.constructor == Number) {
        return obj.toString();
    }
    else if (obj.constructor == String) {
        return '"' + transferCharForJavascript(obj) + '"';
    }
    else if (obj.constructor == Date) {
        return '{"__DataType":"Date","__thisue":' + obj.getTime() - (new Date(1970, 0, 1, 0, 0, 0)).getTime() + '}';
    }
    else if (this.toString != undefined) {
        return String.toSerialize(obj);
    }
}

function UUID(len, radix) {
    var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
    var uuid = [], i;
    radix = radix || chars.length;

    if (len) {
        // Compact form
        for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
    } else {
        // rfc4122, version 4 form
        var r;

        // rfc4122 requires these characters
        uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
        uuid[14] = '4';

        // Fill in random data.  At i==19 set the high bits of clock sequence as
        // per rfc4122, sec. 4.1.5
        for (i = 0; i < 36; i++) {
            if (!uuid[i]) {
                r = 0 | Math.random() * 16;
                uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
            }
        }
    }

    return uuid.join('');
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
}


function message_error(content) {
    messager_fade('提示信息', "<div style='width:100%;height:20px;line-height:20px;text-align:center;'><div style='width:50px;height:20px;background-image:url(/Scripts/easyui.1.5.1/themes/icons/error.gif);background-repeat:no-repeat;float:left;background-position:25px 1px;'></div><div style='float:left;'>" + content + "</div></div>");
}

function message_warning(content) {
    messager_fade('提示信息', "<div style='width:100%;height:20px;line-height:20px;text-align:center;'><div style='width:50px;height:20px;background-image:url(/Scripts/easyui.1.5.1/themes/icons/warning.gif);background-repeat:no-repeat;float:left;background-position:25px 1px;'></div><div style='float:left;'>" + content + "</div></div>");
}

function message_information(content) {
    messager_fade('提示信息', "<div style='width:100%;height:20px;line-height:20px;text-align:center;'><div style='width:50px;height:20px;background-image:url(/Scripts/easyui.1.5.1/themes/icons/information.gif);background-repeat:no-repeat;float:left;background-position:25px 1px;'></div><div style='float:left;'>" + content + "</div></div>");
}

function alert_warning(content) {
    $.messager.alert('提示信息', '<div style=\"padding-top:10px;\">' + content + '</div>', 'warning');
}

function alert_error(content) {
    $.messager.alert('提示信息', '<div style=\"padding-top:10px;\">' + content + '</div>', 'error');
}

function alert_information(content) {
    $.messager.alert('提示信息', '<div style=\"padding-top:10px;\">' + content + '</div>', 'information');
}

var border_color = "#4D4D4D";
var background_color = "#4D4D4D";

var border_color_required = "#ff6600";
var background_color_required = "#4D4D4D";

/*用户登录信息动态验证*/
function login_dynamic_validate(obj) {
    $("#" + obj.id).css("background-color", border_color);
    $("#" + obj.id).css("border-color", border_color);
    $("#" + obj.id).attr("title", "");

    if ($("#" + obj.id).val() == "") {
        $("#" + obj.id).css("background-color", background_color_required);
        $("#" + obj.id).css("border-color", border_color_required);
        $("#" + obj.id).attr("title", "不能为空");
    }
}

/*用户登录验证*/
function login_validate() {
    $("#userId").css("background-color", background_color);
    $("#userId").attr("title", "");

    $("#userPwd").css("background-color", background_color);
    $("#userPwd").attr("title", "");

    $("#loginCode").css("background-color", background_color);
    $("#loginCode").attr("title", "");

    $("#verifyCode").css("background-color", background_color);
    $("#verifyCode").attr("title", "");

    if ($("#loginCode").val() == "") {
        $("#loginCode").css("background-color", background_color_required);
        $("#lognCode").css("border-color", border_color_required);
        $("#loginCode").attr("title", "不能为空");
        alert_error("对不起，“集团或门店代码”不能为空，请填写！");
        return false;
    }
    if ($("#userId").val() == "") {
        $("#userId").css("background-color", background_color_required);
        $("#userId").css("border-color", border_color_required);
        $("#userId").attr("title", "不能为空");
        alert_error("对不起，“登录名”不能为空，请填写！");
        return false;
    }
    if ($("#userPwd").val() == "") {
        $("#userPwd").css("background-color", background_color_required);
        $("#userPwd").css("border-color", border_color_required);
        $("#userPwd").attr("title", "不能为空");
        alert_error("对不起，“登录密码”不能为空，请填写！");
        return false;
    }
    if ($("#verifyCode").val() == "") {
        $("#verifyCode").css("background-color", background_color_required);
        $("#verifyCode").css("border-color", border_color_required);
        $("#verifyCode").attr("title", "不能为空");
        alert_error("对不起，“验证码”不能为空，请填写！");
   
        return false;
    }
    var cookie = new CookieClass();
    if (cookie.getCookie("ValidateCode") != $("#verifyCode").val()) {
        $("#verifyCode").css("background-color", background_color_required);
        $("#verifyCode").css("border-color", border_color_required);
        $("#verifyCode").attr("title", "验证码错误");
        alert_error("对不起，“验证码”错误，请确认后重新填写！");
        return false;
    }
    return true;
}

/*打开修改密码对话框*/
function openPwd() {
    $('#w').window('open');
}
/*关闭修改密码对话框*/
function closePwd() {
    $("#userPwd").textbox("setValue", "");
    $("#confirmPwd").textbox("setValue", "");
    $('#w').window('close');
}

/*修改密码*/
function submitPwd() {
    if (!$("#userPwd").textbox("isValid") || !$("#confirmPwd").textbox("isValid")) return false;

    var userPwd = $('#userPwd').textbox("getValue");

    var user = new Object();
    user.Password = userPwd;

    var data = String.toSerialize(user);

    $.ajax({
        type: 'POST',
        url: "../Api/Security/ChangePwd",
        data: data,
        beforeSend: loading,
        success: function (res) {
            loaded();
            closePwd();

            var json = JSON.parse(res);
            var code = json.code;
            var message = json.data;

            if (code == 0) {
                $.messager.alert('提示信息', message, 'info');
            }
            else {
                $.messager.alert('提示信息', message, 'error');
            }

        }
    });
}
/*用户注销*/
function logOut() {
    $.messager.confirm('提示信息', "确定要退出本次登录吗？", function (r) {
        if (r) {
            $.ajax({
                type: 'POST',
                url: "../Api/Security/Logout",
                data: null,
                beforeSend: loading,
                success: function (res) {
                    loaded();

                    var json = JSON.parse(res);
                    var code = json.code;
                    var message = json.data;

                    if (code == 0) {
                        window.location.href = "/Pages/Login.aspx";
                    }
                    else {
                        $.messager.alert('提示信息', message, 'error');
                    }

                }
            });

        }
    });
}
/*退出系统*/
function exit() {
    window.close();
}
/*显示时间*/
function timePrint() {
    var week; var date;
    var today = new Date();
    var year = today.getYear() + 1900;
    var month = today.getMonth() + 1;
    var day = today.getDate();
    var ss = today.getDay();
    var hours = today.getHours();
    var minutes = today.getMinutes();
    var seconds = today.getSeconds();
    date = year + "年" + month + "月" + day + "日 "
    if (ss == 0) week = "星期日"
    if (ss == 1) week = "星期一"
    if (ss == 2) week = "星期二"
    if (ss == 3) week = "星期三"
    if (ss == 4) week = "星期四"
    if (ss == 5) week = "星期五"
    if (ss == 6) week = "星期六"
    if (minutes <= 9)
        minutes = "0" + minutes
    if (seconds <= 9)
        seconds = "0" + seconds
    myclock = date + week + "&nbsp;" + hours + ":" + minutes + ":" + seconds;

    $("#liveclock").html(myclock);
}

function action_refresh() {
    window.location.reload();
}