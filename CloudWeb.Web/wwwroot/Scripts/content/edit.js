layui.use(['form', 'layer', 'layedit', 'upload', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer,
            laydate = layui.laydate
            , upload = layui.upload;
        var ue = UE.getEditor('container', {
            initialFrameHeight: 200
        });

        //对编辑器的操作最好在编辑器ready之后再做
        ue.ready(function () {
            //设置编辑器的内容
            ue.setContent('');
            //获取html内容，返回: <p>hello</p>
            var html = ue.getContent();
            //获取纯文本内容，返回: hello
            var txt = ue.getContentTxt();
        });

        Category(id);
        layui.form.render("select");

        //封面图片拖拽上传
        upload.render({
            elem: '#Img1Upload',
            url: "https://localhost:44377/api/admin/upload", //上传接口
            data: { path: 'content' },//请求上传接口的额外参数,判断文件从哪里传入
            //headers: { token: 'sasasas' },//头部携带的参数，方便对接口做验证
            method: 'Post',
            type: 'images',
            async: true,
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
                    layui.$('#Img1').removeClass('layui-hide').find('img').attr('src', res.data);
                }
                else {
                    layer.msg('上传失败');
                }
                console.log(res)
            }
        });

        //内页封面图片拖拽上传
        upload.render({
            elem: '#Img2Upload',
            url: "https://localhost:44377/api/admin/upload", //上传接口
            data: { path: 'content' },//请求上传接口的额外参数,判断文件从哪里传入
            //headers: { token: 'sasasas' },//头部携带的参数，方便对接口做验证
            method: 'Post',
            type: 'images',
            async: true,
            accept: 'images',//指定允许上传时校验的文件类型
            ext: 'jpg|png',//允许上传的文件后缀
            acceptMime: 'image/jpg, image/png',//规定打开文件选择框时，筛选出的文件类型，值为用逗号隔开的 MIME 类型列表
            size: "2048",
            //成功后回调
            done: function (res) {
                if (res.code === 200) {
                    layer.msg('上传成功');
                    //绑定图片地址
                    $("#ImgUrl2").val(res.data);
                    layui.$('#Img2').removeClass('layui-hide').find('img').attr('src', res.data);

                }
                else {
                    layer.msg('上传失败');
                }
                console.log(res)
            }
        });

        //时间控件
        var now = new Date();
        laydate.render({
            elem: '#start',
            theme: 'molv',
            type: 'datetime',
            trigger: 'click',
            max: 4073558400000, //公元3000年1月1日
            value: new Date(),
            ready: function (date) {
                //console.log(date); //得到初始的日期时间对象：{year: 2017, month: 8, date: 18, hours: 0, minutes: 0, seconds: 0}
                this.dateTime.hours = now.getHours();
                this.dateTime.minutes = now.getMinutes();
                this.dateTime.seconds = now.getSeconds();
            }
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
                        $('#Img1').removeClass('layui-hide').find('img').attr('src', res.data.imgUrl1);

                        ue.setContent(res.data.content);
                    }
                    else {
                        //layer.msg("");
                    }
                }
            });

        }

        //检查项目添加到下拉框中
        $.ajax({
            url: '/service/select',
            dataType: 'json',
            type: 'post',
            success: function (res) {
                if (res.code === 200) {
                    $("#columnSelect").empty();
                    //$("#service").append(new Option("请选择服务", "0"));
                    $.each(res.data, function (index, item) {
                        $('#columnSelect').append(new Option(item));
                    });
                } else {
                    $("#columnSelect").append(new Option("暂无数据", ""));
                }
                //重新渲染
                form.render("select");
            }
        });


        //自定义验证规则
        form.verify({
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

//类别下拉框
function Category(columnid) {
    console.log(columnid);
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            id: columnid
        },
        url: BaseApi + '/api/admin/Column/GetDropDownList',
        success: function (res) {
            console.log(res.data);
            //category_name = json;
            var ophtmls = '';
            $("select[name=columnId]").html(ophtmls);
            for (var i = 0; i < res.data.length; i++) {
                var Id = res.data[i].columnId;
                var ClassLayer = res.data[i].level;
                var Title = res.data[i].colName;
                if (ClassLayer == 1) {
                    ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                } else {
                    Title = "├ " + Title;
                    Title = StringOfChar(ClassLayer - 1, "　") + Title;
                    ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                }
                console.log(ClassLayer);
            }
            $("#columnSelect").html(ophtmls);

        }
    });
}