//刷新验证码
function newValidateCode() {
    var verCode = document.getElementById("img_validateCode");
    verCode.setAttribute("src", "/User/CreateValidateCode?p=" + Math.random());
}
//注册成功5秒后跳转
var s = 5;
function Redirect()
{
    document.getElementById("s").innerHTML = s;
    s--;
    if (s >= 0) {
        setTimeout('Redirect()', 1000);
    }
    else
        window.location.href = "/User/Login";
}