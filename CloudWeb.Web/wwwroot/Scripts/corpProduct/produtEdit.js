var getUrl = 'https://localhost:44377/api/CorpProduct/GetProductById/id';
var uploadUrl = 'https://localhost:44377/api/admin/upload';
var PostUrl = 'https://localhost:44377/api/CorpProduct/AddProduct';
var GetCorpsUrl = 'https://localhost:44377/api/Corporation/GetCorpSelectList';

layui.use(['form', 'upload', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        upload = layui.upload,
        layer = layui.layer;


    //渲染Select
    $.get(GetCorpsUrl, function (res) {
        if (res.code != 200) {
            layer.open(res.msg);
            return false
        }
        var str = ''; //声明字符串        
        $("#bindCorp option:gt(0)").remove();//重新加载前，移除第一个以外的option
        $.each(res.data, function (i, val) {
            str += '<option value="' + val.corpId + '">' + val.name + '</option>';
        });//遍历循环遍历
        $(str).appendTo("#bindCorp");//绑定
        $("#bindCorp option:eq(0)").attr("selected", 'selected'); //默认选择第一个选项
        form.render("select");//注意：最后必须重新渲染下拉框，否则没有任何效果。
    });


    //如果是编辑页面则初始化数据
    $(function () {
        var id = getUrlParam(id);
        if (id != null) {
            $.ajax({
                type: 'GET',
                url: getUrl,
                data: { id: id },
                success: function (res) {
                    if (res.code != 200) {
                        layer.open(res.msg);
                        return false;
                    }
                    var product = res.data;

                    form.val('product-form', {
                        "Id": product.id,
                        "Name": product.name,
                        "Cover": product.cover,
                        "Content": product.content,
                        "CorpId": product.corpId,
                        "LocationUrl": product.locationUrl,
                        "Sort": product.sort,
                        "IsShow": product.isShow
                    });

                    layui.$('#CoverImgDiv').removeClass('layui-hide');
                    layui.$('#CoverImg').attr('src', product.cover);
                }
            });
        }
    });

    //自定义验证规则:待完善
    form.verify({
        Name: function (value) {

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

    //监听表单提交

    form.on('submit(save-product)', function (res) {
        console.log(res.filed);

        $.ajax({
            type: "POST",
            url: PostUrl,
            async: false,
            dataType: 'json',
            data: res.file.filed,
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
})



function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}