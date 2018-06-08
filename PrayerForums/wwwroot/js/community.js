new Vue({
    el: '#praiseReport',
    data: {
        page: 1,
        count: 5,
        totalPages: 0,
        result: []
    },
    methods: {
        GetData: function () {
            return {
                "page": this.page,
                "count": this.count
            }
        },
        GetPraises: function () {
            var that = this;
            Get(
                url = "prayer/praises",
                data = that.GetData(),
                ignoreWait = true,
                callback = function (data) {
                    that.result = data.result;
                    that.totalPages = data.total_pages
                }
            );
        },
        GoToDetails: function (id) {
            window.location = "report.html?praise_id=" + id;
        },
        NextPage: function () {
            if (this.page == this.totalPages) {
                return;
            }
            this.page += 1;
            this.GetPraises();
        },
        PreviousPage: function () {
            if (this.page == 1) {
                return;
            }
            this.page -= 1;
            this.GetPraises();
        },
        SetPage: function (page) {
            this.page = page;
            this.GetPraises();
        }
    },
    created: function () {
        this.GetPraises();
    }
});

new Vue({
    el: '#prayerWall',
    data: {
        page: 1,
        count: 10,
        totalPages: 0,
        result: []
    },
    methods: {
        GetData: function () {
            return {
                "session_id": GetCookie("session_id"),
                "session_key": GetCookie("session_key"),
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
                    that.totalPages = data.total_pages
                }
            );
        },
        GoToDetails: function (id) {
            window.location = "details.html?request_id=" + id;
        },
        NextPage: function () {
            if (this.page == this.totalPages) {
                return;
            }
            this.page += 1;
            this.GetRequests();
        },
        PreviousPage: function () {
            if (this.page == 1) {
                return;
            }
            this.page -= 1;
            this.GetRequests();
        },
        SetPage: function (page) {
            this.page = page;
            this.GetRequests();
        }
    },
    created: function () {
        this.GetRequests();
    }
});