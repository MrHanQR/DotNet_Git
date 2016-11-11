$(function () {
    $('#menuTable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "retrieve": true,
        "destroy": true,
        //'info': '第 _PAGE_ 页 / 总 _PAGES_ 页',
        'infoEmpty': '第 0 页 / 总 0 页',
        'infoFiltered': '(过滤总件数 _MAX_ 条)',
        "aoColumnDefs": [{
            "aTargets": [8],
            "mRender": function (data) {
                if (data === "True") {
                    return "<i class='fa fa-check'></i>";
                } else {
                    return "<i class='fa fa-close'></i>";
                }
            },
            "sWidth": "100px"
        }, {
            "aTargets": [1],
            "sWidth": "30px"
        }],
        "language": {
            "emptyTable": "没有数据",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sInfo": "从 _START_ 到 _END_ /共 _TOTAL_ 条数据",
            "sInfoEmpty": "没有数据",
            "sInfoFiltered": "(从 _MAX_ 条数据中检索)",
            "sZeroRecords": "没有检索到数据",
            "sSearch": "名称:",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "前一页",
                "sNext": "后一页",
                "sLast": "尾页"
            }
        }
    });
    //设置部门时点击确定按钮
    $("#btnOk").click(function () {
        var selectDep = $("#DepModal :checkbox:checked");
        if (selectDep.length != 1) {
            alert("请选择一个部门！");
            return false;
        } else {
            alert(selectDep.val());
            $("#hideDepId").val(selectDep.val());
            var items = $('input[name="checkId"]:checked');
            var strId = "";
            for (var i = 0; i < items.length; i++) {
                strId = strId + items.eq(i).val() + "|";
            }
            $("#hideUserList").val(strId);
            //$("#btnSub").addClass("disabled");//提交后禁用，防止多次点击
        }
    });
});

function AdminUser_Add() {
    $("#AddModal").modal('show');
}

function AdminUser_Edit() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        var items = id.parent().siblings();//拿到选择的那一行的各个td
        var src = items.eq(0).children().prop("src");
        $("#Id").val(id.val());
        $("#imgSrc").prop("src", src);
        $("#labName").text(items.eq(1).text());
        $("#UserName").val(items.eq(3).text());
        $("#UserEmail").val(items.eq(4).text());
        $("#EditModal").modal('show');
    }
}

function AdminUser_Delete() {
    var items = $('input[name="checkId"]:checked');
    var count = items.length;
    if (count < 1) {
        alert("请选择要删除的数据");
    } else {
        if (confirm("确认删除" + count + "条记录？")) { //yes
            var strId = "";
            for (var i = 0; i < count; i++) {
                strId = strId + items.eq(i).val() + "|";
            }
            $.ajax({
                type: "post",
                url: "/AdminUser/Delete",
                data: { idList: strId },
                dateType: "text",
                success: function (data) {
                    if (data == "1") {
                        location.reload(true);
                    } else {
                        alert("删除失败");
                    }
                },
                error: function () {
                    alert("删除失败");
                }
            });
        }
    }
}

function AdminUser_DisEnable() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        var items = id.parent().siblings();//拿到选择的那一行的各个td
        var isAble = items.eq(7).children().attr("class").split(' ')[1];
        if (isAble === "fa-check") {
            if (confirm("确认要禁用该账户？")) {
                $.ajax({
                    type: "post",
                    url: "/AdminUser/DisEnable",
                    data: { id: id.val(), disable: 1 },
                    dateType: "text",//返回类型
                    success: function (data) {
                        if (data == "1") {
                            alert("禁用成功");
                            location.reload(true);
                        } else {
                            alert("遇到问题，禁用失败");
                        }
                    },
                    error: function () {
                        alert("服务器出现问题，禁用失败");
                    }
                });
            }
        } else {
            if (confirm("确认要启用该账户？")) {
                $.ajax({
                    type: "post",
                    url: "/AdminUser/DisEnable",
                    data: { id: id.val(), disable: 0 },
                    dateType: "text",//返回类型
                    success: function (data) {
                        if (data == "1") {
                            alert("启用成功");
                            location.reload(true);
                        } else {
                            alert("遇到问题，启用失败");
                        }
                    },
                    error: function () {
                        alert("服务器出现问题，启用失败");
                    }
                });
            }
        }
    }
}

function AdminUser_Details() {
    var items = $('input[name="checkId"]:checked');
    var count = items.length;
    if (count != 1) {
        alert("请选择一条要查看的数据");
    } else {
        $.ajax({
            url: "/AdminUser/Details",
            data: { id: items.val() },
            dateType: "html",
            success: function (data) {
                //alert(data);
                $("#detailDiv").html(data);
            },
            error: function () {
                $("#detailDiv").html("服务器遇到错误");
            }
        });
        $("#DitailModal").modal('show');
    }
}

function AdminUser_SetDepartment() {
    var id = $('input[name="checkId"]:checked');
    if (id.length < 1) {
        alert("请选择要操作的用户！");
    } else {
        $("#DepModal").modal('show');
    }
}

function beginSetDep() {
    $("#btnSub").addClass("disabled");//提交后禁用，防止多次点击
}

function completeSetDep() {
    $("#btnSub").removeClass("disabled");
}
