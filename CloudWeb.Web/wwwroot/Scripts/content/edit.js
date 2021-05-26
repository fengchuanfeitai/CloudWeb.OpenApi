layui.use(['form', 'layer', 'layedit', 'upload', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer,
            laydate = layui.laydate
            , upload = layui.upload;

        //封面图片拖拽上传
        upload.render({
            elem: '#logoUpload',
            url: "https://localhost:44377/api/admin/upload", //上传接口
            data: { path: 'content' },//请求上传接口的额外参数,判断文件从哪里传入
            //headers: { token: 'sasasas' },//头部携带的参数，方便对接口做验证
            method: 'Post',
            type: 'images',
            async: true,
            multiple: true,
            accept: 'images',//指定允许上传时校验的文件类型
            ext: 'jpg|png',//允许上传的文件后缀
            acceptMime: 'image/jpg, image/png',//规定打开文件选择框时，筛选出的文件类型，值为用逗号隔开的 MIME 类型列表
            size: "2048",
            //成功后回调
            done: function (res) {
                if (res.code === 200) {
                    layer.msg('上传成功');
                    //绑定图片地址
                    $("#ImgUrl1").val(res.data);
                    layui.$('#logoImg').removeClass('layui-hide').find('img').attr('src', res.data);
                }
                else {
                    layer.msg('上传失败');
                }
                console.log(res)
            }
        });

        //执行一个laydate实例
        laydate.render({
            elem: '#start' //指定元素
            , type: 'datetime'
        });

        //编辑时加载方法
        //判断url中是否携带id参数，携带参数则表示是编辑，否则是添加
        var id = getUrlParam("id");
        console.log(id);
        if (id != null) {
            //保存id
            $("#contentId").val(id);
            console.log("asd")
            $.ajax({
                type: "get"
                , url: "https://localhost:44377/api/admin/Content/GetContent"
                //url: '/scripts/Column/editData.json' //数据接口
                , data: { id: id }//传值
                , success: function (res) {
                    console.log(res);
                    if (res.code === 200) {
                        //表单赋值
                        form.val("contentform", res.data);
                    }
                    else {
                        //layer.msg("");
                    }
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
        form.on('submit(contentsubmit)', function (res) {
            console.log(res.elem) //被执行事件的元素DOM对象，一般为button对象
            console.log(res.form) //被执行提交的form对象，一般在存在form标签时才会返回
            console.log(res.field) //当前容器的全部表单字段，名值对形式：{name: value}
            console.log("内容提交")
            var id = $("#contentId").val();
            //var id = 1;
            //id大于0，执行修改
            if (id > 0) {
                var eidtApi = "https://localhost:44377/api/admin/Content/EditContent";
                ajax(eidtApi, "put", res.field, "修改");
            }
            else {
                //id不存在执行添加
                var addApi = 'https://localhost:44377/api/admin/Content/AddContent';
                ajax(addApi, "post", res.field, "添加");
            }

            return false;
        });

        //提交方法
        function ajax(api, method, params, msg) {
            //提交
            $.ajax({
                type: method,
                url: api,
                data: params,
                async: false,
                success: function (res) {
                    console.log(res);
                    if (res.code === 200) {
                        layer.msg(msg + "成功", { icon: 1 });
                        //关闭当前frame
                        xadmin.close();
                        // 可以对父窗口进行刷新 
                        xadmin.father_reload();
                    }
                    else
                        layer.msg(msg + "失败", { icon: 2 });
                },
                error: function (res) {
                    console.log(res)
                }
            });
        }

    });
