function editRoles(container) {

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

    function getData(link, callbackSuccess, callbackComplete, callbackError) {

        $.ajax({
            type: "GET",
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

    function chooseNewRole() {

        $(container + " .newRole").on("change", function (e) {

            var option = $(this).find("option:selected").hasClass("specialization");
            $(container + " #clear").removeClass("disabled");
            $(container + " #sendNewRole").addClass("disabled");
            $(container + " .newSpec").attr("disabled", "disabled").find(".default")[0].selected = true;

            if (option) {

                $(container + " .newSpec").removeAttr("disabled");

            }
            else {
                $(container + " .newSpec").attr("disabled", "disabled");
                $(container + " #sendNewRole").removeClass("disabled");
            }
        })
    }

    function chooseAccount() {

        $(container + " .currentAcc").on("change", function () {

            $(container + " .currentRoleBlock button").removeClass("disabled");
        })
    }

    function chooseNewSpec() {

        $(container + " .newSpec").on("change", function (e) {

            $(container + " #sendNewRole").removeClass("disabled");
        })
    }

    function chooseCurrentRole() {

        $(container + " .currentRole").on("change", function () {

            var spec = $(this).val();
            $(container + " .currentRoleBlock button").addClass("disabled");

            // spec - specialization value (<option value="BA">)

            //getData("URL&id=" + spec, addAccounts); ---- FOR IMPLEMENT

            getData("../../Scripts/Custom/jsonTest/accounts.json", addAccounts); // --- NOT FOR IMPLEMENT
        });
    }

    function clearNewRole() {

        $(container).on("click", "#clear:not(.disabled)", function () {

            clear();
        })
    }

    function clear() {

        $(container + " .newRoleBlock").addClass("disabled").find("button").addClass("disabled").end().find("select").attr("disabled", "disabled").each(function (ind, it) {

            $(this).find(".default")[0].selected = true;
        })
    }

    function changeRole() {

        $(container).on("click", "#changeRole:not(.disabled)", function () {

            $(container + " .newRoleBlock").removeClass("disabled").find(".newRole").removeAttr("disabled");
        });
    }

    function saveNewRole() {

        $(container).on("click", "#sendNewRole", function () {

            var newData = pickData();

            //sendData("URL", newData, function () {  ---- FOR IMPLEMENT

            //    $(container + " #saveModal").modal("show");
            //    clearCurrent();
            //    clear();
            //})                                     ---- FOR IMPLEMENT

            $(container + " #saveModal").modal("show");
            clearCurrent();  // NOT FOR IMPLEMENT
            clear();  // NOT FOR IMPLENEMNT
        })
    }

    function deleteAcc() {

        $(container).on("click", "#deleteRole:not(.disabled)", function () {

            var dataDelete = pickData();

            $(container + " #deleteModal").modal("show");

            $(container + " #deleteModal .saveModal").on("click", function () {

                //sendData("URL", dataDelete, clearCurrent); --- FOR IMPLEMENT

                $(container + " #deleteModal").modal("hide");
                clearCurrent()     // --- NOT FOR IMPLEMENT
            })

        });
    }

    function pickData() {

        var dataAccount = {};

        dataAccount.currentRole = $(container + " .currentRole").val();
        dataAccount.id = $(container + " .currentAcc").val();
        dataAccount.newRole = $(container + " .newRole").val();
        dataAccount.newSpec = $(container + " .newSpec").val();

        return JSON.stringify(dataAccount);
    }

    function clearCurrent(dat) {

        $(container + " .currentAcc").find("option:not(.default)").remove().end().find(".default")[0].selected = true;
        $(container + " .currentRole").find(".default")[0].selected = true;
        $(container + " .currentRoleBlock button").addClass("disabled");
    }

    function addAccounts(dat) {

        var accounts = dat.map(function (it, ind) {

            return ("<option value='" + it.id + "'>" + it.name + "</option>");
        });

        clear();
        $(container + " .currentAcc").find("option:not(.default)").remove().end().append(accounts);
    }

    return ({
        init: function () {

            changeRole();
            chooseNewRole();
            chooseNewSpec();
            clearNewRole();
            deleteAcc();
            chooseAccount();
            chooseCurrentRole();
            saveNewRole();
        }
    })
}

