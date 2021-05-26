/**访问接口url,发布修改发布网址 */
window.BaseApi = "https://localhost:44377"

/**
 * 获取url中携带参数的值
 * @param {string} name 参数
 */
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

/**
 * ajax 公用方法
 * @param {string} path 地址
 * @param {string} method ajax方法
 * @param {object} params 传入参数
 * @param {string} msg 信息
 */
function ajax(path, method, params, msg) {
    $.ajax({
        type: method,
        url: BaseApi + path,
        data: params,
        async: false,
        success: function (res) {
            console.log(res);
            if (res.code === 200) {

                layer.alert(msg + "成功", {
                    icon: 6
                }, function () {
                    //关闭当前frame
                    xadmin.close();

                    // 可以对父窗口进行刷新 
                    xadmin.father_reload();
                });
                return false;
            }
            else {

                layer.alert(msg + "失败", {
                    icon: 6
                }, function () {
                    //关闭当前frame
                    xadmin.close();
                });
                return false;
            }
        },
        error: function (res) {
            console.log(res)
        }
    });
}

/**
 * 
 * @param {any} datetime
 * @param {any} fmt
 */
function Format(datetime, fmt) {
    if (datetime != null) {
        datetime = new Date(parseInt(datetime.replace("/Date(", "").replace(")/", ""), 10));
        var o = {
            "M+": datetime.getMonth() + 1,                 //月份   
            "d+": datetime.getDate(),                    //日   
            "H+": datetime.getHours(),                   //小时   
            "m+": datetime.getMinutes(),                 //分   
            "s+": datetime.getSeconds(),                 //秒   
            "q+": Math.floor((datetime.getMonth() + 3) / 3), //季度   
            "S": datetime.getMilliseconds()             //毫秒   
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (datetime.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
}