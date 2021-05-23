layui.use(['form', 'layer'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer;

        //编辑时加载方法
        //判断url中是否携带id参数，携带参数则表示是编辑，否则是添加
        var id = getUrlParam("id");
        console.log(id);
        if (id != null) {
            var as = { "colName": "贤心" };
            console.log("asd")
           

            $.ajax({
                type: "get",
                //url: "https://localhost:44377/api/admin/Column/GetColumn/id",
                url: '/scripts/Column/editData.json' //数据接口
                , data: { id: id }//传值
                , success: function (res) {
                    var result = res.data;
                    console.log(res);
              
                    form.val("formTest", res);
                    //$("#colName").val(result.colName);
                    //$("#CoverUrl").val(result.colName);
                    //$("#Icon").val(result.colName);
                    //$("#Summary").val(result.colName);
                    //$("#Video").val(result.colName);
                    //$("#sort").val(result.colName);
                    //$("#LocationUrl").val(result.colName);
                    //$("#IsShow").val(result.colName);
                    //$("#ImgDesc1").val(result.colName);
                    //$("#ImgDesc2").val(result.colName);
                }
            });

        }

        //获取url中的参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }


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
                    dataType: 'application/json',
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