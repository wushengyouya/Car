/*
 * 商品类别js
 *所有model的删除方法
 * 
 * @param {} url 请求的地址
 * @param {} loadData 加载数据的方法
 * @param {} id id列的类名
 * @param {any} isShowSnackBar 是否显示提示
 * @returns {} 
 */
function DeleteEntity(url, loadDataFunction, id, isShowSnackBar) {
    var $td = $("tr.mdui-table-row-selected td." + id);
    if ($td.length === 0) {
        Snackbar("没有选择商品，不可以删除");
        return;
    }
    mdui.confirm('确认删除吗?', '提示框', function () {
        var ids = [];
        $.each($td,
            function (index, obj) {
                ids[index] = obj.innerText;
            });
        //JSON.stringify(ids) //将对象解析成json字符串
        //JSON.parse(ids)  从字符串解析出json对象
        $.ajax({
            method: 'POST',
            url: url,
            data: {
                action: "delete",
                ids: JSON.stringify(ids)
            },
            dataType: "json",
            success: function (data) {
                if (data !== null) {
                    if (data.Msg === "no") {
                        Snackbar("删除失败");
                        return;
                    }
                    loadDataFunction(data.JsonData, id, data.Msg, isShowSnackBar);

                }
            },
            error: function (xhr, textStatus, errorThrown) {
                Snackbar("删除失败");
            }
        });


    }, {}, { confirmText: "确认", cancelText: "取消" });
}

/**
 * 
 * @param {any} url 请求的地址
 * @param {any} loadDataFunction 加载数据的函数
 * @param {any} id id列的类名
 * @param {any} jsonData 编辑项的数据
 * @param {any} isShowSnackBar 是否显示提示
 */
function EditEntity(url, loadDataFunction, id, jsonData, isShowSnackBar) {
    $.ajax({
        method: 'POST',
        url: url,
        data: jsonData,
        dataType: "json",
        success: function (data) {
            if (data !== null) {
                if (data.Msg === "no") {
                    Snackbar("更新失败");
                } else if (data.Msg === "had") {
                    Snackbar("该商品已存在，请修改名称");
                } else {
                    loadDataFunction(data.JsonData, id, data.Msg, isShowSnackBar);
                }
               
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            Snackbar("更新失败");
        }
    });
}


function AddEntity(url, jsonData, loadDataFunction, id, isShowSnackBar) {
    $.ajax({
        method: "post",
        dataType: "json",
        url: url,
        data: jsonData,
        success: function (data) {
            console.log(data);
            //loadDataFunction(data.JsonData, id, data.Msg, isShowSnackBar);
            if (data.Msg === "had") {
                Snackbar("该商品已存在，请修改名称");
            } else if (data.Msg === "") {
                Snackbar("添加失败");
            } else {
                window.location.href = data;
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            Snackbar("信息填写有误");
            console.log(xhr.status);
            //console.log(xhr.responseText);
            //console.log(xhr.status);
            //console.log(xhr.readyState);
            //console.log(xhr.statusText);
            ///*弹出其他两个参数的信息*/
            //console.log(textStatus);
            //console.log(errorThrown);
        }
    });
}
/**重新加载商品类别数据
 * 
 * @param {any} data json数据
 * @param {any} className id列的类名
 * @param {any} msg 操作后的结果
 */
function GoodsTypeLoadData(data, className, msg) {
    var $tr_ = $(".mdui-table tbody").find("tr");

    $.each($tr_,
        function (index, obj) {
            obj.remove();//移除表格里的所有行
        });

    for (var i = 0; i < data.length; i++) {
        var $td =
            "<td class='mdui-table-cell-checkbox'><label class='mdui-checkbox'><input type='checkbox'><i class='mdui-checkbox-icon'></i></label></td>";

        var $tr = "<tr>" + $td + "<td class='" + className + "'>" +
            data[i].goods_type_id +
            "</td><td>" +
            data[i].goods_type_name +
            "</td><td>" + data[i].goods_type_addTime + "</td></tr>";
        $(".mdui-table tbody").append($tr);
    }
    mdui.mutation();
    mdui.updateTables();//重新初始化表格
    Snackbar(msg);

}

//消息提示框
function Snackbar(msg) {
    mdui.snackbar({
        message: msg,
        position: 'right-bottom',
        buttonColor: "#fff"
    });
}

//获取发货订单选择了多少行状态
function GetSelectFlag() {
    if (GetSelectRow().length === 0) {
        Snackbar("没有选择订单");
        return false;
    }
    if (GetSelectRow().length > 1) {
        Snackbar("只能选择一个订单发货");
        return false;
    }
    return true;
}
//被选中行的所有td
function GetSelectRowTds() {
    return $("tr.mdui-table-row-selected").find("td");
}
//选中的行
function GetSelectRow() {
    return ($("tr.mdui-table-row-selected"));
}

//添加商品类别
$("#goodsTypeAdd").click(function () {
    mdui.prompt('请输入类别名', '添加商品类别',
        function (value) {
            $.ajax({
                method: 'POST',
                url: '/goodsmanager/type',
                data: {
                    action: "add",
                    goodsTypeName: value
                },
                dataType: "json",
                success: function (data) {
                    if (data !== null) {
                        if (data.Msg === "no") {
                            Snackbar("添加失败");
                           
                        }else if (data.Msg==="had") {
                            Snackbar("该类别已存在");
                        } else {
                            GoodsTypeLoadData(data.JsonData, "goodsTypeId", data.Msg);
                        }

                        
                    }


                }
            });
        },
        function (value) { },
        {
            closeOnEsc: true,
            confirmText: "确认",
            cancelText: "取消",
            confirmOnEnter: true,
            maxlength: 10
        }
    );
});

//删除商品类别
$("#goodsTypeDelete").click(function () {
    DeleteEntity("/goodsmanager/type", GoodsTypeLoadData, "goodsTypeId");

});



//编辑商品类别
$("#goodsTypeEdit").click(function () {
    if (GetSelectRow().length === 0) {
        Snackbar("未选择行要编辑的类别");
        return;
    }
    if (GetSelectRow().length > 1) {
        Snackbar("编辑只能择一行");
        return;
    }
    var $td = GetSelectRowTds();
    mdui.prompt('请输入新的类别名', '更改商品类别名',
        function (value) {
            $.ajax({
                method: 'POST',
                url: '/goodsmanager/type',
                data: {
                    action: "edit",
                    goodsId: $td.get(1).innerText,
                    newGoodsTypeName: value
                },
                dataType: "json",
                success: function (data) {
                    if (data !== null) {
                        if (data.Msg === "no") {
                            Snackbar("修改失败");
                        } else if (data.Msg === "had") {
                            Snackbar("该类别已存在");
                        } else {
                            GoodsTypeLoadData(data.JsonData, "goodsTypeId", data.Msg);
                        }
                        

                    }


                },
                error: function (xhr, textStatus, errorThrown) {
                    Snackbar("更新失败");
                }

            });
        },
        function (value) { },
        {
            defaultValue: $td.get(2).innerText,
            closeOnEsc: true,
            confirmText: "确认",
            cancelText: "取消",
            confirmOnEnter: true,
            maxlength: 10
        }
    );


});




/**重新加载商品数据
 * 
 * @param {any} data  json数据
 * @param {any} className id列的类名
 * @param {any} msg 操作后的结果
 * @param {any} isShowSnackBar 是否显示提示框
 */
function GoodsLoadData(data, className, msg, isShowSnackBar) {
    var $tr = $(".mdui-table tbody").find("tr");
    $.each($tr,
        function (index, obj) {
            obj.remove();//移除表格里的所有行
        });
    var $td =
        "<td class='mdui-table-cell-checkbox'><label class='mdui-checkbox'><input type='checkbox'><i class='mdui-checkbox-icon'></i></label></td>";
    for (var i = 0; i < data.length; i++) {
        $tr = "<tr>" + $td + "<td class='" + className + "'>" +
            data[i].goods_id +
            "</td><td>" +
            data[i].goods_type_name +
            "</td><td class='mdui-text-truncate' style = 'max-width: 100px;'><a  mdui-tooltip={content:'商品详情'} href=/goodsmanager/detail/" + data[i].goods_id + ">" +
            data[i].goods_name +
            "</a></td><td>" +
            data[i].goods_price +
            "</td><td>" +
            data[i].goods_flag +
            "</td><td class='mdui-text-truncate' style = 'max-width: 112px;' title='" + data[i].goods_info + "'>" +
            data[i].goods_info +
            "</td><td>" +
            data[i].goods_count +
            "</td><td>" +
            getDateTime(ConvertJSONDateToJSDate(data[i].goods_addTime)) +
            "</td><td class='mdui-text-truncate' style = 'max-width: 120px;' title='" + data[i].goods_title + "'>" +
            data[i].goods_title +
            "</td></tr>";

        $(".mdui-table tbody").append($tr);


    }
    mdui.mutation();
    mdui.updateTables();//重新初始化表格


    if (isShowSnackBar) {
        Snackbar(msg);
    }

}
//删除商品
$("#goodsDelete").click(function () {
    DeleteEntity("/goodsmanager/all", GoodsLoadData, "goodsId", true);
});

//分页
function Page(url, pageIndex) {

    $.ajax({
        method: 'POST',
        url: url,
        data: {
            action: "page",
            pageIndex: pageIndex
        },
        dataType: "json",
        success: function (data) {
            if (data !== null) {
                GoodsLoadData(data, "goodsId", "", false);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            Snackbar("加载失败");
        }
    });
}

//上一页
function LastPage(url) {
    pageIndex = (pageIndex - 1) < 1 ? 1 : pageIndex - 1;

    Page(url, pageIndex);
}

//下一页
function NextPage(url) {
    pageIndex = (pageIndex + 1) > totalCount ? totalCount : pageIndex + 1;

    Page(url, pageIndex);
}

$("#firstPage").click(function () {
    pageIndex = 1;
    Page("/goodsmanager/all", pageIndex);
});
$("#endPage").click(function () {
    pageIndex = totalCount;
    Page("/goodsmanager/all", pageIndex);
});
$("#lastPage").click(function () {
    LastPage("/goodsmanager/all");
});
$("#nextPage").click(function () {
    NextPage("/goodsmanager/all");
});


/*时间格式化*/
function getDateTime(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var ss = date.getSeconds();
    return year + "/" + month + "/" + day + " " + hh + ":" + mm + ":" + ss;
}
//调用的是这个方法/*时间格式化*/
function ConvertJSONDateToJSDate(jsondate) {
    var date = new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", ""), 10));
    return date;
}

