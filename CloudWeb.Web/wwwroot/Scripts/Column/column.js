layui.use(['form', 'laypage', 'layer', 'table', 'element'], function () {
    var form = layui.form
    //无token跳转登录
    checkToken();
    //列表加载
    Starting_method();

    //显示按钮状态事件
    form.on('switch(switchTest)', function (obj) {
        var apiurl = BaseApi + '/api/admin/Column/ChangeShowStatus';
        //改变状态
        var onoff = this.checked ? '1' : '0';
        var params = { id: obj.value, ShowStatus: onoff };
        ChangeStatus(apiurl, params, form, obj);
    });
});


//加载table
function Starting_method() {

    layui.config({
        base: '/lib/module/treetable-lay/'
    }).use(['layer', 'treeTable'], function () {
        var $ = layui.jquery;
        var layer = layui.layer;
        var treeTable = layui.treeTable;

        // 渲染树形表格
        var insTb = treeTable.render({
            elem: '#columnTreeTb',
            //url: '/Scripts/column/data.json',
            url: BaseApi + '/api/admin/Column/getall',//数据接口
            cellMinWidth: 100,
            where: { pageIndex: 1, pageSize: 10 },//传递参数
            tree: {
                iconIndex: 2,           // 折叠图标显示在第几列
                isPidData: true,        // 是否是id、pid形式数据
                idName: 'columnId',  // id字段名称
                pidName: 'parentId',     // pid字段名称
                openName: 'colName',// 展示字段
                arrowType: 'arrow' //折叠箭头样式
            }
            , text: {
                none: '暂无相关数据' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
            },
            cols: [[
                //{ type: 'numbers' },
                { type: 'checkbox' },
                { field: 'columnId', title: '编号', align: 'left' },
                {
                    field: 'colName', title: '栏目名称', align: 'left'
                },
                { field: 'level', title: '栏目级别', align: 'center', hide: true },
                { field: 'locationUrl', title: '跳转链接', align: 'center' },
                { field: 'sort', title: '排序', sort: true, align: 'center' },
                {
                    field: 'createTime', title: '创建时间', sort: true, align: 'center'
                },
                {
                    field: 'isShow', title: '是否显示到网站', templet: function (d) {
                        return '<input type="checkbox" value="' + d.columnId + '" ' + (d.isShow == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="switchTest" lay-text="显示|不显示">'
                    }, align: 'center'
                },
                {
                    field: '', title: '操作', align: 'center', width: 280, toolbar: '#sublevelbar'
                }
            ]]
            , parseData: function (res) { //res 即为原始返回的数据
                return {
                    "code": 0, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": res.count, //解析数据长度
                    "data": res.data //解析数据列表
                };
            }
            , done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                console.log("接口返回data:" + res.data);
            }
        });


        //操作事件
        treeTable.on('tool(columnTreeTb)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
            var id = obj.data.columnId; //获得当前行数据
            var level = obj.data.level;
            console.log('选中行level：' + level)
            console.log('选中行id：' + id)
            var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
            switch (layEvent) {
                case 'add'://添加子级
                    {
                        //跳转编辑页面，携带id
                        xadmin.open('添加栏目', '/Column/Edit?columnId=' + id + '&action=addSublevel', 800, 600)
                    }
                    break;
                case 'del':
                    {
                        var level = obj.data.level;//获取栏目级别
                        var ids = new Array();
                        ids.push(id);
                        if (level === 1) {
                            layer.msg('当前为一级栏目不能删除', { icon: 2 });
                        } else {
                            var confirmMsg = '是否删除选中数据？';
                            var delApi = BaseApi + '/api/admin/Column/DeleteColumn';
                            DelAjax(delApi, 'post', confirmMsg, { ids: ids }, insTb)
                        }
                    }
                    break;
                case 'edit'://添加子级
                    {
                        console.log("Column[edit],id:" + id)
                        //跳转编辑页面，携带id
                        xadmin.open('编辑栏目', '/Column/Edit?columnId=' + id + '&action=edit', 800, 600)
                    }
                    break;
            }
        });

        var ids = [];
        //全选删除
        treeTable.on('checkbox(columnTreeTb)', function (obj) {
            //存在bug,全选按钮问题，待修改
            var data = insTb.checkStatus(true);

            console.log(data);
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

            var delApi = BaseApi + '/api/admin/Column/DeleteColumn';
            DelAjax(delApi, 'post', '确认要删除所有选中数据吗？', { ids: ids }, insTb)

        });

    });

}

/**
 * ajax 删除、全选删除
 * @param {string} delApi 删除接口地址
 * @param {string} method ajax 模式
 * @param {string} confirmMsg 弹出框确认信息
 * @param {object} params 参数
 * @param {object} insTb 表对象
 */
function DelAjax(delApi, method, confirmMsg, params, insTb) {
    layer.confirm(confirmMsg, function (index) {

        $.ajax({
            type: method,
            url: delApi,
            dataType: 'json',
            data: params,//'ids='+arr+'&_method=delete',
            success: function (res) {
                console.log(res)
                if (res.code === 200) {
                    layer.msg('删除成功', { icon: 1 });
                    insTb.reload();//刷新表格
                }
                else
                    layer.msg('删除失败', { icon: 2 });
            }
        });
        layer.close(index);
    });
}
