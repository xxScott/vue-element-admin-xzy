$(function () {
    query_data();
})

function query_data() {
    var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=TcOrderManagerQueryList&pageNumber=1&pageSize=100";
    $.ajax({
        type: "get",
        url: url,
        dataType: "text",
        success: function (res) {
            var json = JSON.parse(res);
            var data = json.Data;
            if (json.ResultCode == success_code) {
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    var addTime = data[i].AddTime;
                    addTime = addTime.substring(5, 10)
                    addTime = addTime.replace('-', '月');
                    addTime = addTime + '日';

                    var orderNum = data[i].OrderNumber;
                    var id = "'" + data[i].ID + "'";
                    if (data[i].FetchStatus == 1) {
                        //已取餐
                        var html = '<div class="weui-panel weui-panel_access">' +
                            '<div class="weui-panel__hd" style="color:#333;">' +
                            '<span>' + addTime + '午餐  订单编号' + orderNum + '</span>' +
                            '<span class="order-status font-color">已取餐</span>' +
                            '</div>' +
                            '<div class="weui-panel__bd">' +
                            '<a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">' +
                            '<div class="weui-media-box__bd" onclick="takeMeal('+id+')">' +
                            '<p class="weui-media-box__desc">韭菜炒肉等商品</p>' +
                            '</div>' +
                            '</a>' +
                            '<a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">' +
                            '<div class="weui-media-box__bd" onclick="giveComment(' + id +')">' +
                            '<p class="weui-media-box__desc to-comment">去评论</p>' +
                            '</div>' +
                            ' </a>' +
                            '</div>' +
                            '</div>';
                        $('#orderList').append(html);
                    } else {
                        //未取餐
                        var html = '<div class="weui-panel weui-panel_access">' +
                            '<div class="weui-panel__hd" style="color:#333;">' +
                            '<span>' + addTime + '午餐  订单编号'+ orderNum + '</span>' +
                            '<span class="order-status font-color-no">未取餐</span>' +
                            '</div>' +
                            '<div class="weui-panel__bd">' +
                            '<a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">' +
                            '<div class="weui-media-box__bd" onclick="takeMeal(' + id +')">' +
                            '<p class="weui-media-box__desc">韭菜炒肉等商品</p>' +
                            '</div>' +
                            '</a>' +
                            '<a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">' +
                            '<div class="weui-media-box__bd" onclick="takeMeal(' + id +')">' +
                            '<p class="weui-media-box__desc to-comment">去取餐</p>' +
                            '</div>' +
                            ' </a>' +
                            '</div>' +
                            '</div>';
                        $('#orderListNo').append(html);
                    }
                }
                
            }
        },
        error: function () {
            console.log("数据访问错误，请检查网络！");
        }
    });
}

function takeMeal(id) {
    location.href = "takeMeal.html?id=" + id;
}

function giveComment(id) {
    location.href = "comment.html?id=" + id;
}