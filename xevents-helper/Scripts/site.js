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

    $("#releaseList").change(function () {
        resetEventSearch(true);
    });

    $("#eventSearchResults").on("click", "tr", function () {
        setSelectedEventSearchItem($(this));

        // pull back the event description for the selected 
        // event
        //
        retrieveEventDescription($(this).text().trim());
    });

    $("#eventSearchResults").on("click", "span", function () {
        addEventSelection($(this).attr("data-eventname"));
    });
}

function getReleaseName() {
    return $("#releaseList option:selected").text();
}

function searchEvents(releaseName, searchString) {
    $.ajax({
        url: "searchevents/" + releaseName + "/" + searchString + "/false",
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
        '<tr><td>' + eventName + '</td><td><span data-eventname="' + eventName + '" class="event-add-clicker glyphicon glyphicon-plus"></span></td></tr>');

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

function addEventSelection(eventName) {
    $("#eventSelections").append(
        '<div><p>' + eventName + '</p></div>');

    addActionSelectionToEventSelection(eventName);
}
function addActionSelectionToEventSelection(eventName) {
    var $eventSelection = $("#eventSelections").has("p:contains('" + eventName + "')");
    if ($eventSelection.length === 0)
        return;

    retrieveAllActions(getReleaseName(), $eventSelection);
}
function retrieveAllActions(releaseName, $eventSearchResultContainer) {
    $.ajax({
        url: "../actions/" + releaseName,
        datatype: "json",
        success: function (data) {
            $eventSearchResultContainer.append('<select class="form-control"></select>');
            $.each(data, function (index, value) { 
                $eventSearchResultContainer.find("select").append("<option>" + value.Name + "</option>");
            });
        }
    });
}

$(function () {
    setInitialState();
    setEvents();
});