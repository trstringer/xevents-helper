function fillEventsListBox(data) {
    $("#EventNameList").find("option").remove();
    $("#EventSessionDescription").text("");

    var i;
    for (i = 0; i < data.length; i++) {
        $("#EventNameList")
            .append($("<option></option>")
            .attr("value", data[i].Name)
            .text(data[i].Name));
    }
}

function resetEventSearch() {
    $("#SearchInput").val("");
    $("#EventNameList").scrollTop(0);
    clearAddEventButton();
    $(".event-tile-container").remove();
}

function getAllEventsForRelease(releaseName) {
    var releaseEnc = encodeURIComponent(releaseName);

    var events = [];

    $.ajax({
        url: "../relevents/" + releaseEnc,
        async: false,
        datatype: "json",
        success: function (data) {
            events = data;
        }
    });

    return events;
}

function getSessionDefinition(sessionName) {
    var createSessionDefinition = "";

    var session = {
        Name: sessionName
    };

    $.ajax({
        url: "../getcreatesession",
        async: false,
        datatype: "json",
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify({session: session}),
        success: function (data) {
            createSessionDefinition = data;
        }
    });

    return createSessionDefinition;
}

function setAddEventButtonText(eventName) {
    $("#AddEvent").text("Add " + eventName);
    $("#AddEvent").show();
}
function clearAddEventButton() {
    $("#AddEvent").text("");
    $("#AddEvent").hide();
}

function getSelectedEvent() {
    return $("#EventNameList").val();
}

function addEvent(eventName) {
    $("#SelectedEvents").append(generateEventTile(eventName));
}
function generateEventTile(eventName) {
    var tileMarkup =
        "<div class='col-md-4 center-block col-centered event-tile-container'>" +
            "<div class='event-tile'>" +
                "<p>" + eventName + "</p>" +
            "</div>" +
        "</div>";

    return tileMarkup;
}

$(document).ready(function () {

    $("#ReleaseNameList").change(function () {
        fillEventsListBox(getAllEventsForRelease($(this).val()));
        resetEventSearch();
    });

    $("#EventNameList").change(function () {
        var releaseEnc = encodeURIComponent($("#ReleaseNameList").val());
        var eventName = $("#EventNameList").val();
        var eventNameEnc = encodeURIComponent(eventName);

        $.getJSON("../descsearch/" + releaseEnc + "/" + eventNameEnc, function (data) {
            $("#EventSessionDescription").text(data.eventDescription);
            setAddEventButtonText(eventName);
        });
    });

    $("#SearchInput").keyup(function () {
        var searchStringEnc = encodeURIComponent($(this).val().trim());
        var release = $("#ReleaseNameList").val();
        var releaseEnc = encodeURIComponent(release);

        if ($(this).val().trim().length == 0) {
            fillEventsListBox(getAllEventsForRelease(release));
        }
        else {
            $.getJSON("../searchevents/" + releaseEnc + "/" + searchStringEnc + "/false", function (data) {
                fillEventsListBox(data);
            });
        }
    });

    $("#SessionName").keyup(function () {
        $("#CreateSessionDdl").val(getSessionDefinition($(this).val()));
    });

    $("#AddEvent").click(function () {
        addEvent(getSelectedEvent());
    });

});