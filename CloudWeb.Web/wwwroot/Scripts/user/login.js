layui.use(['form', 'layer'], function () {
    var form = layui.form;
    $ = layui.jquery;

    var verifyApi = BaseApi + '/api/admin/User/VerifyImage';
    $("#VerifyImage").attr('src', verifyApi);

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
                    //保存数据到session
                    layer.msg("登录成功", function () {
                        sessionStorage.setItem('token', res.data.token);
                        sessionStorage.setItem('Account', res.data.userName);
                        location.href = 'index'
                    });
                }
                else {
                    layer.msg(res.msg);
                    $("#VerifyImage").attr('src', verifyApi);
                }
            },
            error: function (res) {
                console.log(res)
                $("#VerifyImage").attr('src', verifyApi);
            }
        });

        return false;
    });
})
