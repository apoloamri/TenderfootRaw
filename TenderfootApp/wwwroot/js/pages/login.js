var login = new Vue({
    el: '#loginForm',
    data: {
        username: "",
        password: ""
    },
    methods: {
        GetData: function () {
            return {
                "username": this.username,
                "password": this.password,
                "session_id": GetCookie("sessionId"),
                "session_key": GetCookie("sessionKey")
            }
        },
        Login: function () {
            CallAjax(
                type = "POST", 
                url = "member/login", 
                data = this.GetData(),
                ignoreWait = false,
                callback = function(data) {
                    SetCookie("sessionId", data.session_id, 1);
                    SetCookie("sessionKey", data.session_key, 1);
                    window.location = "dashboard.html";
                },
                error = function(data) {
                    ShowModal("invalidLogin");
                }
            );
        },
        CheckLogin: function () {
            CallAjax(
                type = "GET", 
                url = "member", 
                data = this.GetData(),
                ignoreWait = false,
                callback = function(data) {
                },
                error = function(data) {
                    window.location = "index.html";
                }
            );
        }
    },
    created: function () {
    }
});