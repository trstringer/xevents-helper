function setEvents () {
    $("#eventSearchResults td").click(function () {
        setSelectedEventSearchItem($(this));

        // pull back the event description for the selected 
        // event
        //
        updateEventDescription("You selected the event " + $(this).text().trim());
    });
}

function searchEvents(releaseName, searchString) {
    $.ajax({
        url: "../searchevents/" + releaseName + "/" + searchString,
        datatype: "json"
    });
}

function updateEventDescription(eventDescription) {
    $("#eventDescription").text(eventDescription);
}
function clearEventDescription() {
    $("#eventDescription").text();
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