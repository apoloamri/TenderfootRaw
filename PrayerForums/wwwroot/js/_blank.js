var _URL = window.URL || window.webkitURL;

$("#imageUploader").on("change", function () {
    var file, img;
    if ((file = this.files[0])) {
        img = new Image();
        img.onload = function () {
            UploadFile(
                fileName = "test_image",
                elementId = "imageUploader",
                url = "image/upload",
                callback = function (data) {

                },
                error = function (data) {

                }
            );
        };
        img.onerror = function () {
            alert("Not a valid file:" + file.type);
        };
        img.src = _URL.createObjectURL(file);
    }
});