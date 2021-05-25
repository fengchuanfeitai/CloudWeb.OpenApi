//全局变量
var Selected;
var getListUrl = 'https://localhost:44377/api/Corporation/GetAll';
var deleteUrl = 'https://localhost:44377/api/Corporation/DelCorporation';

layui.use(['table', 'layer', 'form'], function () {
    var $ = layui.jquery,
        table = layui.table,
        layer = layui.layer,
        form = layui.form;

    //table实例
    table.render({
        elem: '#corplist', //table id
        url: getListUrl, // 数据接口地址
        conteType: 'application/json',//传值格式
        id: 'corpId',
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
            /*{ field: 'index', title: '序号', width:70, sort: true, align: 'center' },*/
            { field: 'corpId', title: '编号', width: 70, sort: true, align: 'center' },
            { field: 'name', title: '公司名', width: 200, align: 'center' },
            { field: 'columnId', title: '栏目Id', width: 100, align: 'center' },
            { field: 'sort', title: '序号', width: 70, sort: true, align: 'center' },
            { field: 'createTime', title: '创建时间', width: 200, sort: true, align: 'center' },
            {
                field: 'isShow', title: '是否显示到网站', width: 150, templet: function (d) {
                    return '<input type="checkbox" value="' + d.corpId + '" ' + (d.isShow == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="IsShow" lay-text="显示|不显示">'
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
        },
        done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            console.log(res);
            //得到当前页码
            //console.log(curr);
            //得到数据总量
            // console.log(count);
        }
    });

    table.on('tool(corporations)', function (obj) {
        //注：tool 是工具条事件名，lay-filter="对应的值"
        var id = obj.data.columnId; //获得当前行数据
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
            xadmin.open('编辑公司信息', '/Corporation/Edit?id=' + id, 800, 600)
        } else if (layEvent === 'LAYTABLE_TIPS') {
            layer.alert('Hi，头部工具栏扩展的右侧图标。');
        }
    });

    //监控按钮状态事件
    form.on('switch(IsShow)', function (obj) {
        var apiurl = "https://localhost:44377/api/Corporation/ChangeShowStatus";
        //改变状态
        var onoff = this.checked ? '1' : '0';
        console.log(obj.value);
        $.post(apiurl, { id: obj.value, ShowStatus: onoff }, function (res) {
            console.log(1);
            //判断是否等于200，否则提示错误信息
            if (res.code === 200) {
                layer.msg('显示状态修改成功', { icon: 1 });
                table.reload("corpId", "", false);//刷新表格
            }
            else
                layer.msg('显示状态修改失败', { icon: 2 });
        });
    });

    //监听表格复选框选择
    table.on('checkbox(corporations)', function (obj) {
        //Selected.data.add();
        console.debug(obj);
        console.debug(JSON.stringify(obj.data));//当前行的一些常用操作集合
        //console.log(obj.checked); //当前是否选中状态
        //console.log(obj.data); //选中行的相关数据
        //console.log(obj.type); //如果触发的是全选，则为：all，如果触发的是单选，则为：one
    });

    var active = {
        createCorp: function () {
            xadmin.open('添加公司', '/Corporation/Edit', 800, 600)
        },
        delSelected: function () {
            var checkStatus = table.checkStatus('corpId');
            var ids = new Array();
            $.each(checkStatus.data, function (index, value) {
                ids.push(value.corpId);
            });
            console.log(ids);
            layer.confirm('确定删除所选公司吗？', function (index) {
                delAjax(ids);
            });
        }
    };

    $('.toolbars .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

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
            location.reload();
        }
    });
}

//弹出框，刷新页面
function alertMsg(msg) {
    layer.open({
        content: msg,
        yes: function (index, layero) {
            location.reload();
            layer.close(index);
        }
    });
}