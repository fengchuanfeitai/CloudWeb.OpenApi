var getUrl = 'https://localhost:44377/api/Corporation/GetCorporation/id';
var uploadUrl = 'https://httpbin.org/post';
var PostUrl = '';

layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        layer = layui.layer;

    //选择下拉框渲染
    var columns = xmSelect.render({
        el: '#ColSelect',
        toolbar: { show: true },
        autoRow: true,
        direction: 'down',
        data: []
    })

    //动态绑定数据
    $.ajax({
        type: 'GET',
        url: 'https://localhost:44377/api/admin/Column/GetColumnsByParent',
        data: { parentId: 1 },
        success: function (res) {
            var colArr = new Array();
            $.each(res.data, function (index, value) {
                var col = { name: value.colName, value: value.columnId }
                colArr.push(col);
            });
            columns.update({
                data: colArr
            })
        }
    });

    //页面初始化给页面赋值
    $(function () {
        var id = getUrlParam("id");

        if (id != null) {
            console.log("Id 不为空，编辑页面");

            $.ajax({
                type: 'GET',
                url: getUrl,
                data: { id: id },//请求参数
                success: function (res) {
                    if (res.code != 200) {
                        layer.open(res.msg);
                        return false;
                    }
                    var corporation = res.data;
                    form.val({
                        "CorpId": corporation.corpId,
                        "Name": corporation.name,
                        "Cover": corporation.cover,
                        "Logo1": corporation.logo1,
                        "Logo2": corporation.logo2,
                        "ColumnId": corporation.columnId,
                        "AboutUs": corporation.aboutUs,
                        "AboutUsCover": corporation.aboutUsCover,
                        "ContactUs": corporation.contactUs,
                        "ContactUsBg": corporation.contactUsBg,
                        "Sort": corporation.sort,
                        "IsShow": corporation.isShow
                    });

                    layui.$('#CoverImgDiv').removeClass('layui-hide');
                    layui.$('#CoverImg').attr('src', corporation.cover);
                    layui.$('#Logo1ImgDiv').removeClass('layui-hide');
                    layui.$('#Logo1Img').attr('src', corporation.logo1);
                    layui.$('#Logo2ImgDiv').removeClass('layui-hide');
                    layui.$('#Logo2Img').attr('src', corporation.logo2);
                    layui.$('#AboutUsCoverImgDiv').removeClass('layui-hide');
                    layui.$('#AboutUsCoverImg').attr('src', corporation.aboutUsCover);
                    layui.$('#ContactUsBgDiv').removeClass('layui-hide');
                    layui.$('#ContactUsBgImg').attr('src', corporation.contactUsBg);

                }
            });
        }

    });

    //自定义验证规则
    form.verify({
        Name: function (value) {
            if (value.length <= 0) {
                return '公司名不能为空';
            };
            if (value.length > 100) {
                return '公司名不能大于一百个字符';
            }
        },

        Cover: function (value) {
            if (value.length <= 0) {
                return '封面图不能为空';
            };
        },

        Logo1: function (value) {
            if (value.length <= 0) {
                return '灰色公司Logo图不能为空';
            };
        },

        Logo2: function (value) {
            if (value.length <= 0) {
                return '正常公司Logo图不能为空';
            };
        },

        AboutUs: function (value) {
            if (value.length <= 0) {
                return '关于我们不能为空';
            };
        },

        AboutUsCover: function (value) {
            if (value.length <= 0) {
                return '关于我们封面图不能为空';
            };
        },

        ContactUs: function (value) {
            if (value.length <= 0) {
                return '联系我们不能为空';
            };
        },

        ContactUsBg: function (value) {
            if (value.length <= 0) {
                return '联系我们背景图不能为空';
            };
        },
        Sort: function (value) {
            if (value.length <= 0) {
                return '排序不能为空';
            };
        },
        IsShow: function (value) {
            if (value.length <= 0) {
                return '联系我们背景图不能为空';
            };
        }
    });

    //封面图拖拽上传   
    var CoverUpload = upload.render({
        elem: '#CoverUpload',
        url: uploadUrl, //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            var file = res.files.file;
            $("input[name='Cover']").val(file);
            console.log("Cover文本" + $("input[name='Cover']").val());
            layui.$('#CoverImgDiv').removeClass('layui-hide');
            layui.$('#CoverImg').attr('src', file);
            console.log(res)
        }
    });

    //灰色Logo图拖拽上传
    var Logo1Upload = upload.render({
        elem: '#Logo1Upload',
        url: uploadUrl, //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            var file = res.files.file;
            $("input[name='Logo1']").val(file);
            console.log("Cover文本" + $("input[name='Logo1']").val());
            layui.$('#Logo1ImgDiv').removeClass('layui-hide');
            layui.$('#Logo1Img').attr('src', file);
            console.log(res)
        }
    });

    //正常Logo图拖拽上传
    var Logo2Upload = upload.render({
        elem: '#Logo2Upload',
        url: uploadUrl, //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            var file = res.files.file;
            $("input[name='Logo2']").val(file);
            console.log("Cover文本" + $("input[name='Logo2']").val());
            layui.$('#Logo2ImgDiv').removeClass('layui-hide');
            layui.$('#Logo2Img').attr('src', file);
            console.log(res)
        }
    });

    //关于我们图片上传
    var AboutUsCoverUpload = upload.render({
        elem: '#AboutUsCoverUpload',
        url: uploadUrl, //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            var file = res.files.file;
            $("input[name='AboutUsCover']").val(file);
            console.log("Cover文本" + $("input[name='AboutUsCover']").val());
            layui.$('#AboutUsCoverImgDiv').removeClass('layui-hide');
            layui.$('#AboutUsCoverImg').attr('src', file);
            console.log(res)
        }
    });

    //联系我们背景图
    var ContactUsBgUpload = upload.render({
        elem: '#ContactUsBgUpload',
        url: uploadUrl, //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            var file = res.files.file;
            $("input[name='ContactUsBg']").val(file);
            console.log("Cover文本" + $("input[name='ContactUsBg']").val());
            layui.$('#ContactUsBgImgDiv').removeClass('layui-hide');
            layui.$('#ContactUsBgImg').attr('src', file);
            console.log(res)
        }
    });

    //监听提交
    form.on('submit(save-corp)', function (res) {
        console.log(res.field)
        $.ajax({
            type: "POST",
            url: 'https://localhost:44377/api/SiteInfo/UpdateSiteInfo',
            async: false,
            data: res.field,
            success: function (data) {
                if (data.code != 0) {
                    layer.msg(data.msg)
                    return false;
                }
                layer.msg("保存成功")
                return true;
            }
        });
        return false;
    });

});

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}