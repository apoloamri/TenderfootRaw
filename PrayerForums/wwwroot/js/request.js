var request = new Vue({
    el: '#request',
    data: {
        lastname: "",
        firstname: "",
        email: "",
        request: ""
    },
    methods: {
        GetData: function () {
            return {
                "lastname": this.lastname,
                "firstname": this.firstname,
                "email": this.email,
                "request": this.request
            }
        },
        Request: function () {
            var that = this;
            Post(
                url = "request",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                },
                error = function (data) {

                }
            );
        }
    },
    created: function () {
    }
});