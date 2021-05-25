layui.use(['form', 'layer', 'layedit', 'upload', 'element', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            element = layui.element,
            layer = layui.layer,
            upload = layui.upload;

        //编辑时加载方法
        //判断url中是否携带id参数，携带参数则表示是编辑，否则是添加
        var id = getUrlParam("id");
        console.log(id);
        if (id != null) {
            //保存id
            $("#columnId").val(id);
            $.ajax({
                type: "get",
                url: "https://localhost:44377/api/admin/Column/GetColumn/id"
                //url: '/scripts/Column/editData.json' //数据接口
                , data: { id: id }//传值
                , success: function (res) {
                    var result = res.data;
                    console.log(res);

                    //赋值
                    form.val("columnForm", res);
                }
            });
        }

        //封面图片拖拽上传
        upload.render({
            elem: '#CoverUpload',
            url: "https://localhost:44377/api/admin/upload", //上传接口
            method: 'Post',
            type: 'images',
            ext: 'jpg|png|gif',
            size: "2048",
            multiple: true,
            number: 4,//限制数量
            acceptMime: 'image/*',
            allDone: function (obj) { //当文件全部被提交后，才触发
                console.log(obj.total); //得到总文件数
                console.log(obj.successful); //请求成功的文件数
                console.log(obj.aborted); //请求失败的文件数
            },
            //成功后回调
            done: function (res) {
                if (res.code === 200) {
                    layer.msg('上传成功');
                    //绑定图片地址
                    $("#ImgUrl1").val(res.data);
                    layui.$('#CoverImg').removeClass('layui-hide').find('img').attr('src', res.data);
                }
                else {
                    layer.msg('上传失败');
                }

                console.log(res)
            }
        });


        //封面图片拖拽上传
        upload.render({
            elem: '#IconUpload',
            url: "https://localhost:44377/api/admin/upload", //上传接口
            method: 'Post',
            type: 'images',
            ext: 'jpg|png|gif',
            size: "2048",
            //成功后回调
            done: function (res) {
                if (res.code === 200) {
                    layer.msg('上传成功');
                    //绑定图片地址
                    $("#ImgUrl1").val(res.data);
                    layui.$('#IconImg').removeClass('layui-hide').find('img').attr('src', res.data);
                }
                else {
                    layer.msg('上传失败');
                }

                console.log(res)
            }
        });

        //上传视频
        upload.render({
            elem: '#uploadvideo'
            , url: "https://localhost:44377/api/admin/upload"//上传接口
            , accept: 'video' //视频
            , done: function (res) {
                layer.msg('上传成功');
                console.log(res)
            }
        });

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
        form.on('submit(columnsubmit)', function (res) {

            console.log(res.elem) //被执行事件的元素DOM对象，一般为button对象
            console.log(res.form) //被执行提交的form对象，一般在存在form标签时才会返回
            console.log(res.field) //当前容器的全部表单字段，名值对形式：{name: value}
            console.log("内容提交")
            var id = $("#columnId").val();
            //提交
            //var id = 1;
            //id大于0，执行修改
            if (id > 0) {
                var eidtApi = "https://localhost:44377/api/admin/Column/EditColumn";
                ajax(eidtApi, "put", res.field, "修改");
            }
            else {
                //id不存在执行添加
                var addApi = 'https://localhost:44377/api/admin/Column/AddColumn';
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
