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
            "aTargets": [5],
            "mRender": function (data) {
                if (data.length>25) {
                    return data.substring(0,25)+"...";
                } else {
                    return data;
                }
            },
            "sWidth": "200px"
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
});
$(function() {
    $("#btnDep").click(function () {
        $("#addOrEdit").val("add");
        $("#AddModalDep").modal('show');
    });
    $("#btnDepEdit").click(function () {
        $("#addOrEdit").val("edit");
        $("#AddModalDep").modal('show');
    });
    $("#btnOk").click(function () {//选择部门ok
        var items = $("#AddModalDep :checkbox:checked");
        if (items.length!=1) {
            alert("请选择一个部门");
        }
        //else if (items.attr("havechild") != "False") {
        //    alert("请不要选择父节点");
        //}
        else {
            if ($("#addOrEdit").val()=="add") {
                $("#idList").val(items.val());
                $("#RoleDepId").val(items.attr("txt"));
                $("#AddModalDep").modal('hide');
            } else if ($("#addOrEdit").val() == "edit") {
                //修改编辑Model的内容
                $("#DepId").val(items.val());
                $("#DepName").val(items.attr("txt"));
                $("#AddModalDep").modal('hide');
            }
            
        }
            
    });
});
function AdminRole_Add() {
    $("#AddModal").modal('show');
}
function beginAdd() {
    if ($("#RoleDepId").val().length<1) {
        alert("请选择部门");
        return false;
    } else {
        $("#btnSub").addClass("disabled");//提交后禁用，防止多次点击
    }
}
function completeAdd(data) {
    if (data.responseText == "1") {
        location.reload(true);
    } else {
        alert("添加失败");
    }
    $("#btnSub").removeClass("disabled");
}
//清空部门树的选中项目
function ClearDepTreeSelected() {
    $("#AddModalDep :checkbox:checked").attr("checked", false);
}

function AdminRole_Edit() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        var items = id.parent().siblings();//拿到选择的那一行的各个td
        $("#RoleId").val(id.val());
        $("#EditRoleName").val(items.eq(0).text());
        $("#DepName").val(items.eq(1).text());
        $("#EditRoleDesc").val(items.eq(4).text());
        $("#EditModal").modal('show');
    }
}
function beginEdit() {
    $("#btnSubEdit").addClass("disabled");//提交后禁用，防止多次点击
}
function completeEdit(data) {
    if (data.responseText == "1") {
        location.reload(true);
    } else {
        alert("修改失败");
    }
    $("#btnSubEdit").removeClass("disabled");
}

function AdminRole_Delete() {
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
                url: "/AdminRole/Delete",
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