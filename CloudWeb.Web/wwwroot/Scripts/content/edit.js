layui.use(['form', 'layer', 'layedit', 'upload', 'laydate'],
    function () {
        $ = layui.jquery;
        var form = layui.form,
            laydate = layui.laydate
            , upload = layui.upload;


        //根据action初始化页面
        var action = getUrlParam("action");
        ActionOperation(action, form);
        form.on('select(brickType)', function (data) {
            DisplayPic(data.value);
        });
        //初始化时间控件
        laydate.render({
            elem: '#start'
            , type: 'datetime'
            , value: new Date(),//获取前三天时间
        });

        var uploadApi = BaseApi + '/api/admin/upload';
        //封面图片拖拽上传
        UploadPic(uploadApi, '#Img1Upload', '#ImgUrl1', '#Img1', upload);
        //内页封面上传
        UploadPic(uploadApi, '#Img2Upload', '#ImgUrl2', '#Img2', upload);

        //自定义验证规则
        form.verify({
        });

        //监听提交
        form.on('submit(contentsubmit)', function (res) {
            var id = $("#contentId").val();

            var columnId = $("#columnSelect").val();
            console.log(columnId)
            if (columnId === null) {
                layer.msg('请先添加栏目', { icon: 2 });
                return false;
            }

            //var id = 1;
            //id大于0，执行修改
            if (id > 0) {
                var eidtApi = BaseApi + '/api/admin/Content/EditContent';
                ajax(eidtApi, "post", res.field, "修改");
            }
            else {
                //id不存在执行添加
                var addApi = BaseApi + '/api/admin/Content/AddContent';
                ajax(addApi, "post", res.field, "添加");
            }

            return false;
        });
    });

/**
 * 上传图片公用方法
 * @param {any} api 
 * @param {any} uploadId
 * @param {any} picHiddenId
 * @param {any} picDivId
 * @param {object} upload
 */
function UploadPic(api, uploadId, picHiddenId, picDivId, upload) {

    upload.render({
        elem: uploadId,
        url: api, //上传接口
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
                $(picHiddenId).val(res.data);
                layui.$(picDivId).removeClass('layui-hide').find('img').attr('src', res.data);
            }
            else {
                layer.msg('上传失败');
            }
            console.log(res)
        }
    });





}

/**
 * 根据传入的action做不同的操作
 * @param {any} action
 * @param {object} form
 */
function ActionOperation(action, form) {
    //判断action,执行不同的操作
    //初始化富文本
    var ue = UE.getEditor('container', {
        initialFrameHeight: 200
    });
    // 加载栏目分类所有数据下拉框
    ColumnDropDown();

    switch (action) {
        case 'add': //作为添加页面操作
            {
                //添加绑定用户id
                $("#creator").val(sessionStorage.getItem('UserId'));
            }
            break;
        case 'edit': //作为编辑页面操作
            {
                //修改绑定用户id
                $("#modifier").val(sessionStorage.getItem('UserId'));
                //获取url中携带的contentId参数
                var contentId = getUrlParam("id");
                $("#contentId").val(contentId);//保存contentId到隐藏控件，用于编辑时的主键
                //数据库拉取数据，重新绑定form控件中
                ue.ready(function () {
                    $.ajax({
                        type: 'get'
                        , url: BaseApi + '/api/admin/Content/GetContent'
                        //url: '/scripts/Column/editData.json' //数据接口
                        , data: { id: contentId }//传值
                        , success: function (res) {
                            console.log(res);
                            if (res.code === 200) {
                                //表单赋值
                                $("#columnSelect").val(res.data.columnId);
                                form.val("contentform", res.data);
                                if (res.data.imgUrl1 !== null && res.data.imgUrl1 !== '') {
                                    $('#Img1').removeClass('layui-hide').find('img').attr('src', res.data.imgUrl1);
                                }

                                //初始化富文本
                                if (res.data.content !== null && res.data.content !== '') {
                                    ue.setContent(res.data.content);
                                }
                            }
                            else {
                                layer.msg("数据加载失败");
                            }
                        }
                    });
                });

            }
            break;
    }

    form.render("select");//加载重新form
}


//类别下拉框
function ColumnDropDown(columnid) {
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
            if (res.code === 200 & res.data !== null) {
                var ophtmls = '';
                $("select[name=columnId]").html(ophtmls);
                for (var i = 0; i < res.data.length; i++) {
                    var Id = res.data[i].columnId;
                    DisplayPic(res.data[0].columnId);
                    var ClassLayer = res.data[i].level;
                    var Title = res.data[i].colName;
                    if (ClassLayer == 1) {
                        ophtmls += "<option value=" + Id + "  >" + Title + "</option>";
                    } else {
                        Title = "├ " + Title;
                        Title = StringOfChar(ClassLayer - 1, "　") + Title;
                        ophtmls += "<option value=" + Id + " >" + Title + "</option>";
                    }
                    console.log(ClassLayer);
                }
                $("#columnSelect").html(ophtmls);
            }
        }
    });
}


function DisplayPic(columnid) {
    console.log('DisplayPic' + columnid);
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            id: columnid
        },
        url: BaseApi + '/api/admin/Column/GetDropDownList',
        success: function (res) {
            console.log(res);
            if (res.code === 200) {
                var isNews = res.data[0].isNews;
                if (!isNews) {
                    $('#newscover').attr('style', 'display:none');//不显示
                }
                else {
                    $('#newscover').attr('style', 'display:block');
                }
            }
        }
    });
}
