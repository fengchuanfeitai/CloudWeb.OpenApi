var getUrl = BaseApi + '/api/SiteInfo/GetSiteInfo';
var uploadUrl = BaseApi + '/api/admin/upload';
var putUrl = BaseApi + '/api/SiteInfo/UpdateSiteInfo';


layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        layer = layui.layer;
    //无token跳转登录
    checkToken();
    //页面初始化给页面元素赋值
    $(function () {
        $.ajax({
            type: "GET",
            url: getUrl,
            success: function (res) {
                console.log(res)
                if (res.code != 200) {
                    layer.msg(res.msg)
                    return false;
                }
                var siteInfo = res.data;
                form.val('siteInfo-form', {
                    "Id": siteInfo.id,
                    "SiteTitle": siteInfo.siteTitle,
                    "SiteKeyword": siteInfo.siteKeyword,
                    "SiteDesc": siteInfo.siteDesc,
                    "SiteLogo": siteInfo.siteLogo,
                    "CopyRight": siteInfo.copyRight,
                    "Icp": siteInfo.icp,
                    "Tel": siteInfo.tel,
                    "Address": siteInfo.address,
                    "WeChatPublicNo": siteInfo.weChatPublicNo
                });

                if (siteInfo.siteLogo != null) {
                    layui.$('#SiteLogoImgDiv').removeClass('layui-hide');
                    layui.$('#SiteLogoImg').attr('src', siteInfo.siteLogo);
                }
                if (siteInfo.weChatPublicNo != null) {
                    layui.$('#WeChatPublicNoImgDiv').removeClass('layui-hide');
                    layui.$('#WeChatPublicNoImg').attr('src', siteInfo.weChatPublicNo);
                }
            }
        });
    });

    //自定义验证规则
    form.verify({
        SiteTitle: function (value) {
            if (value.length <= 0) {
                return '网站标题不能为空';
            };
            if (value.length > 200) {
                return '网站标题不能大于200个字符';
            }
        },
        SiteKeyword: function (value) {
            if (value.length <= 0) {
                return '网站关键字不能为空';
            }
            if (value.length >= 200) {
                return '网站关键字不能大于200个字符';
            }
        },
        SiteDesc: function (value) {
            if (value.length <= 0) {
                return '网站描述不能为空';
            }
            if (value.length > 1000) {
                return '网站描述不能大于1000个字符';
            }
        },
        SiteLogo: function (value) {
        },
        CopyRight: function (value) {
            if (value.length <= 0) {
                return '版权信息不能为空';
            }
            if (value.length > 200) {
                return '版权信息不能大于200个字符';
            }
        },
        Icp: function (value) {
            if (value.length <= 0) {
                return '备案号不能为空';
            }
            if (value.length > 200) {
                return '备案号不能大于200个字符';
            }
        },
        Tel: function (value) {
            if (value.length <= 0) {
                return '联系方式不能为空';
            }
            if (value.length > 20) {
                return '联系方式不能大于20个字符';
            }
        },
        Address: function (value) {
            if (value.length <= 0) {
                return '地址不能为空';
            }
            if (value.length > 500) {
                return '地址不能大于500个字符';
            }
        },
        WeChatPublicNo: function (value) {
        }
    });

    //Logo图片拖拽上传
    var logoUpload = upload.render({
        elem: '#logoUpload',
        url: uploadUrl,
        method: 'POST',
        type: 'images',
        exts: 'jpg|png|gif',
        data: { path: 'site' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='SiteLogo']").val(file);
                layui.$('#SiteLogoImgDiv').removeClass('layui-hide');
                layui.$('#SiteLogoImg').attr('src', file);
            } else {
                layer.msg('上传失败');
            }
        }
    });

    //微信公众号图片上传
    var WechatUpload = upload.render({
        elem: '#WeChatPublicNoUpload',
        url: uploadUrl,
        method: 'POST',
        type: 'images',
        exts: 'jpg|png|gif',
        data: { path: 'site' },
        //成功后回调
        done: function (res) {
            if (res.code === 200) {
                layer.msg('上传成功');
                var file = res.data;
                $("input[name='WeChatPublicNo']").val(file);
                layui.$('#WeChatPublicNoImgDiv').removeClass('layui-hide');
                layui.$('#WeChatPublicNoImg').attr('src', file);
            } else {
                layer.msg('上传失败');
            }
        }
    });

    //监听提交
    form.on('submit(save-siteinfo)', function (res) {
        $.ajax({
            type: "PUT",
            url: putUrl,
            async: false,
            data: res.field,
            success: function (data) {
                if (data.code != 200) {
                    layer.msg(data.msg)
                    return false;
                }
                layer.msg("保存成功");
                return true;
            }
        });
        return false;
    });

});
