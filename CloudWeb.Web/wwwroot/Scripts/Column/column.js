var data = "";
layui.use('table', function () {
    var table = layui.table;
    //第一个实例
    var dataApi = "https://localhost:44377/api/admin/Column/getall";
    table.render({
        elem: '#list'
        //, height: 260
        //, url: dataApi //数据接口
        , url: '/scripts/Column/data.json' //数据接口
        , contentType: 'application/ json'//传值格式
        //, where: { pageIndex: index, pageSize: 10 }//传递参数
        , request: {
            pageName: 'PageIndex' //页码的参数名称，默认：page
            , limitName: 'PageSize' //每页数据量的参数名，默认：limit
        }
        , response: {
            statusName: 'code' //规定数据状态的字段名称，默认：code
            , statusCode: 200 //规定成功的状态码，默认：0
            , msgName: 'msg' //规定状态信息的字段名称，默认：msg
            , countName: 'count' //规定数据总数的字段名称，默认：count
            , dataName: 'data' //规定数据列表的字段名称，默认：data
        }
        //, parseData: function (res) { //res 即为原始返回的数据
        //    alert(res)
        //    return {
        //        "code": 0, //解析接口状态
        //        "msg": res.msg, //解析提示文本
        //        "count": res.count, //解析数据长度
        //        "data": res.data //解析数据列表
        //    };
        //}
        , id: 'columnId'
        , page: true //开启分页
        , limit: 10 //每页显示数量
        , limits: [10, 15]//每页可以选择的展示数量
        , text: {
            none: '暂无相关数据' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        }
        , cols: [[ //表头
            { field: '', title: '', height: 90, type: 'checkbox', width: 80 },
            { field: 'columnId', title: '编号', height: 90, width: 80, sort: true, align: 'center' },
            { field: 'colName', title: '栏目名称', width: 200, sort: true, align: 'center' },
            { field: 'localUrl', title: '跳转链接', width: 100, sort: true, align: 'center' },
            { field: 'sort', title: '排序', width: 80, sort: true, align: 'center' },
            { field: 'IsShow', title: '是否显示', width: 280, sort: true, align: 'center' },
            { field: 'createTime', title: '创建时间', width: 280, sort: true, align: 'center' },
            { field: '', title: '操作', width: 280, sort: true, templet: '', align: 'center', toolbar: '#barDemo' }
        ]]
        , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
            //,curr: 5 //设定初始在第 5 页
            , groups: 1 //只显示 1 个连续页码
            , first: false //不显示首页
            , last: false //不显示尾页
            , pageSize: 10
        }
        , done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度

            console.log(res);

            //得到当前页码
            console.log(curr);
            //得到数据总量
            console.log(count);
        }
    });

    table.on('tool(column)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        var id = obj.data.columnId; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的 DOM 对象（如果有的话）
        console.log(layEvent)
        if (layEvent === 'del') { //删除
            console.log(id)
            layer.confirm('是否删除当前数据？', function (index) {
                var ids = new Array();
                ids.push(id)
                $.ajax({
                    type: 'delete',
                    url: 'https://localhost:44377/api/admin/Column/DeleteColumn',
                    dataType:'application/json',
                    data: { ids: ids },//'ids='+arr+'&_method=delete',
                    success: function (res) {
                        if (res.code != 0)
                            alertMsg(res.msg);
                        console.log(data)
                    },
                    error: function (data) {
                        alert(2);
                    }
                });
                layer.close(index);
            });
        } else if (layEvent === 'edit') { //编辑

            //跳转编辑页面，携带id
            xadmin.open('添加栏目', '/Column/Edit?id='+id, 800, 600)

        } else if (layEvent === 'LAYTABLE_TIPS') {
            layer.alert('Hi，头部工具栏扩展的右侧图标。');
        }
    });
    table.on('checkbox(test)', function (obj) {
        console.log(obj); //当前行的一些常用操作集合
        console.log(obj.checked); //当前是否选中状态
        console.log(obj.data); //选中行的相关数据
        console.log(obj.type); //如果触发的是全选，则为：all，如果触发的是单选，则为：one
    });

    data = table.checkStatus('id')
    console.log(data)

});

function delAll(argument) {

    //var data = tableCheck.getData();

    console.log(data)
    layer.confirm('确认要删除吗？', function (index) {
        //捉到所有被选中的，发异步进行删除
        layer.msg('删除成功', { icon: 1 });
        $(".layui-form-checked").not('.header').parents('tr').remove();
    });
}


//删除
function deleteUser(id) {
    layer.confirm('确认要删除吗？', function (index) {
        url = "/User/Delete";
        parameter = { userId: id };
        $.post(url, parameter, function (data) {
            if (data.IsSuccess) {
                alertMsg("删除成功!");
            }
            else {
                alertMsg("删除失败!");
            }
        });
    });
}


//删除
function deleteAllUser() {
    var ids = new Array();
    $("div[class='layui-unselect layui-form-checkbox layui-form-checked']").each(function () {
        ids.push($(this).attr("data-id"));
    });

    if (ids.length > 0) {
        layer.confirm('确认要删除选中项吗？', function (index) {
            url = "/User/DeleteAll";
            parameter = { ids: ids.toString() };
            $.post(url, parameter, function (data) {
                if (data.IsSuccess) {
                    alertMsg("删除成功!");
                }
                else {
                    alertMsg("删除失败!");
                }
            });
        });
    }
    else {
        alertMsg("请选选择要删除的用户!");
        return;
    }
}


//搜索
function SearchUser() {
    $.ajax({
        type: "POST",
        async: true,
        url: "/User/Index",
        data: { username: $("#username").val() },
        success: function (data) {
            $("#content").empty();
            $("#content").html(data);

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