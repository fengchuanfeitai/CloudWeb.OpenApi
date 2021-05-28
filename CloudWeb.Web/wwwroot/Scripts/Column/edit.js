layui.use(['form', 'layer', 'layedit', 'upload', 'element', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer,
            upload = layui.upload;
        //无token跳转登录
        //checkToken();
        //编辑时加载方法
        //判断url中是否携带id参数，携带参数则表示是编辑，否则是添加
        var id = getUrlParam("id");

        //修改
        if (id != null) {

            //保存id
            $("#columnId").val(id);
            //判断是否是“添加子级”跳转
            var action = getUrlParam("action");

            if (action === 'addSublevel') {
                Category(id, action);
                layui.form.render("select");
            }
            else {
                //下拉框
                Category();
                layui.form.render("select");

             
                $.ajax({
                    type: "get",
                    url: BaseApi + "/api/admin/Column/GetColumn"
                    //url: '/scripts/Column/editData.json' //数据接口
                    , data: { id: id }//传值
                    , success: function (res) {
                        var result = res.data;
                        console.log(result);
                        if (res.code === 200) {
                            //赋值
                            form.val("columnForm", res.data);

                            $("#parentIdSelect").val(res.data.parentId);
                            if (res.data.icon !== '' & res.data.icon !== null) {
                            
                                $('#IconImg').removeClass('layui-hide').find('img').attr('src', res.data.icon);
                            }
                            //多图赋值
                            if (res.data.coverUrl !== '' & res.data.coverUrl !==null) {

                                var pics = res.data.coverUrl.split(",")

                                for (var i = 0; i < pics.length; i++) {
                                    $('#slide-pc-priview').append('<li class="item_img"><div class="operate"><i  class="close layui-icon">  <button type="button" class="layui-btn layui-btn-sm"><i class="layui-icon"></i></button></i></div><img src="' + pics[i] + '" class="img" ></li>');
                                }
                            }

                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        }
        else {
            //下拉框
            Category();
            layui.form.render("select");
        }

        upload.render({
            elem: '#slide-pc',
            url: BaseApi + "/api/admin/upload", //上传接口
            exts: 'jpg|png|jpeg',
            multiple: true,
            data: { path: 'column' },
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
                $('#slide-pc-priview').append('<li class="item_img"><div class="operate"><i  class="close layui-icon">  <button type="button" class="layui-btn layui-btn-sm"><i class="layui-icon"></i></button></i></div><img src="' + res.data + '" class="img"><input type="hidden" value="' + res.data + '" class="pic" /></li>');

                //图片地址重新赋值
                var arr = $(".pic");
                var arr1 = [];
                arr.each(function () {
                    arr1.push($(this).val());
                });
                $("#coverUrl").val(arr1.toString());
            }
        });


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
                    $("#icon").val(res.data);
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
                if (res.code === 200) {
                    layer.msg('上传成功');
                    $("#Video").val(res.data);
                }
                else
                    layer.msg('上传失败');
                console.log(res)
            }
        });

        //自定义验证规则
        form.verify({
        });

        //监听提交
        form.on('submit(columnsubmit)', function (res) {

            console.log(res.elem) //被执行事件的元素DOM对象，一般为button对象
            console.log(res.form) //被执行提交的form对象，一般在存在form标签时才会返回
            console.log(res.field) //当前容器的全部表单字段，名值对形式：{name: value}

            //点击提交按钮，限制按钮点击，防止重复提交

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
    //图片地址重新赋值
    var arr = $(".pic");
    var arr1 = [];
    arr.each(function () {
        arr1.push($(this).val());
    });
    $("#coverUrl").val(arr1.toString());

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