//CONFIG
var appId = '500661543455968';
var version = 'v2.8';
var scope = 'public_profile, email';
var fields = 'id, first_name, last_name, email, link, gender, picture';
//CONFIG - end

//DEFAULTS
window.fbAsyncInit = function () {
    FB.init({
        appId: appId,
        cookie: true,
        xfbml: true,
        version: version
    });

    FB.AppEvents.logPageView();

    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            //do something
        }
    });
};
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js#xfbml=1";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
//DEFAULTS - end

function FbLogin() {
    FB.login(function (response) {
        if (response.authResponse) {
            GetFbUserData();
        }
        else {
            alert("Facebook log in failed.");
        }
    }, { scope: scope });
}

function GetFbUserData() {
    FB.api('/me', { locale: 'en_US', fields: fields },
        function (response) {
            var data = {
                "user_id": response.id,
                "email": response.email,
                "lastname": response.last_name,
                "firstname": response.first_name
            }
            SaveData(data);
        });
}

function SaveData(data) {
    Post(
        url = "member/facebook",
        data = data,
        ignoreWait = true,
        callback = function (data) {
            SetCookie("session_id", data.session_id, 1);
            SetCookie("session_key", data.session_key, 1);
            window.location = "community.html";
        },
        error = function (response) {
            alert("Facebook log in failed.");
        }
    );
}