
layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        element = layui.element
    layer = layui.layer;

    //页面初始化给页面元素赋值
    $(function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:44377/api/SiteInfo/GetSiteInfo",
            success: function (res) {
                console.log(res)
                if (res.code != 0) {
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
        url: 'https://httpbin.org/post', //改成您自己的上传接口
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            layui.$('#logoImg').removeClass('layui-hide').find('img').attr('src', res.files.file);
            console.log(res)
        }
    });

    //微信公众号图片上传
    var WechatUpload = upload.render({
        elem: '#WeChatPublicNoUpload',
        url: '',
        method: 'Post',
        type: 'images',
        ext: 'jpg|png|gif',
        //成功后回调
        done: function (res) {
            layer.msg('上传成功');
            layui.$('#WeChatPublicNoUpload').removeClass('layui-hide').find('img').attr('src', res.files.file);
            console.log(res)
        }
    });

    //监听提交
    form.on('submit(save-siteinfo)', function (res) {
        console.log(res.field)
        $.ajax({
            type: "PUT",
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
