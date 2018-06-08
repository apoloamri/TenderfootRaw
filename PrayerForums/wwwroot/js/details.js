var details = new Vue({
    el: '#details',
    data: {
        // Input
        requestId: GetParameterByName("request_id"),
        lastname: "",
        firstname: "",
        email: "",
        response: "",

        // Output
        result: {},
        replies: [],
        replyCount: 0,
        messages: "",
        valid: false
    },
    methods: {
        GetData: function () {
            return {
                "session_id": GetCookie("session_id"),
                "session_key": GetCookie("session_key"),
                "response": {
                    "request_id": this.requestId,
                    "lastname": this.lastname,
                    "firstname": this.firstname,
                    "email": this.email,
                    "response": this.response
                },
                "request_id": this.requestId
            }
        },
        GetDetails: function () {
            var that = this;
            Get(
                url = "prayer/details",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    that.result = data.result;
                    that.replies = data.replies;
                    that.replyCount = data.reply_count;
                },
                error = function (data) {
                    var response = data.responseJSON;
                    alert(DisplayMessages(response.messages));
                    window.location = "community.html";
                }
            );
            HideModal();
        },
        SendResponse: function () {
            var that = this;
            that.valid = false;
            ShowModal("modalLoading");
            Post(
                url = "prayer/reply",
                data = that.GetData(),
                ignoreWait = false,
                callback = function (data) {
                    that.valid = true;
                    that.response = "";
                    ShowModal("modalMessage");
                },
                error = function (data) {
                    var response = data.responseJSON;
                    that.messages = DisplayMessages(response.messages, true);
                    ShowModal("modalMessage");
                }
            );
        }
    },
    created: function () {
        this.GetDetails();
    }
});