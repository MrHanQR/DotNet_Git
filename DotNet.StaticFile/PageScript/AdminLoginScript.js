
var city;
$(function () {
    //调用新浪api获取用户所在城市
    $.ajax({
        url: "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js",
        type: "get",
        dataType: "script",
        timeout: 3000,    //3s请求不到新浪的api就停止发送请求，进入error回调函数
        success: function (result) {
            try {
                city = remote_ip_info.city == remote_ip_info.province ? remote_ip_info.province : remote_ip_info.province + remote_ip_info.city;
                $("#hideCity").val(city);
            } catch (e) {
                city = "未知";
                $("#hideCity").val(city);
            }
        },
        error: function (data, textStatus, jqXHR) {
            city = "未知";
            $("#hideCity").val(city);
        }
    });
    $("#hideCity").val(city);

    $("#sign").click(function () {
        if ($("#passwordsignup").val() == $("#passwordsignup_confirm").val()) {
            return true;
        } else {
            alert('两次密码不一致，请检查输入');
            return false;
        }
    });
});
function ChangeCheckCode() {
    var str = $("#img").attr("src") + "1";
    $("#img").attr("src", str);
}
//提交之后，执行的代码
function AfterSubmit(data) {
    if (data == "ok") {
        window.location.href = "/AdminHome/Index";
    } else {
        ChangeCheckCode();
        $("#loginBtn").removeAttr("disabled");//启用
        alert(data);
    }
}
//开始提交，禁用点击按钮，防止连击
function DisableButton() {
    $("#loginBtn").attr({ "disabled": "disabled" }); //点击登陆后先禁用登录按钮，防止连击
}
//注册提交之后
function AfterSign(data) {
    if (data == "ok") {
        // window.location.href = "/Home/Index";
        alert('注册成功！');
    } else {
        ChangeCheckCode();
        $("#sign").removeAttr("disabled");//启用
        alert(data);
    }
}
//提交注册表单
function DisableSign() {
    $("#sign").attr({ "disabled": "disabled" }); //点击登陆后先禁用登录按钮，防止连击
}
