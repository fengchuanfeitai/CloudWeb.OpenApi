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
        console.log("ada:" + id);

        //修改
        if (id != null) {
            //判断是否是“添加子级”跳转

            var action = getUrlParam("action");

            if (action === 'addSublevel') {
                Category(id, action);
                layui.form.render("select");
            }
            else {
                Category();
                layui.form.render("select");

                //保存id
                $("#columnId").val(id);
                $.ajax({
                    type: "get",
                    url: BaseApi + "/api/admin/Column/GetColumn"
                    //url: '/scripts/Column/editData.json' //数据接口
                    , data: { id: id }//传值
                    , success: function (res) {
                        var result = res.data;
                        console.log(result);

                        //赋值
                        form.val("columnForm", res.data);
                        $('#CoverImg').removeClass('layui-hide').find('img').attr('src', res.data.coverUrl);
                        $('#IconImg').removeClass('layui-hide').find('img').attr('src', res.data.icon);

                    }
                });
            }
        }
        else {
            Category();
            layui.form.render("select");
        }


        upload.render({
            elem: '#slide-pc',
            url: BaseApi + "/api/admin/upload", //上传接口
            exts: 'jpg|png|jpeg',
            multiple: true,
            data: { path: 'asd' },
            before: function (obj) {
                layer.msg('图片上传中...', {
                    icon: 16,
                    shade: 0.01,
                    time: 0
                })
            },
            done: function (res) {
                layer.close(layer.msg());//关闭上传提示窗口
                if (res.code !== 200) {
                    return layer.msg(res.msg);
                }
                console.log(res)
                //$('#slide-pc-priview').append('<input type="hidden" name="pc_src[]" value="' + res.filepath + '" />');
                $('#slide-pc-priview').append('<li class="item_img"><div class="operate"><i  class="close layui-icon">  <button type="button" class="layui-btn layui-btn-sm"><i class="layui-icon"></i></button></i></div><img src="' + res.data + '" class="img" ><input type="hidden" name="pc_src[]" value="' + res.data + '" /></li>');
            }
        });

        //多图片上传
        upload.render({
            elem: '#test2'
            , url: BaseApi + "/api/admin/upload" //上传接口
            , multiple: true
            , type: 'images'
            , ext: 'jpg|png|gif'
            , data: { path: 'column' }
            , choose: function (obj) {
                //将每次选择的文件追加到文件队列
                var files = obj.pushFile();

                //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
                obj.preview(function (index, file, result) {
                    console.log(index); //得到文件索引
                    console.log(file); //得到文件对象
                    console.log(result); //得到文件base64编码，比如图片

                    //obj.resetFile(index, file, '123.jpg'); //重命名文件名，layui 2.3.0 开始新增

                    //这里还可以做一些 append 文件列表 DOM 的操作

                    //obj.upload(index, file); //对上传失败的单个文件重新上传，一般在某个事件中使用
                    //delete files[index]; //删除列表中对应的文件，一般在某个事件中使用
                });
            },
            before: function (obj) {
                //layer.load(); //上传loading
                //预读本地文件示例，不支持ie8
                obj.preview(function (index, file, result) {
                    $('#demo2').append('<img src="' + result + '" alt="' + file.name + '" title="点击删除" class="layui-upload-img" style="width: 100%;height: 100px;" onclick="delMultipleImgs(this)">&nbsp;')
                });
            },
            allDone: function (obj) { //当文件全部被提交后，才触发
                console.log("文件总数：" + obj.total); //得到总文件数
                console.log("成功文件总数：" + obj.successful); //请求成功的文件数
                console.log("失败文件总数：" + obj.aborted); //请求失败的文件数
            }
        });

        ////封面图片拖拽上传
        //upload.render({
        //    elem: '#CoverUpload',
        //    url: BaseApi + "/api/admin/upload", //上传接口
        //    method: 'Post',
        //    type: 'images',
        //    ext: 'jpg|png|gif',
        //    size: "2048",
        //    multiple: true,
        //    number: 4,//限制数量
        //    acceptMime: 'image/*',

        //    allDone: function (obj) { //当文件全部被提交后，才触发
        //        console.log(obj.total); //得到总文件数
        //        console.log(obj.successful); //请求成功的文件数
        //        console.log(obj.aborted); //请求失败的文件数
        //    },
        //    //成功后回调
        //    done: function (res) {
        //        if (res.code === 200) {
        //            layer.msg('上传成功');
        //            //绑定图片地址
        //            $("#ImgUrl1").val(res.data);
        //            layui.$('#CoverImg').removeClass('layui-hide').find('img').attr('src', res.data);
        //        }
        //        else {
        //            layer.msg('上传失败');
        //        }

        //        console.log(res)
        //    }
        //});

        //图标拖拽上传
        upload.render({
            elem: '#IconUpload',
            url: BaseApi + "/api/admin/upload", //上传接口
            method: 'Post',
            type: 'images',
            ext: 'jpg|png|gif',
            size: "2048",
            data: { path: 'column' },
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
            , url: BaseApi + "/api/admin/upload"//上传接口
            , accept: 'video' //视频
            , data: { path: 'column' }
            , done: function (res) {
                layer.msg('上传成功');

                if (res.code === 200)
                    $("#Video").val(res.data);
                else
                    layer.msg('上传失败');
                console.log(res)
            }
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
        form.on('submit(columnsubmit)', function (res) {

            console.log(res.elem) //被执行事件的元素DOM对象，一般为button对象
            console.log(res.form) //被执行提交的form对象，一般在存在form标签时才会返回
            console.log(res.field) //当前容器的全部表单字段，名值对形式：{name: value}

            //点击提交按钮，限制按钮点击，防止重复提交

            $("").attr("", "");

            var id = $("#columnId").val();
            //提交
            //var id = 1;
            //id大于0，执行修改
            if (id > 0) {
                var eidtApi = BaseApi + "/api/admin/Column/EditColumn";
                ajax(eidtApi, "put", res.field, "修改");
            }
            else {
                //id不存在执行添加
                var addApi = BaseApi + '/api/admin/Column/AddColumn';
                ajax(addApi, "post", res.field, "添加");
            }

            return false;
        });


    });

//移除图片
$("body").on("click", ".close", function () {
    $(this).closest("li").remove();
});

//类别下拉框
function Category(columnid, action) {
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
            if (action !== 'addSublevel') {
                var ophtmls = '<option value="0">顶级</option>';
                $("select[name=parentId]").html(ophtmls);
            }
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
            $("#parentIdSelect").html(ophtmls);

        }
    });
}