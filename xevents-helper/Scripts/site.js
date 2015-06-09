$(document).ready(function () {

    $("#ReleaseNameList").change(function () {
        var releaseEnc = encodeURIComponent($(this).val());

        $.getJSON("../relevents/" + releaseEnc, function (data) {
            $("#EventNameList").find("option").remove();
            $("#EventSessionDescription").text("");

            var i;
            for (i = 0; i < data.length; i++) {
                $("#EventNameList")
                    .append($("<option></option>")
                    .attr("value", data[i].Name)
                    .text(data[i].Name));
            }
        });
    });

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