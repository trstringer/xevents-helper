$(document).ready(function () {

    $("#EventNameList").change(function () {
        //alert($("#EventNameList").val())

        //$("#EventSessionDescription").text($("#EventNameList").val());

        var releaseEnc = encodeURIComponent($("#ReleaseNameList").val());
        var eventNameEnc = encodeURIComponent($("#EventNameList").val());

        $.getJSON("../descsearch/" + releaseEnc + "/" + eventNameEnc, function (data) {
            $("#EventSessionDescription").text(data.eventDescription);
        });
    });

});