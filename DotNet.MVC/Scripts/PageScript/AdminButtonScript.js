$(function () {
    $('#buttonTable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "retrieve": true,
        "destroy": true,
        "aoColumnDefs": [{ "sWidth": "250px", "aTargets": [7] }],
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
    //设置checkbox点击效果
    //$('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
    //    checkboxClass: 'icheckbox_flat-green',
    //    radioClass: 'iradio_flat-green'
    //});
});

function AdminButton_Add() {
    $("#AddModal").modal('show');
}
function AdminButton_Edit() {
    //var id = $("[name='checkId'][checked='checked']");
    var id = $('input[name="checkId"]:checked');
    if (id.length != 1) {
        alert("请选择一条数据！");
    } else {
        var items = id.parent().siblings();//拿到选择的那一行的各个td
        $("#hideId").val(id.val());
        $("#EditButtonName").val(items.eq(0).text());
        $("#EditButtonIcon").val(items.eq(1).children().attr("class").split(' ')[1]);
        $("#EditButtonSort").val(items.eq(2).text());
        $("#EditButtonHttpMethod").val(items.eq(4).text());
        $("#EditButtonActionName").val(items.eq(5).text());
        $("#EditButtonDesc").val(items.eq(6).text());
        $("#EditModal").modal('show');
    }
}
function AdminButton_Delete() {
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
                url: "/AdminButton/Delete",
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