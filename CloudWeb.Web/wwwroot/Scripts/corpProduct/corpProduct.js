//全局变量
var getListUrl = 'https://localhost:44377/api/CorpProduct/GetPageList';
var deleteUrl = 'https://localhost:44377/api/CorpProduct/DelProduct';
var changeShowUrl = 'https://localhost:44377/api/CorpProduct/ChangeShowStatus';
var GetCorpsUrl = 'https://localhost:44377/api/Corporation/GetCorpSelectList';


layui.use(['table', 'layer', 'form'], function () {
    var $ = layui.jquery,
        table = layui.table,
        layer = layui.layer,
        form = layui.form;
    //渲染select框
    $.get(GetCorpsUrl, function (res) {
        if (res.code != 200) {
            layer.open(res.msg);
            return false
        }
        var str = ''; //声明字符串        
        $("#corpSelect option:gt(0)").remove();//重新加载前，移除第一个以外的option
        $.each(res.data, function (i, val) {
            str += '<option value="' + val.corpId + '">' + val.name + '</option>';
        });//遍历循环遍历
        $(str).appendTo("#corpSelect");//绑定
        $("#corpSelect option:eq(0)").attr("selected", 'selected'); //默认选择第一个选项
        form.render("select");//注意：最后必须重新渲染下拉框，否则没有任何效果。
    });


    //table实例
    table.render({
        elem: '#corp_productlist', //table id
        url: getListUrl, // 数据接口地址
        conteType: 'application/json',//传值格式
        id: 'id',
        request: {
            pageName: 'PageIndex',
            limitName: 'PageSize'
        },
        response: {
            statusName: 'code', //规定数据状态的字段名称
            statusCode: 200, //规定成功的状态码
            msgName: 'msg',//规定状态信息的字段名称
            countName: 'count',//规定数据总数的字段名称
            dataName: 'data' //规定数据列表的字段名称
        },
        page: true, //开启分页
        limit: 10,//每页显示数量
        limits: [10, 15, 20],//每页可以选择的展示数量
        text: {
            none: '暂无相关数据', //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
        cols: [[ //表头
            { type: 'checkbox', width: 50 },      
            { field: 'id', title: '编号', width: 70, sort: true, align: 'center' },
            { field: 'name', title: '产品名', width: 150, align: 'center' },
            { field: 'corpName', title: '所属公司', width: 150, align: 'center' },
            { field: 'locationUrl', title: '跳转链接', width: 150, align: 'center' },
            { field: 'sort', title: '排序', width: 70, sort: true, align: 'center' },
            { field: 'createTime', title: '创建时间', width: 200, sort: true, align: 'center' },
            {
                field: 'isShow', title: '是否显示到网站', width: 150, templet: function (d) {
                    return '<input type="checkbox" value="' + d.id + '" ' + (d.isShow == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="IsShow" lay-text="显示|不显示">'
                }, sort: true, align: 'center'
            },
            { title: '操作', align: 'center', toolbar: '#bar' }
        ]],
        page: {
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'], //自定义分页布局
            groups: 5,//只显示1个连续页码
            first: false,
            last: false,
            pageSize: 10
        }
    });

    table.on('tool(corp_products)', function (obj) {
        //注：tool 是工具条事件名，lay-filter="对应的值"
        var id = obj.data.id; //获得当前行数据
        var layEvent = obj.event; //获得 lay - event 对应的值

        if (layEvent === 'del') {
            //删除          
            layer.confirm('是否删除当前数据？', function (index) {
                var ids = new Array();
                ids.push(id)
                delAjax(ids);
            });
        }
        else if (layEvent === 'edit') {
            //编辑
            xadmin.open('编辑公司信息', '/CorpProduct/Edit?id=' + id, 800, 600)
        } else if (layEvent === 'LAYTABLE_TIPS') {
            layer.alert('Hi，头部工具栏扩展的右侧图标。');
        }
    });

    //监控按钮状态事件
    form.on('switch(IsShow)', function (obj) {
        //改变状态
        var onoff = this.checked ? '1' : '0';
        console.log(obj.value);
        $.post(changeShowUrl, { id: obj.value, ShowStatus: onoff }, function (res) {
            console.log(1);
            //判断是否等于200，否则提示错误信息
            if (res.code === 200) {
                layer.msg('显示状态修改成功', { icon: 1 });
                table.reload("id", "", false);//刷新表格
            }
            else
                layer.msg('显示状态修改失败', { icon: 2 });
        });
    });

    var active = {
        reload: function () {
            console.log("a")
            table.reload('id', {
                where: { CorpId: $("#corpSelect").val(), NameKeyword: $("#SearchName").val() }
            });
        },
        createProduct: function () {
            xadmin.open('添加公司展品', '/CorpProduct/Edit', 800, 600)
        },
        delSelected: function () {
            var checkStatus = table.checkStatus('corpId');
            var ids = new Array();
            $.each(checkStatus.data, function (index, value) {
                ids.push(value.corpId);
            });
            console.log(ids);
            layer.confirm('确定删除所选展品吗？', function (index) {
                delAjax(ids);
            });
        }
    };

    $('.toolbars .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    $('#search').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    })

});

//删除
function delAjax(ids) {
    $.ajax({
        type: 'delete',
        url: deleteUrl,
        dataType: 'json',
        data: { ids: ids },
        success: function (res) {
            if (res.code != 200) {
                layer.msg(res.msg);
                return false;
            }
            layer.msg('删除成功');
            setTimeout(function () {
                location.reload();
            }, 1000);
        }
    });
}