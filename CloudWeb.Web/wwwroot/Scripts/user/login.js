﻿layui.use(['form', 'layer'], function () {
    var form = layui.form;
    $ = layui.jquery;

    //$.get('https://localhost:44377/api/admin/User/VerifyImage', function (data) {
    //    $("#VerifyImage").attr("src", "data:image/png;base64,"+data);
    //});
    //var a = $.cookie('LoginValidateCode');
    //console.log(a);
    //监听提交
    form.on('submit(login)', function (data) {
        // alert(888)
        console.log(data)
        var apiurl = BaseApi + "/api/admin/user/Login";
        $.ajax({
            type: 'post',
            url: apiurl,
            contentType: 'application/json',
            data: JSON.stringify(data.field),
            success: function (res) {
                console.log(res);
                if (res.code === 200) {
                    layer.msg('登录成功');
                    //保存数据到session
                    sessionStorage.setItem('token', res.data.token);
                    sessionStorage.setItem('Account', res.data.userName);
                    location.href = 'index'
                }
                else {
                    layer.msg(res.msg);
                }
            },
            error: function (res) {
                console.log(res)
            }
            //layer.msg("登录成功", function () {
            //    location.href = 'index.html'
            //});

        });

        return false;
    });
})
