function Logout() {
    DeleteCookies();
    window.location = "/index.html";
}

var navigation = new Vue({
    el: '#navigation',
    data: {
        sessionId: GetCookie("session_id"),
        sessionKey: GetCookie("session_key"),
        isLoggedIn: false,
        isAdmin: false
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
                ignoreWait = true,
                callback = function (data) {
                    that.isLoggedIn = true;
                    that.isAdmin = (data.member.admin == 1);

                    // request.js
                    if (typeof request !== "undefined") {
                        request.lastname = data.member.lastname;
                        request.firstname = data.member.firstname;
                        request.email = data.member.email;
                    }

                    // details.js
                    if (typeof praise !== "undefined") {
                        praise.lastname = data.member.lastname;
                        praise.firstname = data.member.firstname;
                        praise.email = data.member.email;
                    }

                    // details.js
                    if (typeof details !== "undefined") {
                        details.lastname = data.member.lastname;
                        details.firstname = data.member.firstname;
                        details.email = data.member.email;
                    }
                }
            );
        }
    },
    created: function () {
        this.CheckLogin();
    }
});