function CookieClass() {
    this.expires = 0; //有效时间,以分钟为单位
    this.path = ""; //设置访问路径
    this.domain = ""; //设置访问主机
    this.secure = false; //设置安全性

    this.setCookie = function(name, value) {
        var str = name + "=" + escape(value);
        if (this.expires > 0) //如果设置了过期时间
        {
            var date = new Date();
            var ms = this.expires * 60 * 1000; //每分钟有60秒，每秒1000毫秒
            date.setTime(date.getTime() + ms);
            str += "; expires=" + date.toGMTString();
        }
        if (this.path != "") str += "; path=" + this.path; //设置访问路径
        if (this.domain != "") str += "; domain=" + this.domain; //设置访问主机
        if (this.secure != "") str += "; true"; //设置安全性

        var value = this.getCookie(name);
        if (value != "" || value != null) this.deleteCookie(name);
        document.cookie = str;
    };

    this.getCookie = function(name) {
        var cookieArray = document.cookie.split("; "); //得到分割的cookie名值对
        var cookie = {};
        for (var i = 0; i < cookieArray.length; i++) {
            var arr = cookieArray[i].split("="); //将名和值分开
            if (arr[0] == name) return unescape(arr[1]); //如果是指定的cookie，则返回它的值
        }
        return "";
    };

    this.deleteCookie = function(name) {
        var date = new Date();
        var ms = 1 * 1000;
        date.setTime(date.getTime() - ms);
        var str = name + "=no; expires=" + date.toGMTString(); //将过期时间设置为过去来删除一个cookie
        document.cookie = str;
    };

    this.showCookie = function() {
        alert(unescape(document.cookie));
    }
}

var cook = new CookieClass();
cook.expires = 1; //一分钟有效


//获取cookie
function getCookie(objName, keyName) {
    var result = "";
    //如果Cookie长度大于0
    if (document.cookie.length > 0) {
        //包含多个cookie的数组
        var objArray = document.cookie.split(";");
        //Cookie对象名
        var objSearch = objName + "=";
        //Cookie键名称
        var nameSearch = keyName + "=";
        for (var i = 0; i < objArray.length; i++) {
            //如果数组中(cookie)包含对象名
            if (objArray[i].indexOf(objSearch) != -1) {
                //在数组中取Cookie值
                begin = objArray[i].indexOf(nameSearch);
                if (begin != -1) {
                    begin += nameSearch.length;
                    end = objArray[i].indexOf("&", begin);
                    if (end == -1) {
                        end = objArray[i].length;
                    }
                    result = decodeURI(objArray[i].substring(begin, end));
                }
            }
        }
    }
    return result;
}

//删除cookie
function delCookie(name)//删除cookie
{
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}