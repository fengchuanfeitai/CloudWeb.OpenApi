layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,       
        layer = layui.layer;

    //页面初始化给页面赋值
    $(function () {
        layer.confirm('纳尼？', {
            btn: ['按钮一', '按钮二', '按钮三'] //可以无限个按钮
            , btn3: function (index, layero) {
                //按钮【按钮三】的回调
            }
        }, function (index, layero) {
            //按钮【按钮一】的回调
        }, function (index) {
            //按钮【按钮二】的回调
        });

    });


});