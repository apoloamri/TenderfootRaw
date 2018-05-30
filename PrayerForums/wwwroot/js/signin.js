new Vue({
    el: '#signIn',
    data: {
        username: "",
        password: "",
        messages: "",
        key: GetParameterByName("key")
    },
    methods: {
        GetData: function () {
            return {
                "username": this.username,
                "password": this.password,
                "key": this.key
            }
        },
        Activate: function () {
            var that = this;
            if (that.key == "" || that.key == null) {
                return;
            }
            ShowModal("modalLoading");
            Post(
                url = "member/activate",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    that.messages = "Your account is successfully activated.";
                    ShowModal("modalMessage");
                },
                error = function (data) {
                    var response = data.responseJSON;
                    that.messages = DisplayMessages(response.messages);
                    ShowModal("modalMessage");
                }
            );
        },
        Login: function () {
            var that = this;
            ShowModal("modalLoading");
            Post(
                url = "member/login",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    SetCookie("session_id", data.session_id, 1);
                    SetCookie("session_key", data.session_key, 1);
                    window.location = "community.html";
                },
                error = function (data) {
                    var response = data.responseJSON;
                    that.messages = DisplayMessages(response.messages);
                    ShowModal("modalMessage");
                }
            );
        }
    },
    created: function () {
        this.Activate();
    }
});