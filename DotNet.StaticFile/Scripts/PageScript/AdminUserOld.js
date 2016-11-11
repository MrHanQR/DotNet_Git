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
        }
        //, {
        //    "aTargets": [9],
        //    "bVisible": false
        //}
        ],

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

function AdminUser_Restore() {
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        if (confirm("确认要恢复该账户？")) {
            $.ajax({
                type: "post",
                url: "/AdminUser/Restore",
                data: { id: id.val() },
                dateType: "text",//返回类型
                success: function (data) {
                    if (data == "1") {
                        alert("恢复成功");
                        location.reload(true);
                    } else {
                        alert("恢复失败");
                    }
                },
                error: function () {
                    alert("服务器出现问题，禁用失败");
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
        if (confirm("确认要永久该账户？(不可恢复)")) {
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
