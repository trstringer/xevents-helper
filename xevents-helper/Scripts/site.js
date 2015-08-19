function setInitialState() {
    $("#addEvent").hide();
}
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
            resetEventSearch(false);
        }
    });

    $("#eventSearchResults").on("click", "td", function () {
        setSelectedEventSearchItem($(this));

        // pull back the event description for the selected 
        // event
        //
        retrieveEventDescription($(this).text().trim());
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
    hideAddEventButton();
    clearEventDescription();

    var i;
    for (i = 0; i < events.length; i++) {
        addEventSearchResult(events[i].Name);
    }
}
function clearEventSearchResults() {
    $("#eventSearchResults tr").remove();
}
function clearEventSearchInput() {
    $("#eventSearchInput").val("");
}
function addEventSearchResult(eventName) {
    $("#eventSearchResults").append(
        '<tr><td>' + eventName + '</td><td><span class="glyphicon glyphicon-plus"</td></tr>');
}
function showAddEventButton() {
    $("#addEvent").show();
}
function hideAddEventButton() {
    $("#addEvent").hide();
}
function resetEventSearch(removeSearchString) {
    if (removeSearchString === true)
        clearEventSearchInput();
    clearEventSearchResults();
    clearEventDescription();
    hideAddEventButton();
}

function retrieveEventDescription(eventName) {
    $.ajax({
        url: "../descsearch/" + getReleaseName() + "/" + eventName,
        datatype: "json",
        success: function (data) {
            populateEventDescription(data.eventDescription);
        }
    });
}
function populateEventDescription(eventDescription) {
    $("#eventDescription").text(eventDescription);
    showAddEventButton();
}
function clearEventDescription() {
    $("#eventDescription").text("");
}

function setSelectedEventSearchItem(item) {
    $("#eventSearchResults td").removeClass("active success");
    item.addClass("success");
}
function clearEventSearchSelection() {
    $("#eventSearchResults td").removeClass("active success");
}

$(function () {
    setInitialState();
    setEvents();
});