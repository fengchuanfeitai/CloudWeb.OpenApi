layui.use(['form', 'layer', 'layedit', 'upload', 'element', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            layer = layui.layer,
            upload = layui.upload;
        //无token跳转登录
        checkToken();

        //判断是否是“添加子级”跳转
        var action = getUrlParam("action");
        ActionOperation(action, form);
        form.on('select(parentIdChange)', function (data) {
            Display(data.value);
            form.render("select");
        });
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
            }
        });

        //上传视频
        upload.render({
            elem: '#uploadvideo'
            , url: BaseApi + "/api/admin/upload"//上传接口
            , accept: 'video' //视频
            , data: { path: 'column' }
            , size: 30000
            , before: function (obj) { //obj参数包含的信息，跟 choose回调完全一致，可参见上文。
                layer.load(); //上传loading
            }
            , done: function (res) {
                layer.closeAll('loading'); //关闭loading
                if (res.code === 200) {
                    layer.msg('上传成功');
                    $("#Video").val(res.data);
                }
                else
                    layer.msg('上传失败');
            }, error: function (index, upload) {
                layer.closeAll('loading'); //关闭loading
            }
        });

        //监听提交
        form.on('submit(columnsubmit)', function (res) {
            //点击提交按钮，限制按钮点击，防止重复提交
            var sort = $('#sort').val()
            //if (sort === "") {
            //    layer.msg('请填写排序！', { icon: 2 });
            //    return false;
            //}
            if (sort.length > 0) {
                if (!(/^[0-9]*$/.test(sort))) {
                    layer.msg('排序只能是数字！', { icon: 2 });
                    return false;
                }
            }
            var summary = $('#summary').val()
            if (summary.length > 900) {
                layer.msg('描述最多只能输入900字符之内！', { icon: 2 });
                return false;
            }

            var level = $('#level').val();
            if (level <= 2) {
                var cover = $("#coverUrl").val();
                var value = $("#coverLinks").val();
                if (cover === "") {
                    layer.msg('请上传上传轮播图！', { icon: 2 });
                    return false;
                }
                if (value === "") {
                    layer.msg('请填写轮播图跳转链接！', { icon: 2 });
                    return false;
                }
                if (cover === "" && value != "") {
                    layer.msg('未上传轮播图，不能填写轮播图跳转链接！', { icon: 2 });
                    return false;
                }

                if (!(cover == "" && value == "")) {
                    var linkArr = value.split(',');
                    var coverArr = cover.split(',');
                    var tag = true;
                    var tag1 = true;
                    $.each(linkArr, function (i, v) {
                        if (v == '') {
                            tag = false;
                        }
                        else {
                            //判断链接是否有http://
                            if (v != '#') {
                                if (!(/(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?/).test(v)) {
                                    tag1 = false;
                                }
                            }
                        }
                    })
                    if (!tag) {
                        layer.msg('轮播图跳转链接占位需要”#“填位！', { icon: 2 });
                        return false;
                    }
                    if (!tag1) {
                        layer.msg('第三方链接必须以https://或http://开头', { icon: 2 });
                        return false;
                    }
                    if (linkArr.length != coverArr.length) {
                        layer.msg('跳转链接个数不等于轮播图数量，请添加或删除！。无链接用‘#’占位', { icon: 2 });
                        return false;
                    }
                }
            }

            if (level > 2) {
                var moduleVal = $("select[name='module']").val()
                if (moduleVal == null || moduleVal == "") {
                    layer.msg('请选择模块样式', { icon: 2 });
                    return false;
                }
            }

            switch (action) {
                case 'addSublevel':
                    {
                        var addApi = BaseApi + '/api/admin/Column/AddColumn';
                        ajax(addApi, "post", res.field, "添加");
                    }
                    break;
                case 'add':
                    {
                        var addApi = BaseApi + '/api/admin/Column/AddColumn';
                        ajax(addApi, "post", res.field, "添加");
                    }
                    break;
                case 'edit':
                    {
                        var eidtApi = BaseApi + "/api/admin/Column/EditColumn";
                        ajax(eidtApi, "post", res.field, "修改");
                    }
                    break;

            }
            return false;
        });
    });

//根据传入的action做不同的操作
function ActionOperation(action, form) {
    //判断action,执行不同的操作
    var userId = sessionStorage.getItem('UserId');
    switch (action) {
        case 'add': //作为添加页面操作
            {
                //添加绑定用户id
                $("#creator").val(userId);
                // 加载栏目分类所有数据下拉框
                ColumnDropDown(null, true);
                //加载组件模板下拉框
                ModuleDropDown();
            }
            break;
        case 'addSublevel': //作为添加子级页面操作
            {
                console.log('addSublevel' + sessionStorage.getItem('UserId'));
                //添加绑定用户id
                $("#creator").val(userId);
                //获取url中携带的columnId参数,不保存,加载从对应栏目对应的栏目的下拉框数据
                var columnId = getUrlParam("columnId");
                ColumnDropDown(columnId, false);
                //加载组件模板下拉框
                ModuleDropDown();
            }
            break;
        case 'edit': //作为编辑页面操作
            {
                //修改绑定用户id
                console.log("edit:" + sessionStorage.getItem('UserId'));
                $("#modifier").val(userId);
                //获取url中携带的columnId参数
                var columnId = getUrlParam("columnId");
                $("#columnId").val(columnId);//保存columnId到隐藏控件，用于编辑时的主键
                // 加载栏目分类所有数据下拉框
                ColumnDropDown(null, true);

                //加载组件模板下拉框
                ModuleDropDown();

                //数据库拉取数据，重新绑定form控件中
                $.ajax({
                    type: "get",
                    url: BaseApi + "/api/admin/Column/GetColumn"
                    //url: '/scripts/Column/editData.json' //数据接口
                    , data: { id: columnId }//传值
                    , success: function (res) {
                        if (res.code === 200) {
                            //赋值
                            form.val("columnForm", res.data);

                            $("#parentIdSelect").val(res.data.parentId);
                            if (res.data.icon !== '' & res.data.icon !== null) {

                                $('#IconImg').removeClass('layui-hide').find('img').attr('src', res.data.icon);
                            }
                            //多图赋值
                            if (res.data.coverUrl !== '' & res.data.coverUrl !== null) {

                                var pics = res.data.coverUrl.split(",")

                                for (var i = 0; i < pics.length; i++) {
                                    $('#slide-pc-priview').append('<li class="item_img"><div class="operate"><i  class="close layui-icon">  <button type="button" class="layui-btn layui-btn-sm"><i class="layui-icon"></i></button></i></div><img src="' + pics[i] + '" class="img" ><input type="hidden" value="' + pics[i] + '" class="pic" /></li>');
                                }
                            }

                            if (res.data.level > 2) {
                                //隐藏上传轮播，链接输入框
                                $('#coverUrlPic').attr('style', 'display:none');//不显示
                                $('#picCoverLinks').attr('style', 'display:none');//不显示
                                $('#divIsNews').attr('style', 'display:block')//显示
                                $('#moduleDiv').attr('style', 'display:block')//显示
                            }
                            else {
                                $('#coverUrlPic').attr('style', 'display:block');
                                $('#picCoverLinks').attr('style', 'display:block');//显示
                                $('#divIsNews').attr('style', 'display:none')//显示
                                $('#moduleDiv').attr('style', 'display:none')//显示
                            }

                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });

            }
            break;
    }
    form.render("select");//加载重新form
}

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

//栏目下拉框
function ColumnDropDown(columnid, existTopLevel) {
    $.ajax({
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            id: columnid,
            existTopLevel: existTopLevel
        },
        url: BaseApi + '/api/admin/Column/GetDropDownList',
        success: function (res) {
            console.log(res.data);
            if (res.code === 200 & res.data.length > 0) {
                Display(res.data[0].columnId);
                var ophtmls = '';
                for (var i = 0; i < res.data.length; i++) {
                    var Id = res.data[i].columnId;
                    var ClassLayer = res.data[i].level;
                    var Title = res.data[i].colName;
                    if (ClassLayer == 1 || ClassLayer == 0) {
                        ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                    } else {
                        Title = "├ " + Title;
                        Title = StringOfChar(ClassLayer - 1, "　") + Title;
                        ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                    }
                }
                $("#parentIdSelect").html(ophtmls);
            }
            else {
                layer.msg('下拉数据加载失败');
            }
        }
    });
}

function Display(columnid) {
    $.ajax({
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            id: columnid,
            existTopLevel: true
        },
        url: BaseApi + '/api/admin/Column/GetDropDownList',
        success: function (res) {
            console.log('Display:' + columnid);


            if (res.code === 200) {
                console.log(res.data);
                var plevel = res.data[0].level;
                var level = plevel + 1;
                console.log('level:' + level)
                $('#level').val(level)
                ModuleDropDown()
                if (level > 2) {
                    //隐藏上传轮播，链接输入框
                    $('#coverUrlPic').attr('style', 'display:none');//不显示
                    $('#picCoverLinks').attr('style', 'display:none');//不显示
                    $('#divIsNews').attr('style', 'display:block');
                    $('#moduleDiv').attr('style', 'display:block');
                }
                else {
                    $('#coverUrlPic').attr('style', 'display:block');
                    $('#picCoverLinks').attr('style', 'display:block');//显示
                    $('#divIsNews').attr('style', 'display:none');//不显示
                    $('#moduleDiv').attr('style', 'display:none');
                }
            }
        }
    });
}

function ModuleDropDown() {
    $.ajax({
        type: "GET",
        dataType: "json",
        async: false,
        data: {},
        url: BaseApi + '/api/admin/Column/GetModuleDownList',
        success: function (res) {
            console.log(res)
            if (res.code === 200) {
                var str = '';
                $("#bindModule option:gt(0)").remove();
                $.each(res.data, function (i, val) {
                    str += '<option value="' + val.value + '">' + val.text + '</option>';
                });
                $(str).appendTo("#bindModule");
                $("#bindModule option:eq(0)").attr("selected", 'selected');
            }
        }
    })
}
