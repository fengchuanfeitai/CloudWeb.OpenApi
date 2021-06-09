var active = "";
layui.use('table', function () {
    var table = layui.table;
    var form = layui.form;
    //无token跳转登录
    checkToken();

    //搜素
    active = {
        reload: function () {

            table.reload('id', {
                where: {
                    columnId: $('#columnSelect').val(), TitleKeyword: $('#title').val()
                }
            });
        }
    }

    //接口
    var getlistapi = BaseApi + "/api/admin/Content/GetAll";
    table.render({
        elem: '#list'
        , url: getlistapi //数据接口
        //, url: '/script/Column/data.json' //数据接口
        //, contentType: 'application/json'//传值格式
        //, where: { columnId: $('').val(), title: $('title').val() }//传递参数
        , headers: {
            "Authorization": "Bearer " + sessionStorage.getItem('token')
        }
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
        //, cellMinWidth: 100//一般用于列宽自动分配的情况
        , id: 'id'
        , page: true //开启分页
        , limit: 10 //每页显示数量
        , limits: [10, 15]//每页可以选择的展示数量
        , text: {
            none: '暂无相关数据' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        }
        , cols: [[ //表头
            { field: '', title: '', type: 'checkbox', width: 80 },
            { field: 'id', title: '编号', width: 70, align: 'center' },
            { field: 'title', title: '标题', width: 200, align: 'center' },
            { field: 'colName', title: '所属栏目类别', width: 200, align: 'center' },
            { field: 'linkUrl', title: '跳转链接', width: 100, align: 'center' },
            { field: 'sort', title: '排序', width: 80, sort: true, align: 'center' },
            { field: 'createTime', title: '创建时间', width: 180, sort: true, align: 'center' },
            {
                field: 'isDefault', title: '是否推荐到首页', width: 120, templet: function (d) {
                    return '<input type="checkbox" value="' + d.id + '" ' + (d.isDefault == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="switchDefault" lay-text="推荐|不推荐">'
                }, align: 'center'
            },
            {
                field: 'isCarousel', title: '是否添加到轮播', width: 120, templet: function (d) {
                    return '<input type="checkbox" value="' + d.id + '" ' + (d.isCarousel == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="switchTop" lay-text="添加|不添加">'
                }, align: 'center'
            },
            {
                field: 'isPublic', title: '是否发布', width: 120, templet: function (d) {
                    return '<input type="checkbox" value="' + d.id + '" ' + (d.isPublic == 1 ? 'checked' : '') + ' name="open" lay-skin="switch"  lay-filter="switchPublic" lay-text="发布|不发布">'
                }, align: 'center'
            },
            { field: '', title: '操作', width: 180, align: 'center', toolbar: '#barContent' }
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

    //初始化下拉框
    ColumnDropDown();
    layui.form.render("select");

    //推荐首页按钮状态事件
    form.on('switch(switchDefault)', function (obj) {

        var apiurl = BaseApi + '/api/admin/Content/ChangeDefaultStatus';
        //改变状态
        var onoff = this.checked ? '1' : '0';
        var params = { id: obj.value, DefaultStatus: onoff };
        ChangeStatus(apiurl, params, form, obj);
    });

    //是否发布按钮状态事件
    form.on('switch(switchPublic)', function (obj) {
        var apiurl = BaseApi + '/api/admin/Content/ChangePublicStatus';
        //改变状态
        var onoff = this.checked ? '1' : '0';
        var params = { id: obj.value, PublicStatus: onoff };
        ChangeStatus(apiurl, params, form, obj);
    });

    //是否添加到轮播按钮状态事件
    form.on('switch(switchTop)', function (obj) {
        var apiurl = BaseApi + '/api/admin/Content/ChangeCarouselStatus';
        //改变状态
        var onoff = this.checked ? '1' : '0';
        var params = { id: obj.value, CarouselStatus: onoff };
        ChangeStatus(apiurl, params, form, obj);
    });

    //按钮操作事件
    table.on('tool(content)', function (obj) { //注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        var id = obj.data.id; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）

        switch (layEvent) {
            case 'del'://删除
                {
                    var ids = new Array();
                    ids.push(id)
                    console.log('content[delete],id:' + id)

                    var delApi = BaseApi + '/api/admin/content/DeleteContent';
                    DelAjax(delApi, 'post', '是否删除当前数据？', { ids: ids }, 'id', table);
                }
                break;
            case 'edit'://编辑
                {
                    //跳转编辑页面，携带id
                    xadmin.open('编辑内容', '/Content/Edit?id=' + id + '&action=edit', 800, 600)
                }
                break;
        }
    });

    var ids = [];
    //全选删除
    table.on('checkbox(content)', function (obj) {
        var checkStatus = table.checkStatus('id')
            , data = checkStatus.data;

        ids = new Array();//每次加载时间，重新初始化数组

        for (var i = 0; i < data.length; i++) {
            var id = data[i].id;
            ids.push(id);
        }
        console.log("事件全选删除选中数据：" + ids);

    })

    //全部删除
    $("#delall").click(function () {

        //判读是否选择数据
        if (ids.length === 0) {
            layer.msg('请先选择要删除的数据', { icon: 2 });
            return false;
        }
        var delApi = BaseApi + '/api/admin/content/DeleteContent';
        DelAjax(delApi, 'post', '确认要删除所有选中数据吗', { ids: ids }, 'id', table);
    });

});


//栏目下拉框
function ColumnDropDown(columnid) {
    console.log(columnid);
    $.ajax({
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            id: columnid,
            existTopLevel: false
        },
        url: BaseApi + '/api/admin/Column/GetDropDownList',
        success: function (res) {
            console.log(res.data);
            var ophtmls = '<option value="0">全部</option>';
            $("select[name=columnId]").html(ophtmls);
            for (var i = 0; i < res.data.length; i++) {
                var Id = res.data[i].columnId;
                var ClassLayer = res.data[i].level;
                var Title = res.data[i].colName;
                if (ClassLayer == 1) {
                    ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                } else {
                    Title = "├ " + Title;
                    Title = StringOfChar(ClassLayer - 1, "　") + Title;
                    ophtmls += "<option value=" + Id + ">" + Title + "</option>";
                }
                console.log(ClassLayer);
            }
            $("#columnSelect").html(ophtmls);

        }
    });
}

$('#search').click(function () {
    var type = $(this).data('type');
    active[type] ? active[type].call(this) : '';
});