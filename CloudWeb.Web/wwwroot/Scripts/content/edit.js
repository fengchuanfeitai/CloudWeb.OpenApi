﻿layui.use(['form', 'layer', 'layedit', 'upload', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer,
            laydate = layui.laydate;

        //执行一个laydate实例
        laydate.render({
            elem: '#start' //指定元素
            , type: 'datetime'
        });


        form.val('example', {
            "username": "贤心" // "name": "value"
            , "password": "123456"
            , "interest": 1
            , "like[write]": true //复选框选中状态
            , "close": true //开关状态
            , "sex": "女"
            , "desc": "我爱 layui"
        });
        //自定义验证规则
        form.verify({
            //nikename: function (value) {
            //    if (value.length < 5) {
            //        return '昵称至少得5个字符啊';
            //    }
            //},
            //pass: [/(.+){6,12}$/, '密码必须6到12位'],
            //repass: function (value) {
            //    if ($('#L_pass').val() != $('#L_repass').val()) {
            //        return '两次密码不一致';
            //    }
            //}
        });

        //监听提交
        form.on('submit(*)',
            function (res) {
                console.log(res.field);
                //提交
                $.ajax({
                    type: 'post',
                    url: 'https://localhost:44377/api/admin/Column/AddColumn',
                    dataType: 'json',
                    data: res.field,
                    success: function (res) {
                        if (res.code != 0) {
                            layer.alert("增加成功", {
                                icon: 6
                            },
                                function () {
                                    //关闭当前frame
                                    xadmin.close();

                                    // 可以对父窗口进行刷新 
                                    xadmin.father_reload();
                                });
                        }
                    },
                    error: function (res) {
                        console.log(res)
                    }
                });

            });

    });
//弹出框，刷新页面
function alertMsg(msg) {
    layer.open({
        content: msg,
        yes: function (index, layero) {
            location.reload();
            layer.close(index);
        }
    });
}