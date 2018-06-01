new Vue({
    el: '#prayerWall',
    data: {
        page: 1,
        count: 10,
        result: []
    },
    methods: {
        GetData: function () {
            return {
                "page": this.page,
                "count": this.count
            }
        },
        GetRequests: function () {
            var that = this;
            Get(
                url = "prayer/requests",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    that.result = data.result;
                }
            );
        }
    },
    created: function () {
        this.GetRequests();
    }
});