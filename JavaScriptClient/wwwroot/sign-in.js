var createState = function () {
    return "SessionValueMakeItABitLongerafwertsdfdfgsdfasdfasdfawerfqweadwsfcasdfasdfqwetfadsfcasdfv";
}

var createNonce = function () {
    return "NonceValuegrrdfvzxcvazsdfvasdasdfasertgasdfasdfasdfasfdasfgasdfasdfasfdasfd";
}


var signIn = function () {
    var redirectUrl = "https://localhost:44324/Home/SignIn";
    var responseType = "id_token token"
    var scope = "openid ApiOne";
    var authUrl = "/connect/authorize/callback" +
        "?client_id=client_id_js" +
        "&redirect_uri="+encodeURIComponent(redirectUrl)+
        "&response_type="+encodeURIComponent(responseType)+
        "&scope="+encodeURIComponent(scope)+
        "&nonce="+createNonce() +
        "&state="+createState();

    var returnUrl = encodeURIComponent(authUrl);

    window.location.href = "https://localhost:44332/Auth/Login?ReturnUrl=" + returnUrl;

    console.log(authUrl);
    console.log(returnUrl);
}