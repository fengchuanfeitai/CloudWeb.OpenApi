var selectData;
var getUrl = BaseApi + '/api/Corporation/GetCorporation/id';
var uploadUrl = BaseApi + '/api/admin/upload';
var PostUrl = BaseApi + '/api/Corporation/AddCorporaion';
var GetColumnsUrl = BaseApi + '/api/admin/Column/GetColumnsByParent';

layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        layer = layui.layer;
    //无token跳转登录
    checkToken();
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
        data: { parentId: 1, level: 2 },
        success: function (res) {
            if (res.data.length <= 0) {
                layer.msg('所属栏目为空！<br />请先在一级栏目“仿真实验云”下，添加仿真实验云类别', {
                    time: 10000, //10s后自动关闭
                    btn: ['知道了']
                });
                return false;
            }
            var colArr = new Array();
            $.each(res.data, function (index, value) {
                var col = { name: value.colName, value: value.columnId }
                colArr.push(col);
            });
            selectData = colArr;
            columnSelect.update({
                data: colArr
            })
        },
        complete: function () {
            var id = getUrlParam("id");
            if (id != null) {
                active['initPage'].call(this)
            }
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
            if (value.length <= 0)
                return '封面图必须上传'
        },
        Logo1: function (value) {
            if (value.length <= 0)
                return '灰色Logo必须上传'
        },
        Logo2: function (value) {
            if (value.length <= 0)
                return '正常Logo图必须上传'
        },
        ContactUs: [/http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/, '第三方链接必须以https://或http://开头'],
        AboutUsCover: function (value) {
            if (value.length <= 0)
                return '关于我们图片必须上传'
        },
        Sort: function (value) {
            if (value.length > 0) {
                if (!(/^\d$/.test(value))) {
                    return '排序只能是数字';
                }
            }
        }
    });
    var active = {
        //页面初始化给页面赋值
        initPage: function () {
            var id = getUrlParam("id");
            $("input[name='Creator']").val(sessionStorage.getItem("UserId"));
            $.ajax({
                type: 'GET',
                url: getUrl,
                data: { id: id },//请求参数
                success: function (res) {
                    if (res.code != 200) {
                        layer.msg(res.msg);
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
                        "IsShow": corporation.isShow,
                        "Sort": corporation.sort
                    });
                    if (corporation.cover != null) {
                        layui.$('#CoverImgDiv').removeClass('layui-hide');
                        layui.$('#CoverImg').attr('src', corporation.cover);
                    }
                    if (corporation.logo1 != null) {
                        layui.$('#Logo1ImgDiv').removeClass('layui-hide');
                        layui.$('#Logo1Img').attr('src', corporation.logo1);
                    }
                    if (corporation.logo2 != null) {
                        layui.$('#Logo2ImgDiv').removeClass('layui-hide');
                        layui.$('#Logo2Img').attr('src', corporation.logo2);
                    }
                    if (corporation.aboutUsCover != null) {
                        layui.$('#AboutUsCoverImgDiv').removeClass('layui-hide');
                        layui.$('#AboutUsCoverImg').attr('src', corporation.aboutUsCover);
                    }
                    if (corporation.contactUsBg != null) {
                        layui.$('#ContactUsBgImgDiv').removeClass('layui-hide');
                        layui.$('#ContactUsBgImg').attr('src', corporation.contactUsBg);
                    }
                    if (corporation.columnId != null) {
                        var ColArray = getColArray(corporation.columnId);
                        columnSelect.setValue(ColArray);
                    }
                }
            });
        }
    }

    //封面图拖拽上传   
    var CoverUpload = upload.render({
        elem: '#CoverUpload',
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        exts: 'jpg|png|gif',
        data: { path: 'corp_cover' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Cover']").val(file);
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
        exts: 'jpg|png|gif',
        data: { path: 'corp_logo' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Logo1']").val(file);
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
        exts: 'jpg|png|gif',
        data: { path: 'corp_logo' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='Logo2']").val(file);
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
        exts: 'jpg|png|gif',
        data: { path: 'corp_aboutUs' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='AboutUsCover']").val(file);
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
        url: uploadUrl,
        method: 'Post',
        type: 'images',
        exts: 'jpg|png|gif',
        data: { path: 'corp_contactUs' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='ContactUsBg']").val(file);
                layui.$('#ContactUsBgImgDiv').removeClass('layui-hide');
                layui.$('#ContactUsBgImg').attr('src', file);
            } else {
                layer.msg("上传失败");
            }
        }
    });

    //监听提交
    form.on('submit(save-corp)', function (res) {
        if (selectData.length <= 0) {
            layer.msg('所属栏目为空！<br />请先在一级栏目“仿真实验云”下，添加仿真实验云类别', {
                time: 10000, //10s后自动关闭
                btn: ['知道了']
            });
            return false;
        }

        //获取Columns数组
        var columnIds = columnSelect.getValue("value");
        if (columnIds.length <= 0) {
            layer.msg('请选择栏目！')
            return false;
        }
        var postData = {
            "CorpId": $("input[name='CorpId']").val(),
            "Name": $("input[name='Name']").val(),
            "Cover": $("input[name='Cover']").val(),
            "Logo1": $("input[name='Logo1']").val(),
            "Logo2": $("input[name='Logo2']").val(),
            "ColumnId": columnIds.toString(),
            "AboutUs": $("textarea[name='AboutUs']").val(),
            "AboutUsCover": $("input[name='AboutUsCover']").val(),
            "ContactUs": $("input[name='ContactUs']").val(),
            "ContactUsBg": $("input[name='ContactUsBg']").val(),
            "Sort": $("input[name='Sort']").val(),
            "IsShow": $("input[name='IsShow']").val(),
            "Creator": $("input[name='Creator']").val(),
        };

        var frameIndex = parent.layer.getFrameIndex(window.name); //获取窗口索引

        $.ajax({
            type: "POST",
            url: PostUrl,
            async: false,
            dataType: 'json',
            data: postData,
            success: function (data) {
                if (data.code != 200) {
                    layer.msg(data.msg)
                    return false;
                }
                layer.msg("保存成功")
                setTimeout(function () {
                    parent.layer.close(frameIndex);
                    parent.location.reload();
                }, 1000);
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