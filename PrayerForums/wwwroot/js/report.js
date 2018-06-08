var details = new Vue({
    el: '#report',
    data: {
        // Input
        praiseId: GetParameterByName("praise_id"),
        
        // Output
        result: {},
        messages: "",
        valid: false
    },
    methods: {
        GetData: function () {
            return {
                "praise_id": this.praiseId
            }
        },
        GetDetails: function () {
            var that = this;
            Get(
                url = "prayer/report",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    that.result = data.result;
                },
                error = function (data) {
                    var response = data.responseJSON;
                    alert(DisplayMessages(response.messages));
                    window.location = "community.html";
                }
            );
            HideModal();
        }
    },
    created: function () {
        this.GetDetails();
    }
});