$('input[type="checkbox"]').change(function () {
    this.value = (Number(this.checked));
});

var request = new Vue({
    el: '#request',
    data: {
        lastname: "",
        firstname: "",
        email: "",
        request: "",
        response: 0,
        sendEmail: 0,
        messages: "",
        valid: false
    },
    methods: {
        GetData: function () {
            return {
                "request": {
                    "lastname": this.lastname,
                    "firstname": this.firstname,
                    "email": this.email,
                    "request": this.request,
                    "response": this.response,
                    "send_email": (this.sendEmail) ? 1 : 0
                }
            }
        },
        SendRequest: function () {
            var that = this;
            ShowModal("modalLoading");
            Post(
                url = "prayer/request",
                data = that.GetData(),
                ignoreWait = false,
                callback = function (data) {
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