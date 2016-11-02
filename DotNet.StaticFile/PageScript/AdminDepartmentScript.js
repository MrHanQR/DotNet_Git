function AdminDepartment_Add() {
    $("#AddModal").modal('show');
}

function AdminDepartment_Edit() {
    var items = $("#divLeft input[name='checkDepId']:checked");
    if (items.length!=1) {
        alert('请选择一项进行操作!');
    } else {
        var id = items.val();
        $("#hideId").val(id);
        $("#DepartmentName").val(items.attr("txt"));
        $("#ParentId").val(items.attr("parentid"));
        $("#Sort").val(items.attr("sort"));
        var haveChild = items.attr("havechild");
        if (haveChild === "True") {//有子节点
            $("input[type=radio][name=HaveChild][value=0]").removeAttr("checked");
            $("input[type=radio][name=HaveChild][value=1]").prop("checked", true);
        } else {
            $("input[type=radio][name=HaveChild][value=1]").removeAttr("checked");
            $("input[type=radio][name=HaveChild][value=0]").prop("checked", true);
        }
        $("#EditModal").modal('show');
    }

}

function AdminDepartment_Delete() {
    var id = $("input[name='checkDepId']:checked");
    if (id.length!=1) {
        alert("请勾选一项要删除的项目");
    } else {
        if (confirm("确定删除该项？其子节点与相关信息都将被删除！")) {
            $.ajax({
                url: "",
                type: "post",
                data: { id: id.val() },
                dateType: "text",
                sucdess: function (data) {
                    if (data==1) {
                        alert('删除成功');
                        location.reload(true);
                    } else {
                        alert('删除失败');
                    }
                },
                error: function () {
                    alert('服务器错误，删除失败');
                }
            })
        }
    }

}

function beginAdd() {
    $("#btnOk").addClass("disabled");//提交后禁用，防止多次点击
}

function completeAdd(data) {
    if (data == 1) {
        location.reload(true);
    } else {
        alert("添加失败");
    }
    $("#btnOk").removeClass("disabled");
}

function beginEdit() {
    $("#btnEditOk").addClass("disabled");//提交后禁用，防止多次点击
}

function completeEdit(data) {
    if (data == 1) {
        location.reload(true);
    } else {
        alert("修改失败");
    }
    $("#btnEditOk").removeClass("disabled");
}

function failEdit() {
    alert('服务器错误,修改失败');
    $("#btnEditOk").removeClass("disabled");
}

function openUserList(depId) {
    var divLeft = $("#divLeft");
    var boxDiv = $("#boxDiv");
    var width = boxDiv.width() / 2 - 50;
    var height = divLeft.height();
    divLeft.width(width);
    if (!document.getElementById('divRight')) {
        boxDiv.append("<div id='divRight' class='tree well' style='float: left;margin-left: 5px'></div>");
        var divRight = $("#divRight");
        divRight.width(width);
        divRight.height(height);
    }
    
}