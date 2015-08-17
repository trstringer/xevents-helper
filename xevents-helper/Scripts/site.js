function setEvents() {
    $("#eventSearchInput").keyup(function () {
        var searchString = $(this).val();

        if (searchString.length >= 3) {
            // search for the event(s)
            //
            searchEvents(getReleaseName(), searchString);
        }
        else {
            // clear any search results
            //
        }
    });

    $("#eventSearchResults").on("click", "td", function () {
        setSelectedEventSearchItem($(this));

        // pull back the event description for the selected 
        // event
        //
        updateEventDescription("You selected the event " + $(this).text().trim());
    });
}

function getReleaseName() {
    return $("#releaseList option:selected").text();
}

function searchEvents(releaseName, searchString) {
    $.ajax({
        url: "../searchevents/" + releaseName + "/" + searchString + "/false",
        datatype: "json",
        success: function (data) {
            populateEventSearchResults(data);
        }
    });
}
function populateEventSearchResults(events) {
    clearEventSearchResults();

    var i;
    for (i = 0; i < events.length; i++) {
        addEventSearchResult(events[i].Name);
    }
}
function clearEventSearchResults() {
    $("#eventSearchResults tr").remove();
}
function addEventSearchResult(eventName) {
    $("#eventSearchResults").append(
        "<tr><td>" + eventName + "</td></tr>");
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