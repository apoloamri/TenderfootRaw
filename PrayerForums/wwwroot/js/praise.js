var praise = new Vue({
    el: '#praise',
    data: {
        //INPUT
        lastname: "",
        firstname: "",
        email: "",
        title: "",
        message: "",
        imageUrl: "",

        //OUTPUT
        messages: "",
        valid: false
    },
    methods: {
        GetData: function () {
            return {
                "praise": {
                    "lastname": this.lastname,
                    "firstname": this.firstname,
                    "email": this.email,
                    "title": this.title,
                    "message": this.message,
                    "image_url": this.imageUrl
                }
            }
        },
        SendRequest: function () {
            var that = this;
            ShowModal("modalLoading");
            Post(
                url = "prayer/praise",
                data = that.GetData(),
                ignoreWait = false,
                callback = function (data) {
                    that.valid = true;
                    ShowModal("modalMessage");
                },
                error = function (data) {
                    var response = data.responseJSON;
                    that.messages = DisplayMessages(response.messages, true);
                    ShowModal("modalMessage");
                }
            );
        },
        ImageUpload(element) {
            var that = this;
            var files = element.target.files || element.dataTransfer.files;
            ShowModal("modalLoading");
            UploadFile(
                file = files[0],
                fileName = "",
                url = "upload/image/praise",
                callback = function (data) {
                    that.imageUrl = data.full_url;
                    HideModal();
                },
                error = function (data) {
                    var response = data.responseJSON;
                    that.messages = DisplayMessages(response.messages, true);
                    ShowModal("modalMessage");
                }
            );
        }
    }
});