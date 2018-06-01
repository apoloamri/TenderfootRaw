new Vue({
    el: '#signUp',
    data: {
        email: "",
        lastname: "",
        firstname: "",
        username: "",
        password: "",
        messages: "",
        valid: false
    },
    methods: {
        GetData: function () {
            return {
                "member": {
                    "email": this.email,
                    "lastname": this.lastname,
                    "firstname": this.firstname,
                    "username": this.username,
                    "password": this.password
                }
            }
        },
        Register: function () {
            var that = this;
            ShowModal("modalLoading");
            Post(
                url = "member",
                data = that.GetData(),
                ignoreWait = false,
                callback = function (data) {
                    that.messages = "A confirmation email was sent to your email account.";
                    that.valid = true;
                    ShowModal("modalMessage");
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
    }
});