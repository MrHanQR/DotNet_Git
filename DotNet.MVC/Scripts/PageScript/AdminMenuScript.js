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

function AdminMenu_Add() {
    $("#AddModal").modal('show');
}
function AdminMenu_Edit() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        var items = id.parent().siblings();//拿到选择的那一行的各个td
        //var isHaveChildIcon = items.eq(7).children().attr("class").split(' ')[1];
        //if (isHaveChildIcon === "fa-check") {
        //    $("input[type=radio][name=HaveChild][value=0]").removeAttr("checked");
        //    $("input[type=radio][name=HaveChild][value=1]").prop("checked", true);
        //} else {
        //    $("input[type=radio][name=HaveChild][value=1]").removeAttr("checked");
        //    $("input[type=radio][name=HaveChild][value=0]").prop("checked", true);
        //}
        if (items.eq(7).children().hasClass("fa-check")) {
            $("input[type=radio][name=HaveChild][value=0]").removeAttr("checked");
            $("input[type=radio][name=HaveChild][value=1]").prop("checked", true);
        } else {
            $("input[type=radio][name=HaveChild][value=1]").removeAttr("checked");
            $("input[type=radio][name=HaveChild][value=0]").prop("checked", true);
        }
        $("#hideId").val(id.val());
        $("#Name").val(items.eq(0).text());
        $("#Icon").val(items.eq(1).children().attr("class").split(' ')[1]);
        $("#Sort").val(items.eq(2).text());
        //$("#ParentId").val(items.eq(8).text());//拿不到那个<td>的值，没法自动加载，以后再说，留个bug
        $("#ControllerNameCode").val(items.eq(4).text());
        $("#ActionNameCode").val(items.eq(5).text());
        $("#EditModal").modal('show');
    }
}
function AdminMenu_Delete() {
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
                url: "/AdminMenu/Delete",
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
function AdminMenu_SetButton() {
    var items = $('input[name="checkId"]:checked');
    var count = items.length;
    if (count < 1) {
        alert("请选择一个菜单项目");
    }  else {
        var item = items.parent().siblings();//拿到选择的那一行的各个td
        var isHaveChildIcon = item.eq(7).children().attr("class").split(' ')[1];
        if (isHaveChildIcon == "fa-check") {
            alert("无法给父节点分配权限按钮！");
        } else {
            var menuId = items.val();
            $("#hideMenuId").val(menuId);
            $.ajax({
                type: "post",
                url: "/AdminMenu/GetButton",
                data: { menuId: menuId },
                dateType: "html",
                success: function(data) {
                    $("#box-body").html(data);
                },
                error: function() {
                    alert("载入失败");
                }
            });
            $("#ButtonModal").modal('show');
        }
    }
}

function beginSetButton() {
    var itemsCount = $("input[type=checkbox][name=checkButton]:checked").length;
    if (itemsCount < 1) {
        alert("请选择相关按钮之后再提交");
        return false;//阻止提交
    }
    else {
        $("#btnOk").addClass("disabled");
    }
}

function completeSetButton(data) {
    if (data == "1") {
        location.reload(true);
    } else {
        alert("设置失败");
    }
    $("#btnOk").removeClass("disabled");
}