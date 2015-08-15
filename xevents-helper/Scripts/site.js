function setEvents() {
    $("#eventSearchResults td").hover(function () {
        $(this).addClass("active");
    });
    $("#eventSearchResults td").mouseleave(function () {
        $(this).removeClass("active");
    });
}

$(function () {
    setEvents();
})();