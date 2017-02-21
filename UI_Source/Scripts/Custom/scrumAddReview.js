$(document).ready(function () {
    
    //mask for textarea

    mask(".reviewItem:not(.filled)", (/[~@#$%^\]\[&*=|/}{]+/ig),800);

    // comment box events

    $(".filled").on("click", function () {
        $(this).nextAll(".fullComment").slideToggle(200);

    }).on("mouseenter", function () {
        $(this).nextAll(".fullComment").slideDown(200);

    }).on("mouseleave", function () {
        $(this).nextAll(".fullComment").slideUp(200);
    });

    // location navigation

    window.location.hash = "";
    $("textarea:not(.filled)").val("");

    function locationHandler() {

        window.onhashchange = function () {
            var hash = window.location.hash

            function backButton() {
                window.history.back();
            }

            function backHash() {
                window.history.forward();
                $("#modalScrum").modal("hide");
            }

            if (!hash) {

                var fields = $(".reviewItem:not(.filled)").each(function (ind, it) {

                    if ($(this).val()) {
                        popup("Несохраненные данные будут утеряны. Продолжить ?", backButton, backHash);
                    }
                    else {

                        $("[href]:not(link)").each(function (ind, item) {
                            $(this).attr("href", $(this).attr("data-href"));
                        });
                    }
                })
            }
            else if (hash == "#change") {

                $("[href]:not(link)").each(function (ind, item) {
                    $(this).attr("data-href", $(this).attr("href")).attr("href", "#" + $(this).attr("href"));
                });

                $("[href]:not(link)").on("click", function (e) {
                    e.preventDefault();
                    popup("Несохраненные данные будут утеряны. Продолжить ?", exitPage, backHash);

                    function exitPage() {
                        if ($(e.target).attr("href")) {
                            location.href = location.origin + $(e.target).attr("data-href");
                        }
                        else {
                            location.href = location.origin + $(e.target).closest("a").attr("data-href");
                        }
                    }
                });
            }
        }
    }

    // validation

    function validation(item, reqMess) {

        if (item.val()) {

            $(item).removeClass("errorField").closest("td").find(".error").remove();
            return true
        }
        else {
            if ($(item).hasClass("errorField")) {
                return false;
            }
            else {
                $(item).addClass("errorField").closest("td").find(".error").remove().end().closest("td").append("<span class='error'>" + reqMess + "</span>");
                $(item).on("keyup change", function () {

                    validation(item, reqMess);
                })
            }
        }
    }

    // ajax send

    function post(calb, dat) {

        $.ajax({
            type: "POST",
            url: "/ScrumMaster/PostReview/",
            data: dat,
            contentType: 'application/json; charset=utf-8',
            success: function () {

                calb();
            }
        })
    }

    $("select").each(function (ind, it) {

        $(this).val($(this).find("option:selected").text());
    })

    // save and send data event

    $(".saveReview").on("click", function () {

        //var err = $(".reviewItemWrapper").next(".error");

        //if (!err.length) {

            var self = this;
            var val = validation($(this).closest("tr").find(".reviewItem"), "Заполните поле");
            var mark = validation($(this).closest("tr").find("select"), "Поставьте оценку");

            if (val && mark) {

                popup("Вы не сможете редактировать отзыв после сохранения. Сохранить ?", send, hide);

                // cancel from modal

                function hide() {

                    $("#modalScrum").modal("hide");
                    $(self).closest("tr").find(".reviewItem").focus();
                }

                // create and send json data

                function send() {

                    var data = {};
                    data.id = $(self).attr("data-id");
                    data.review = $(self).closest("tr").find(".reviewItem").val();
                    data.mark = $(self).closest("tr").find("select").val();
                    data.scrumId = $("h1").attr("data-scrumId");
                    data = JSON.stringify(data);
                    //post(callback, data);

                    callback();

                    function callback() {

                        $("#modalScrum").modal("hide");
                        $(self).closest("tr").find(".reviewItem").off().addClass("filled").attr("readonly", "readonly").end()
						.find("select").closest("td").append("<span class='mark'>" + $(self).closest("tr").find("select").find("option:selected").text() + "</span>").end().remove();
                        $(self).after("<i class='fa fa-check fa-2x' aria-hidden='true'></i>").remove();
                        $("[href]:not(link)").off().each(function (ind, item) {

                            $(this).attr("href", $(this).attr("data-href"));
                        });
                        window.location.hash = "";
                    }
                }
            }
        //}
    })

    // open textarea for filling review

    $(".reviewItem:not(.filled)").on("focusin", function () {

        $(this).addClass("edited");

    }).on("focusout", function () {

        $(this).removeClass("edited").scrollTop(0);

    }).on("keyup change", function () {

        if ($(this).val()) {

            if (window.location.hash != "#change") {
                window.location.hash = "#change";
            }
        }
        else {
            if (window.location.hash == "#change") {
                window.location.hash = "";
            }
        }
    });

    $("select").on("click change", function () {

        $(this).children("option[value='']").remove();
        window.location.hash = "#change";
    });

    // show popups

    function popup(message, saveButton, noButton) {

        $("#modalScrum").modal("show").find(".modal-body").text(message).end().find("#saveChangesModal").off().on("click", function () {
            saveButton();
        }).siblings("#notSaveChanges").off().on("click", function () {
            noButton();
        });
    }

    locationHandler();
})