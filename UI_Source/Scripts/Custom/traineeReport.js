$(document).ready(function () {
    ShowInfoIfIssuesTableEmpty();

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
            },
            error: function () {
                $(".prf-report-modal-load-wrap").css("display", "none");
                $(".prf-report-modal-save-error").css("display", "table-cell");
            },
            beforeSend: function () { },
            complete: function () {
                $(".prf-report-modal-load-wrap").css("display", "none");
                ShowInfoIfIssuesTableEmpty();
            }
        });
    })

    $("#prfSubmitSave").on("click", function () {
        $(".prf-report-modal-save-ask").css("display", "table-cell");
    })

    $(".prf-report-modal-save-ask a").first().on("click", function () {
        $.ajax({
            url: $(this).data('action') + inputId,
            method: 'GET',
            async: true,
            contentType: 'application/json',
            success: function (data) {
                $(".prf-report-modal-save-ask").css("display", "none");
                $(".prf-report-modal-save-complete").css("display", "table-cell");
            },
            error: function () {
                $(".prf-report-modal-save-ask").css("display", "none");
                $(".prf-report-modal-save-error").css("display", "table-cell");
            },
            beforeSend: function () { },
            complete: function () {
            }
        });
    })
    $(".prf-report-modal-save-ask a").last().on("click", function () {
        $(".prf-report-modal-save-ask").css("display", "none");
    })

    $(".prf-report-modal-save-error a").on("click", function () {
        $(".prf-report-modal-save-error").css("display", "none");
    })

    function ShowInfoIfIssuesTableEmpty() {
        if ($.trim($(".prf-report-table").text()).length == 0) {
            $(".prf-report-body-text-table-body-load").css("display", "block");
        }
        else {
            $(".prf-report-body-text-table-body-load").css("display", "none");
        }
    }
})
