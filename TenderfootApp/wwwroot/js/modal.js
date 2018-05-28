function ShowModal(modalName) {
    if (modalName != null && modalName != "") {
        $("#" + modalName).removeClass("unshow");
    }
    else {
        $(".modal").removeClass("unshow");
    }
    $("#modalShadow").removeClass("unshow");
    $("body").css("overflow", "hidden");
}

function HideModal(modalName) {
    if (modalName != null && modalName != "") {
        $("#" + modalName).addClass("unshow");
    }
    else {
        $(".modal").addClass("unshow");
    }
    $("#modalShadow").addClass("unshow");
    $("body").css("overflow", "auto");
}