function setRoles(container) {

    function sendData(link, dat, callbackSuccess, callbackComplete, callbackError) {

        $.ajax({
            type: "POST",
            data: dat,
            url: link,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (typeof (callbackSuccess) == "function") {

                    callbackSuccess(data);
                }
            },
            error: function (data, err, mess) {

                console.log("Error: " + err + ", message: " + mess);

                if (typeof (callbackError) == "function") {

                    callbackError();
                }
            },
            beforeSend: function () {

                if (typeof (callbackComplete) == "function") {

                    callbackComplete();
                }
            }
        })
    }

    function showButtons() {

        $(container + " .checkInput").on("change", function () {

            $(container + " .buttonsSets .container-fluid").show();

            if (!checkTrainees("value").length) {

                hideButtons();
            }
        })
    }

    function hideButtons() {

        $(container + " .buttonsSets  .container-fluid").hide().find("input").prop("checked", false);
        $(container + " #ready").addClass("disabled");
        $(container + " .specButtons .checkButton").addClass("disabled").prev("input").attr("disabled", "disabled");
    }

    function showDisabled() {

        $(container + " .checkButton").prev("input").on("change", function () {

            var chRole = checkButtons(".roleButtons");
            var chSpec = checkButtons(".specButtons");
            var nospec = $(this).next(".checkButton").hasClass("nospec");

            if (chRole && !chSpec) {

                $(container + " .specButtons .checkButton").removeClass("disabled").prev("input").removeAttr("disabled");
                $(container + " #ready").addClass("disabled");

                if (nospec) {
                    $(container + " .specButtons .checkButton").addClass("disabled").prev("input").attr("disabled", "disabled").each(function (ind, it) {

                        $(this).prop("checked", false);
                    });
                    $(container + " #ready").removeClass("disabled");
                }
            }
            else if ((chRole && chSpec)) {

                if (nospec) {
                    $(container + " .specButtons .checkButton").addClass("disabled").prev("input").attr("disabled", "disabled").each(function (ind, it) {

                        $(this).prop("checked", false);
                    });
                }

                $(container + " #ready").removeClass("disabled");
            }
        })
    }

    function checkTrainees(attribute) {

        var checkedTrainees = [];

        $(container + " .checkInput").each(function (ind, it) {

            if ($(this).prop("checked")) {

                checkedTrainees.push($(this).attr(attribute));
            }
        })

        return checkedTrainees;
    }

    function checkButtons(buttonContainer) {

        var value;

        $(container + " " + buttonContainer + " .checkButton").prev("input").each(function (ind, it) {

            if ($(this).prop("checked")) {

                value = $(this).val();
            }
        })

        return value
    }

    function deleteTrainee() {

        $(container + " .delete span").on("click", function () {
            
            $(container + " #deleteTrainee").modal("show");
        })
    }

    function sendCallback(modalId, mass) {

        $(container + " " + modalId).modal("hide").find(".reloader").remove();
        hideButtons()
        mass.map(function (it, ind) {

            $("[value='" + it + "']").closest("tr").remove();
        })
    }

    function cancelTrainee() {

        $(container + " #cancel").on("click", function () {

            hideButtons();
            $(".checkInput").prop("checked", false);
        })
    }

    function pickData() {

        var data = {};

        data.Users = checkTrainees("value");
        data.Role = $(container + " .roleButtons input:checked").val();
        data.Spec = $(container + " .specButtons input:checked").val();

        return data;
    }

    function assignToRole() {

        $(container).on("click", "#ready:not(.disabled)", function () {
            
            var list = checkTrainees("data-name").map(function (it, ind) {

                return "<li><span>" + it + "<span></li>"
            });

            $(container + " #saveTrainee").modal("show");
            $(container + " #saveTrainee")
                .find(".roleSave").text($(container + " .roleButtons input:checked").attr("data-roleName"))
                .next(".specSave").text($(container + " .specButtons input:checked").attr("data-specName"))
                .closest(".modal-body").find(".listSave").append(list);

            $(container + " #saveTrainee").on("hide.bs.modal", function () {

                $(this).find(".modal-body").find("span").empty().end().find("ul").empty();
            })
        })
    }

    function saveData() {

        $(container + " #saveRole:not(.disabled)").on("click", function () {

            var self = this;
            var traineeRoles = pickData(); // trainee data with spec and role

            $(this).addClass("disabled");

            // ----- FOR IMPLEMENT ---

            //sendData(URL, JSON.stringify(traineeRoles), function (d) {  // URL - url for saving trainees roles and spec

            //    sendCallback("#saveTrainee", traineeRoles.Users);
            //    $("#saveOk").modal("show");
                //$(self).removeClass("disabled");

            //})

            // ----- FOR IMPLEMENT ---

            // ---- NOT FOR IMPLEMENT
            setTimeout(function () {

                sendCallback("#saveTrainee", traineeRoles.Trainees);
                $("#saveOk").modal("show");
                $(self).removeClass("disabled");

            }, 2000)
            // ---- NOT FOR IMPLEMENT
        })
    }

    function deleteData() {

        $(container + " #deleteTrainee #saveDeleting:not(.disabled)").on("click", function () {

            var self = this;
            var deletedTrainees = checkTrainees("value"); // array of trainees ID
            $(this).addClass("disabled");

            // ---- FOR IMPLEMENT -----

            //deletedTrainees = JSON.stringify(deletedTrainees);

            //sendData(URL, deletedTrainees, function (d) { // URL - link for deleting post

            //    sendCallback("#deleteTrainee", deletedTrainees);
            //    $(self).removeClass("disabled");
            //});

            // ---- FOR IMPLEMENT -----

            // ---- NOT FOR IMPLEMENT
            setTimeout(function () {

                sendCallback("#deleteTrainee", deletedTrainees); 
                $(self).removeClass("disabled");

            }, 2000)
            // ---- NOT FOR IMPLEMENT
        })
    }

    return ({
        init: function () {

            showButtons();
            showDisabled();
            deleteTrainee();
            cancelTrainee();
            assignToRole();
            saveData();
            deleteData();
        }
    })
}