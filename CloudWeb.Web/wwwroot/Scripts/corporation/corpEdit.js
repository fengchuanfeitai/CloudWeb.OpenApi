﻿var getUrl = 'https://localhost:44377/api/Corporation/GetCorporation/id';
var uploadUrl = 'https://localhost:44377/api/admin/upload';
var PostUrl = 'https://localhost:44377/api/Corporation/AddCorporaion';
var GetColumnsUrl = 'https://localhost:44377/api/admin/Column/GetColumnsByParent';

layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        layer = layui.layer;

    //选择下拉框渲染
    var columnSelect = xmSelect.render({
        el: '#ColSelect',
        toolbar: { show: true },
        autoRow: true,
        direction: 'down',
        data: []
    })

    //动态绑定数据
    $.ajax({
        type: 'GET',
        url: GetColumnsUrl,
        data: { parentId: 1 },
        success: function (res) {
            var colArr = new Array();
            $.each(res.data, function (index, value) {
                var col = { name: value.colName, value: value.columnId }
                colArr.push(col);
            });
            columnSelect.update({
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
                    form.val('corp-form', {
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
                        "Sort": corporation.sort
                    });

                    layui.$('#CoverImgDiv').removeClass('layui-hide');
                    layui.$('#CoverImg').attr('src', corporation.cover);
                    layui.$('#Logo1ImgDiv').removeClass('layui-hide');
                    layui.$('#Logo1Img').attr('src', corporation.logo1);
                    layui.$('#Logo2ImgDiv').removeClass('layui-hide');
                    layui.$('#Logo2Img').attr('src', corporation.logo2);
                    layui.$('#AboutUsCoverImgDiv').removeClass('layui-hide');
                    layui.$('#AboutUsCoverImg').attr('src', corporation.aboutUsCover);
                    layui.$('#ContactUsBgImgDiv').removeClass('layui-hide');
                    layui.$('#ContactUsBgImg').attr('src', corporation.contactUsBg);
                    if (corporation.columnId != null) {
                        var ColArray = getColArray(corporation.columnId);
                        columnSelect.setValue(ColArray);
                    }
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
        }
    });

    //封面图拖拽上传   
    var CoverUpload = upload.render({
        elem: '#CoverUpload',
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Cover']").val(file);
                console.log("Cover文本" + $("input[name='Cover']").val());
                layui.$('#CoverImgDiv').removeClass('layui-hide');
                layui.$('#CoverImg').attr('src', file);
            } else {
                layer.msg('上传失败');
            }
        }
    });

    //灰色Logo图拖拽上传
    var Logo1Upload = upload.render({
        elem: '#Logo1Upload',
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Logo1']").val(file);
                console.log("Cover文本" + $("input[name='Logo1']").val());
                layui.$('#Logo1ImgDiv').removeClass('layui-hide');
                layui.$('#Logo1Img').attr('src', file);
            } else {
                layer.msg("上传失败");
            }
        }
    });

    //正常Logo图拖拽上传
    var Logo2Upload = upload.render({
        elem: '#Logo2Upload',
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Logo2']").val(file);
                console.log("Cover文本" + $("input[name='Logo2']").val());
                layui.$('#Logo2ImgDiv').removeClass('layui-hide');
                layui.$('#Logo2Img').attr('src', file);
            }
            else {
                layer.msg("上传失败");
            }
        }
    });

    //关于我们图片上传
    var AboutUsCoverUpload = upload.render({
        elem: '#AboutUsCoverUpload',
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='AboutUsCover']").val(file);
                console.log("Cover文本" + $("input[name='AboutUsCover']").val());
                layui.$('#AboutUsCoverImgDiv').removeClass('layui-hide');
                layui.$('#AboutUsCoverImg').attr('src', file);
            }
            else {
                layer.msg("上传失败");
            }
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
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='ContactUsBg']").val(file);
                console.log("Cover文本" + $("input[name='ContactUsBg']").val());
                layui.$('#ContactUsBgImgDiv').removeClass('layui-hide');
                layui.$('#ContactUsBgImg').attr('src', file);
            } else {
                layer.msg("上传失败");
            }
        }
    });

    //监听提交
    form.on('submit(save-corp)', function (res) {
        console.log(res.field);
        var columnIds = columnSelect.getValue("value");
        console.log(columnIds);
        var data = {
            "CorpId": $("input[name='CorpId']").val(),
            "Name": $("input[name='Name']").val(),
            "Cover": $("input[name='Cover']").val(),
            "Logo1": $("input[name='Logo1']").val(),
            "Logo2": $("input[name='Logo2']").val(),
            "ColumnId": columnIds.toString(),
            "AboutUs": "固定关于我们",
            "AboutUsCover": $("input[name='AboutUsCover']").val(),
            "ContactUs": $("input[name='ContactUs']").val(),
            "ContactUsBg": $("input[name='ContactUsBg']").val(),
            "Sort": $("input[name='Sort']").val(),
            "IsShow": "true"
        }
        console.log(data)

        $.ajax({
            type: "POST",
            url: PostUrl,
            async: false,
            dataType: 'json',
            data: data,
            success: function (data) {
                if (data.code != 200) {
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

function getColArray(columnId) {
    return columnId.split(',');
}