//CONFIGURATIONS
var apiUrl = "http://localhost:60400/";
//CONFIGURATIONS -- end

//REQUEST FUNCTIONS
function UploadFile(file, fileName, url, callback, error) {
    var formData = new FormData();
    formData.append("file", file);
    formData.append("name", fileName);
    $.ajax({
        type: "POST",
        url: apiUrl + url,
        data: formData,
        success: callback,
        processData: false,
        contentType: false,
        error: error
    });
}
function Get(url, data, ignoreWait, callback = null, error = null) {
    CallAjax("GET", url, data, ignoreWait, callback, error);
}
function Post(url, data, ignoreWait, callback = null, error = null) {
    CallAjax("POST", url, data, ignoreWait, callback, error);
}
function Put(url, data, ignoreWait, callback = null, error = null) {
    CallAjax("PUT", url, data, ignoreWait, callback, error);
}
function Delete(url, data, ignoreWait, callback = null, error = null) {
    CallAjax("DELETE", url, data, ignoreWait, callback, error);
}
function CallAjax(type, url, data, ignoreWait, callback = null, error = null) {
    if (underRequest && !ignoreWait) {
        return;
    }
    underRequest = true;
    $.ajax({
        type: type,
        url: apiUrl + url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: data,
        success: callback,
        error: error,
        complete: function (data) { underRequest = false; }
    });
}
var underRequest = false;
//REQUEST FUNCTIONS -- end

//DISPLAY MESSAGES FROM RESPONSE
function DisplayMessages(object, isHtml = false) {
    var output = "";
    var items = [];
    for (var property in object) {
        var value = object[property];
        if (items.includes(value)) {
            continue;
        }
        items.push(value);

        if (isHtml) {
            output += "<p>" + value + "</p>";
        }
        else {
            output += value + "\n";
        }
    }
    return output;
}
//DISPLAY MESSAGES FROM RESPONSE -- end

//COOKIES
function SetCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function GetCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(";");
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === " ") {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function DeleteCookies() {
    document.cookie
        .split(";")
        .forEach(function (c) {
            document.cookie = c.replace(/^ +/, "")
                .replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/");
        });
}
//COOKIES -- end

//GET QUERY STRING VALUE
function GetParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return "";
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
//GET QUERY STRING VALUE -- end

//BUTTON LINK
function VisitPage(url) {
    window.location = url;
}
//BUTTON LINK -- end

//PAGINATIONS

//PAGINATIONS -- end