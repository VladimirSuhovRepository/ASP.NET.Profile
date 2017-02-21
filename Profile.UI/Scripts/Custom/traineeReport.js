$(document).ready(function () {
    $("#prfSubmitSave").attr("disabled", true);
    ShowInfoIfIssuesTableEmpty();
    RemoveButtonsIfReportExist();
    ConvertTableToJiraTimeFormat();


    $("#prfSubmitLoad").on("click", function () {
        $(".prf-report-modal-load-wrap").css("display", "table-cell");
        var inputId = $(".prf-questionary-up").attr("traineeId");

        $.ajax({
            url: $(this).data('action') + inputId,
            method: 'GET',
            async: true,
            contentType: 'application/json',
            success: function (data) {
                $(".prf-report-table").html(data);
                ConvertTableToJiraTimeFormat();
            },
            error: function () {
                $(".prf-report-modal-load-wrap").css("display", "none");
                $(".prf-report-modal-save-error").css("display", "table-cell");
            },
            beforeSend: function () { },
            complete: function () {
                $(".prf-report-modal-load-wrap").css("display", "none");
                ShowInfoIfIssuesTableEmpty();
                $("#prfSubmitSave").attr("disabled", false);
            }
        });
    })

    $("#prfSubmitSave").on("click", function () {
        $(".prf-report-modal-save-ask").css("display", "table-cell");
        $("#prfSubmitSave").attr("disabled", true);
        $("#prfSubmitLoad").attr("disabled", true);
    })

    $(".prf-report-modal-save-ask a").first().on("click", function () {
        var inputId = $(".prf-questionary-up").attr("traineeId");;
        $.ajax({
            url: "/Report/Save/" + inputId,
            method: 'POST',
            async: true,
            success: function (data) {
                $(".prf-report-modal-save-ask").css("display", "none");
                $(".prf-report-modal-save-complete").css("display", "table-cell");
                RemoveButtonsIfReportExist();
            },
            error: function () {
                $(".prf-report-modal-save-ask").css("display", "none");
                $(".prf-report-modal-save-error").css("display", "table-cell");
                $("#prfSubmitSave").attr("disabled", false);
                $("#prfSubmitLoad").attr("disabled", false);
            },
            beforeSend: function () { },
            complete: function () {
            }
        });
    })

    $(".prf-report-modal-save-ask a").last().on("click", function () {
        $(".prf-report-modal-save-ask").css("display", "none");
        $("#prfSubmitSave").attr("disabled", false);
        $("#prfSubmitLoad").attr("disabled", false);
    })

    $(".prf-report-modal-save-error a").on("click", function () {
        $(".prf-report-modal-save-error").css("display", "none");
    })

    $(".prf-report-modal-save-complete a").on("click", function () {
        $(".prf-report-modal-save-complete").css("display", "none");
    })

    function RemoveButtonsIfReportExist() {
        if (IsReportExists()) {
            $("#prfSubmitSave").remove();
            $("#prfSubmitLoad").remove();
        }
    }

    function ShowInfoIfIssuesTableEmpty() {
        if (!IsReportExists()) {
            $(".prf-report-body-text-table-body-load").css("display", "block");
        }
        else {
            $(".prf-report-body-text-table-body-load").css("display", "none");
        }
    }

    function IsReportExists() {
        return $.trim($(".prf-report-table").text()).length != 0;
    }

    function ConvertTableToJiraTimeFormat() {
        $('.jira-time').each(function () {
            var time = Number($(this).text());
            $(this).text(ConvertValueToJiraTimeFormat(time));
        });
    }

    function integerDivision(x, y) {
        return (x - x % y) / y
    }

    function ConvertValueToJiraTimeFormat(seconds) {
        var week = 144000;
        var day = 28800;
        var hour = 3600;
        var minute = 60;
        var weeks = 0;
        var days = 0;
        var hours = 0;
        var minutes = 0;
        var jiraTime = "";
        
        weeks = integerDivision(seconds, week);
        if (weeks > 0)
        {
            seconds -= weeks * week;
            jiraTime += String(weeks) + "w ";
        }

        days = integerDivision(seconds, day);
        if (days > 0)
        {
            seconds -= days * day;
            jiraTime += String(days) + "d ";
        }

        hours = integerDivision(seconds, hour);
        if (hours > 0)
        {
            seconds -= hours * hour;
            jiraTime += String(hours) + "h ";
        }

        minutes = integerDivision(seconds, minute);
        if (minutes > 0)
        {
            seconds -= minutes * minute;
            jiraTime += String(minutes) + "m";
        }
        
        return jiraTime.trim().length === 0 ? "-" : jiraTime.trim();
    }
})
