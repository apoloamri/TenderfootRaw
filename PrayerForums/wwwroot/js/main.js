function Logout() {
    DeleteCookies();
    window.location = "index.html";
}

var navigation = new Vue({
    el: '#navigation',
    data: {
        sessionId: GetCookie("session_id"),
        sessionKey: GetCookie("session_key"),
        isLoggedIn: false
    },
    methods: {
        GetData: function () {
            return {
                "session_id": this.sessionId,
                "session_key": this.sessionKey
            }
        },
        CheckLogin: function () {
            var that = this;
            Get(
                url = "member/login",
                data = that.GetData(),
                ignoreWait = false,
                callback = function (data) {
                    that.isLoggedIn = true;
                    if (typeof request !== "undefined") {
                        request.lastname = data.member.lastname;
                        request.firstname = data.member.firstname;
                        request.email = data.member.email;
                    }
                }
            );
        }
    },
    created: function () {
        this.CheckLogin();
    }
});