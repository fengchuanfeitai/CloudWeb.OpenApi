layui.use(['form'], function () {
    var form = layui.form;
    $ = layui.jquery;

    //初始化验证码
    var verifyApi = BaseApi + '/api/admin/User/VerifyImage?r=' + Math.random();
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
            dataType: 'JSON',//注意哦，这一句要记得加上哦，我就是因为没加这句还查了好久的
            crossDomain: true,
            xhrFields: {
                withCredentials: true
            },
            success: function (res) {
                if (res.code === 200) {
                    //保存数据到session
                    layer.msg("登录成功", function () {
                        sessionStorage.setItem('token', res.data.token);
                        sessionStorage.setItem('Account', res.data.userName);
                        sessionStorage.setItem('UserId', res.data.userId);

                        location.href = 'index'
                    });
                }
                else {
                    layer.msg(res.msg);
                    $("#VerifyImage").click();
                }
            },
            error: function (res) {
                $("#VerifyImage").click();
            }
        });

        return false;
    });

    $('#VerifyImage').click(function () {
        verifyApi = BaseApi + '/api/admin/User/VerifyImage?r=' + Math.random();
        $("#VerifyImage").attr('src', verifyApi);
    })
})


