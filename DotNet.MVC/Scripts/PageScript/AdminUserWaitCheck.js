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

function AdminUser_PassCheck() {
    var items = $('input[name="checkId"]:checked');
    var count = items.length;
    if (count < 1) {
        alert("请选择要操作的数据");
    } else {
        if (confirm("确认通过" + count + "个账号？")) { //yes
            var strId = "";
            for (var i = 0; i < count; i++) {
                strId = strId + items.eq(i).val() + "|";
            }
            $.ajax({
                type: "post",
                url: "/AdminUser/PassCheck",
                data: { idList: strId },
                dateType: "text",
                success: function (data) {
                    if (data == "1") {
                        location.reload(true);
                    } else {
                        alert("对不起，审核失败");
                    }
                },
                error: function () {
                    alert("服务器错误，审核失败");
                }
            });
        }
    }
}
function AdminUser_PhysicallyDelete() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        if (confirm("确认要删除账户？(不可恢复)")) {
            $.ajax({
                type: "post",
                url: "/AdminUser/PhysicallyDelete",
                data: { id: id.val() },
                dateType: "text",//返回类型
                success: function (data) {
                    if (data == "1") {
                        alert("删除成功");
                        location.reload(true);
                    } else {
                        alert("删除失败");
                    }
                },
                error: function () {
                    alert("服务器出现问题，删除失败");
                }
            });
        }
    }
}