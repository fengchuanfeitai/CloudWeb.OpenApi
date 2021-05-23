var data = "";

layui.use('table', function () {
    var table = layui.table;

    //table实例
    table.render({
        elem: '#corporation_list', //table id
        url: '', // 数据接口地址
        conteType: 'application/json',//传值格式
        id: 'corpId',
        page: true, //开启分页
        limit: 10,//每页显示数量
        limits: [10, 15],//每页可以选择的展示数量
        text: {
            none: '暂无相关数据', //默认：无数据。注：该属性为 layui 2.2.5 开始新增
        },
        cols: [//表头
            { field: '', title: '', height: 90, type: 'checkbox', width: 80 },
            { field: 'corpId', title: '编号', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'name', title: '公司名', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'cover', title: '封面图', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'logo1', title: '灰色Logo', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'logo2', title: '正常Logo', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'columnId', title: '栏目Id', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'aboutUs', title: '关于我们', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'aboutUsCover', title: '关于我们封面图', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'contactUs', title: '联系我们', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'contactUsBg', title: '联系我们背景图', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'Sort', title: '序号', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'IsDisplay', title: '是否显示', height: 90, with: 80, sort: true, align: 'center' },
            { field: 'IsDel', title: '是否删除', height: 90, with: 80, sort: true, align: 'center' },
            { field: '', title: '操作', width: 280, sort: true, templet: '', align: 'center', toolbar: '#barDemo' }
        ],
        page: {
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'], //自定义分页布局
            groups: 1,//只显示1个连续页码
            first: false,
            last: false,
            pageSize: 10
        },
        done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            console.log(res);

            //得到当前页码
            console.log(curr);

            //得到数据总量
            console.log(count);
        }
    });

    table.on('tool(column)', function (obj) {//注：tool 是工具条事件名，test 是 table 原始容器的属性 lay-filter="对应的值"
        var id = obj.data.corpId;
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的 DOM 对象（如果有的话）
        console.log(layEvent)
    });
});