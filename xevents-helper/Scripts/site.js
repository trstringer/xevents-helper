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
    $("#AddEvent").val("Add" + eventName);
}

$(document).ready(function () {

    $("#ReleaseNameList").change(function () {
        fillEventsListBox(getAllEventsForRelease($(this).val()));
        resetEventSearch();
    });

    $("#EventNameList").change(function () {
        var releaseEnc = encodeURIComponent($("#ReleaseNameList").val());
        var eventNameEnc = encodeURIComponent($("#EventNameList").val());

        $.getJSON("../descsearch/" + releaseEnc + "/" + eventNameEnc, function (data) {
            $("#EventSessionDescription").text(data.eventDescription);
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

});