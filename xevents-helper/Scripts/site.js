function setEvents () {
    $("#eventSearchResults td").click(function () {
        setSelectedEventSearchItem($(this));
    });
}

function setSelectedEventSearchItem(item) {
    $("#eventSearchResults td").removeClass("active success");
    item.addClass("success");
}
function clearEventSearchSelection() {
    $("#eventSearchResults td").removeClass("active success");
}

$(function() {
    setEvents();
});