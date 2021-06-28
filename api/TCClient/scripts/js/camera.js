var app = {
    initialize: function () {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },
    onDeviceReady: function () {
        this.receivedEvent();
    },
    $$: function (id) {
        return document.getElementById(id);
    },
    receivedEvent: function () {
        var getDomLabrary = this.$$('openLabrary');
        var _this = this;
        getDomLabrary.onclick = function () {
            // 打开图片库
            //alert("拍照")
            navigator.camera.getPicture(onSuccess, onFail, {
                quality: 50,                                            // 相片质量是50
                sourceType: Camera.PictureSourceType.Camera,            // 设置从摄像头拍照获取
                destinationType: Camera.DestinationType.DATA_URL        // 以base64返回
            });

            function onSuccess(imageData) {
                var imageBase = "data:image/jpeg;base64," + imageData;
                var this2 = _this;

                //压缩图片
                _this.reducePic(imageBase, function (dataURL) {
                    this2.uploadPic(dataURL);
                });
            }

            function onFail(message) {
                alert('Failed because: ' + message);
            }
        }
    },
    reducePic: function (imageBase, callback) {
        var MAX_WH = 800;
        var img = new Image();
        img.src = imageBase;
        img.onload = function () {
            var that = this;
            if (this.height > MAX_WH) {
                this.width *= MAX_WH / this.height;
                this.height = MAX_WH;
            }
            if (this.width > MAX_WH) {
                this.height *= MAX_WH / this.width;
                this.width = MAX_WH;
            }
            //默认按比例压缩
            var w = this.width,
                h = this.height;
            //生成canvas
            var canvas = document.createElement("canvas"),
                ctx = canvas.getContext("2d");
            canvas.width = w;
            canvas.height = h;
            ctx.drawImage(that, 0, 0, w, h);
            //默认图片质量0.8
            var quality = 0.8;
            //回调函数返回base64值
            var dataURL = canvas.toDataURL("image/jpeg", quality);
            callback(dataURL);
        }
    },
    uploadPic: function (dataURL) {
        var entity = new Object();
        entity.Iconbase64 = dataURL;
        var data = String.toSerialize(entity);
        var url = serviceUrl_pre + "/AjaxHandler.ashx?methodName=CheckImgUploadMbHandlers";
        $.ajax({
            url: url,
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=UTF-8",
            cache: false,
            data: data,
            //beforeSend: loading,
            success: function (res) {
                //loaded();
                if (res.ResultCode == success_code) {
                    var upload_src = res.Data;
                    $wap = $('#preview'),
                        html = '<li class="pic-item pic">' +
                        '<image class="pic-view" src="' + upload_src + '">' +
                        '<div class="del-img" onclick="removePic();"></div>' +
                        '</li>';
                    $wap.append(html);
                }
                else {
                    var message = res.Message;
                    console.log(message);
                }
            },
            error: function (res) {
                console.log('提示信息', '上传失败' + res, 'info');
            }
        });
    },

    removePic: function () {
        var $wap = this.$$('preview');
        $wap.on("click", ".del-img", function () {
            $(this).parent().remove();//删除预览图片
        })
    }

};
app.initialize();