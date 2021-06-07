/**访问接口url,发布修改发布网址 */
//window.BaseApi = "https://localhost:44377"

window.BaseApi = "http://192.168.0.138:8085"
/* 判断是否登录，不登录访问页面时，默认跳转登录页面 */
function checkToken() {

    //无token跳转登录
    var toke = sessionStorage.getItem('token');
    var userid = sessionStorage.getItem('UserId');
    if (toke === null || userid === null) {
        parent.location.href = '/home/login';
    }
}

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
 * ajax  添加、编辑公用方法
 * @param {string} path 地址
 * @param {string} method ajax方法
 * @param {object} params 传入参数
 * @param {string} msg 信息
 */
function ajax(path, method, params, msg) {
    $.ajax({
        type: method,
        url: path,
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

                layer.alert(res.msg, {
                    icon: 2
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
 * ajax 删除、全选删除
 * @param {string} delApi 删除接口地址
 * @param {string} method ajax 模式
 * @param {string} confirmMsg 弹出框确认信息
 * @param {object} params 参数
 * @param {string} tableId 要刷新的表id
 * @param {object} table 表对象
 */
function DelAjax(delApi, method, confirmMsg, params, tableId, table) {
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
                    table.reload(tableId, "", false);//刷新表格
                }
                else
                    layer.msg('删除失败', { icon: 2 });
            }
            , error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 401) {
                    layer.msg('你没有访问权限', { icon: 2 });
                    console.log(401)
                } else {
                }
            }
        });
        layer.close(index);
    });
}

/**
 * 生成指定长度的字符串,即生成strLong个str字符串
 * @param {number} strLong 字符长度
 * @param {string} str 字符
 */
function StringOfChar(strLong, str) {
    var ReturnStr = "";
    for (var i = 0; i < strLong; i++) {
        ReturnStr += str;
    }
    return ReturnStr;
}



/**
 * 改变状态公用方法
 * @param {string} api 接口地址
 * @param {object} params 参数
 * @param {object} form form对象
 * @param {object} obj form(checkbox)返回对象
 */
function ChangeStatus(api, params, form, obj) {
    console.log('选中id:' + obj.value);
    $.post(api, params, function (res) {
        console.log('content[ChangeStatus],result:' + res);
        //判断是否等于200，否则提示错误信息
        if (res.code === 200) {
            layer.msg('状态修改成功', { icon: 1 });
        }
        else {
            layer.msg('状态修改失败', { icon: 2 });
            obj.elem.checked = !obj.elem.checked;
        }
        form.render();//刷新表格
    });
}