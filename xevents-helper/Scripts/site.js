function setInitialState() {

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

    $("#eventSearchResults").on("click", "tr", function () {
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

    $("#eventSearchResults span").hide();
}
function resetEventSearch(removeSearchString) {
    if (removeSearchString === true)
        clearEventSearchInput();
    clearEventSearchResults();
    clearEventDescription();
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
}
function clearEventDescription() {
    $("#eventDescription").text("");
}

function setSelectedEventSearchItem(item) {
    $("#eventSearchResults tr").removeClass("active success");
    $("#eventSearchResults span").hide();
    item.addClass("success");
    item.find("span").show();
}
function clearEventSearchSelection() {
    $("#eventSearchResults td").removeClass("active success");
}

$(function () {
    setInitialState();
    setEvents();
});