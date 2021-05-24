layui.use('table', function () {
    var table = layui.table;
    var form = layui.form;
    //查询列表数据接口
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
        , cellMinWidth: 100//一般用于列宽自动分配的情况
        , id: 'columnId'
        , page: true //开启分页
        , limit: 10 //每页显示数量
        , limits: [10, 15]//每页可以选择的展示数量
        , text: {
            none: '暂无相关数据' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        }
        , cols: [[ //表头
            { field: '', title: '全选', type: 'checkbox' },
            { field: 'num', title: '序号', sort: true, align: 'center' },
            { field: 'columnId', title: '编号', sort: true, align: 'center' },
            { field: 'colName', title: '栏目名称', align: 'center' },
            { field: 'localUrl', title: '跳转链接', align: 'center' },
            { field: 'sort', title: '排序', sort: true, align: 'center' },
            {
                field: 'isShow', title: '是否显示到网站', templet: function (d) {
                    console.log(d)
                    return '<input type="checkbox" value="' + d.columnId + '" ' + (d.isShow == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="switchTest" lay-text="显示|不显示">'
                }, align: 'center'
            },
            { field: 'createTime', title: '创建时间', sort: true, align: 'center' },
            { field: '', title: '操作', align: 'center', width: 280, toolbar: '#barDemo' }
        ]]
        , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
            //,curr: 5 //设定初始在第 5 页
            , groups: 1 //只显示 1 个连续页码
            , first: "首页" //不显示首页
            , last: "尾页" //不显示尾页
            , pageSize: 10
        }
        , done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            console.log("接口返回data:" + res);

            //得到当前页码
            console.log("当前页码：" + curr);
            //得到数据总量
            console.log("数据总数：" + count);
        }
    });

    //显示按钮状态事件
    form.on('switch(switchTest)', function (obj) {
        console.log(`我监听到的switch的值是：${obj.value}`);

        console.log(`我监听到的switch是否为checked：${obj.elem.checked}`);
        var apiurl = "https://localhost:44377/api/admin/Column/ChangeShowStatus";
        //改变状态
        var onoff = this.checked ? '1' : '0';
        console.log(obj.value);
        $.post(apiurl, { id: obj.value, ShowStatus: onoff }, function (res) {
            console.log(1);
            //判断是否等于200，否则提示错误信息
            if (res.code === 200) {
                layer.msg('显示状态修改成功', { icon: 1 });
                table.reload("columnId", "", false);//刷新表格
            }
            else
                layer.msg('显示状态修改失败', { icon: 2 });
        });
    });


    //操作事件
    table.on('tool(column)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        var id = obj.data.columnId; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        if (layEvent === 'del') { //删除
            console.log(id)
            layer.confirm('是否删除当前数据？', function (index) {
                var ids = new Array();
                ids.push(id)
                console.log("单选删除数据：" + ids);
                $.ajax({
                    type: 'delete',
                    url: 'https://localhost:44377/api/admin/Column/DeleteColumn',
                    dataType: 'json',
                    data: { ids: ids },//'ids='+arr+'&_method=delete',
                    success: function (res) {
                        console.log(res)
                        if (res.code === 200) {
                            layer.msg('删除成功', { icon: 1 });
                            table.reload("columnId", "", false);//刷新表格
                        }
                        else
                            layer.msg('删除失败', { icon: 2 });
                    }
                });
                layer.close(index);
            });
        } else if (layEvent === 'edit') { //编辑

            //跳转编辑页面，携带id
            xadmin.open('添加栏目', '/Column/Edit?id=' + id, 800, 600)
        }
    });
    var ids = [];
    //全选删除
    table.on('checkbox(column)', function (obj) {

        var checkStatus = table.checkStatus('columnId')
            , data = checkStatus.data;

        ids = new Array();//每次加载时间，重新初始化数组

        for (var i = 0; i < data.length; i++) {

            var id = data[i].columnId;
            ids.push(id);
        }
        console.log("事件全选删除选中数据：" + ids);

    })


    //全部删除
    $("#delall").click(function () {

        //判断是否存在选中数据
        console.log("全选删除选中数据：" + ids);
        //判读是否选择数据

        if (ids.length === 0) {
            layer.msg('请先选择要删除的数据', { icon: 2 });
            return false;
        }
        layer.confirm('确认要删除所有选中数据吗？', function (index) {
            $.ajax({
                type: 'delete',
                url: 'https://localhost:44377/api/admin/Column/DeleteColumn',
                dataType: 'json',
                data: { ids: ids },//'ids='+arr+'&_method=delete',
                success: function (res) {
                    console.log(res)
                    if (res.code === 200) {
                        layer.msg('删除成功', { icon: 1 });
                        table.reload("columnId", "", false);//刷新表格
                    }
                    else
                        layer.msg('删除失败', { icon: 2 });
                }
            });
            layer.close(index);
        });
    });
});

