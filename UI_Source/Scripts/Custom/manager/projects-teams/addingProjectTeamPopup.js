// linkList = part for GET url (json for trainee and mentor list after checking specialisation)
// linkSend = POST url (change method in postData() )
// linkMenu = GET url (for getting new menu layout)

function addPopup(container, linkList, linkSend, linkMenu) {

    var popupWrapper = container;

    // for edit team popup
    function editProject() {

        $(popupWrapper + " .changeButton").click(function (e) {

            newProjectDates(e)
            $(this).closest(".disabled").removeClass("disabled");
            $(popupWrapper + " .projDate .end," + popupWrapper + " .projDate .start").addClass("datepicker-here");
            calendar();
            $(popupWrapper + " .changeName," + popupWrapper + " .projName," + popupWrapper + " .projDate").find("input").removeAttr("disabled")[0].focus();
            $(popupWrapper + " .projDate .end," + popupWrapper + " .projDate .start").removeClass("disabled").removeAttr("disabled");
        })
    }

    function newProjectDates(ev) {

        var dateProjFields = $(popupWrapper + " .projDate .start," + popupWrapper + " .projDate .end");
        var dateTeamFields = $(popupWrapper + " .teamDate .start," + popupWrapper + " .teamDate .end");
        var data = [[],[]]

        dateProjFields.each(function (ind, it) {

            var value = $(this).val();

            if (value) {

                value = value.split("-");
                data[0].push(new Date(value[2], value[1] - 1, value[0]))

                if (ev) {

                    $(this).datepicker().data("datepicker").selectDate(new Date(value[2], value[1] - 1, value[0]));
                }
            }
        })

        dateTeamFields.each(function (ind, it) {

            var value = $(this).val();

            if (value) {

                value = value.split("-");
                data[1].push(new Date(value[2], value[1] - 1, value[0]))
                $(this).datepicker().data("datepicker").selectDate(new Date(value[2], value[1] - 1, value[0]));
            }
        })
        return data
    }

    function calendar() {

        var teamDate = [];
        var projDate = [];

        var dates = newProjectDates();

        if (dates[0].length) {
            projDate = dates[0];            
        }
        else if (dates[1].length) {
            teamDate = dates[1];
        }

        function refreshDates() {

            if ($(".projDate .datepicker-here").length) {

                projDate[0] = $(popupWrapper + " .projDate .start.datepicker-here").datepicker().data("datepicker").selectedDates[0];
                projDate[1] = $(popupWrapper + " .projDate .end.datepicker-here").datepicker().data("datepicker").selectedDates[0];
            }

            teamDate[0] = $(popupWrapper + " .teamDate .start.datepicker-here").datepicker().data("datepicker").selectedDates[0];
            teamDate[1] = $(popupWrapper + " .teamDate .end.datepicker-here").datepicker().data("datepicker").selectedDates[0];
        }

        refreshDates();

        $(popupWrapper + " .datepicker-here").datepicker({
            dateFormat: "dd-mm-yyyy",
            autoClose: true,
            onShow: function (inst, callb) {

                refreshDates();

                if ($(".projDate .datepicker-here").length) {

                    $(popupWrapper + " .projDate .end.datepicker-here").datepicker().data("datepicker").update({
                        maxDate: "",
                        minDate: teamDate[1] ? teamDate[1] : (projDate[0] ? projDate[0] : teamDate[0])
                    })

                    $(popupWrapper + " .projDate .start.datepicker-here").datepicker().data("datepicker").update({
                        maxDate: teamDate[0] ? teamDate[0] : (projDate[1] ? projDate[1] : teamDate[1]),
                        minDate: ""
                    });
                }

                $(popupWrapper + " .teamDate .end.datepicker-here").datepicker().data("datepicker").update({
                    maxDate: projDate[1],
                    minDate: teamDate[0] ? teamDate[0] : projDate[0],
                })

                $(popupWrapper + " .teamDate .start.datepicker-here").datepicker().data("datepicker").update({
                    maxDate: teamDate[1] ? teamDate[1] : projDate[1],
                    minDate: projDate[0]
                })
            },
            onSelect: function (fd, date, el) {

                el.el.value = fd;
                $(el.el).removeClass("error").next(".errorText").remove();
            }
        });
    }

    function masks() {

        mask(popupWrapper + " .projName input", (/[~@#$%^\]\[&*=|/\}\{]+/ig), 20);
        mask(popupWrapper + " .teamName input", (/[~@#$%^\]\[&*=|/\}\{]+/ig), 15);
    }

    function viewListForSelect(dat, item) {

        var checkedTrainee = checkTrainee(dat[0], item);

        checkedTrainee.map(function (it, ind) {

            $(item).closest(".traineeToTeam").find("[value='" + it + "']").attr("disabled", "disabled");;
        })

        var optListTrainee = dat[0].map(function (it, ind) {

            return "<option class='mainList' value='" + it.id + "'>" + it.name + "</option>"
        });

        var optListMentor = dat[1].map(function (it, ind) {

            return "<option class='mainList' value='" + it.id + "'>" + it.name + "</option>"
        });

        $(item).next(".preload").remove();
        $(item).closest(".traineeToTeam").find(".trainee").find("select").removeAttr("disabled").find(".mainList").remove().end().append(optListTrainee).end().find(".placeholder").prop("selected", true);
        $(item).closest(".traineeToTeam").find(".mentor").find("select").find(".mainList").remove().end().attr("disabled", "disabled").find(".placeholder").prop("selected", true);

        traineeBinder(optListMentor)
    };

    function traineeBinder(options) {

        $(document).on("change", popupWrapper + " .trainee select", function (e) {

            var mentorCont = $(this).closest(".traineeToTeam").find(".mentor");
            mentorCont.find("select").removeAttr("disabled").find(".mainList").remove().end().append(options);
        })
    }

    function filledTraineeBinder() {

        $(document).on("change", popupWrapper + " .trainee select", function (e) {

            $(popupWrapper + " .traineeToTeam .trainee .mainList").removeAttr("disabled");
            $(popupWrapper + " .trainee select").each( function (ind, it) {

                var value = $(this).val();

                if (value) {

                    $(e.target).closest(".traineeToTeam").siblings(".traineeToTeam").find(".trainee").find("select").find("[value='" + value + "']:not(:selected)").attr("disabled", "disabled");

                    $(e.target).closest(".traineeToTeam").find("[value='" + value + "']:not([value='" + e.target.value + "'])").attr("disabled", "disabled")
                }
            });
        });
    }

    function deleteRow() {

        $(document).on("click", popupWrapper + " .delete", function () {

            var thisRow = $(this).closest(".traineeToTeam");
            var otherRows = thisRow.siblings(".traineeToTeam");

            $(otherRows).each(function (ind, it) {

                $(this).find(".counter").find("span").text(++ind);
            });

            thisRow.remove();
            var delButton = $(popupWrapper).find(".delete");

            if (delButton.length == 1) {

                delButton.hide();
            }
        })
    }

    function scrumBinder() {

        $(document).on("change", popupWrapper + " .scrumName select", function () {

            if ($(this).find("option:selected").hasClass("busy")) {

                $(this).addClass("error");
            }
            else {
                $(this).removeClass("error");
            }
        })
    }

    function specBinder() {

        $(document).on("change", popupWrapper + " .spec select", function () {

            var self = this;
            var spec = $(this).find("option:selected").val();

            getData(linkList + spec + "ListTest.json", function (d) {

                viewListForSelect(d, self);

            }, function () {

                preload(self);
            });
        })
    }

    function preload(select) {

        $(select).off().next('preload').remove();

        $(select).addClass("disabled").after("<img class='preload' src='../../images/loading.gif'/>");
    }

    function preloadAfter(select) {

        $(select).removeClass("disabled").next('preload').remove();
    }

    function addTraineeRow() {

        $(popupWrapper + " #addTraineeRow").on("click", function () {

            var spec = ["BE .NET", "BE ANDROID"];
            var row = $(this).closest(".addTrainee").prev(".traineeToTeam").clone();
            var counter = +(row.find(".counter").find("span").text());
            row.find(".counter").find("span").text(++counter);
            row.find(".trainee").find("select").attr("disabled", "disabled").find(".mainList").remove();
            row.find(".mentor").find("select").attr("disabled", "disabled").find(".mainList").remove();
            $(".addTrainee").before(row);
            $(popupWrapper).find(".delete").show();
        });
    }

    function validate() {

        var check = 1;

        $(popupWrapper + " .projName input," + popupWrapper + " .projDate input," + popupWrapper + " .teamName input," + popupWrapper + " .teamDate input").each(function (it, ind) {

            var self = $(this);
            var val = this.value;

            if (!val) {

                self.addClass("error").next(".errorText").remove().end().after("<span class='errorText'>Это обязательное поле</span>");
                check = 0;

                self.on("keyup change", function () {

                    var val = this.value;

                    if (!val) {

                        self.addClass("error").next(".errorText").remove().end().after("<span class='errorText'>Это обязательное поле</span>");
                    }
                    else {

                        self.removeClass("error").next(".errorText").remove();
                    }
                })
            }
        });

        return check;
    }

    function showNewTeam(d) {

        location.hash = "#";
        $("#" + popupWrapper.substr(1) + "ManagerSave").modal("show");
        preloadAfter("#save");
        hidePopup();
        getData(linkMenu, ProjectsList.addProject);  // ONLY FOR LOCAL
        //getData(linkMenu + "&id=" + d, ProjectsList.addProject);  // FOR IMPLEMENT
        location.hash = "#id=" + d;
    }

    function save() {

        $(popupWrapper + " #save:not(.disabled)").on("click", function () {

            postData();
        })
    }

    function postData() {

        var valid = validate();

        if (valid) {

            preload("#save");
            var newItem = pickData();

            newItem = JSON.stringify(newItem);

            //sendData(linkSend, newProject, showNewTeam);
            getData("../../Scripts/Custom/jsonTest/getTeamId.json", showNewTeam);
        }
        else {

            window.scrollTo(0, 0);
        }
    }

    function pickData() {

        var newItem = {};
        newItem.Team = {};
        newItem.Team.Trainees = [];

        $(popupWrapper + " .traineeToTeam").each(function (ind, it) {

            var trainee = {};

            trainee.traineeId = $(this).find(".trainee").find(".mainList:selected").val();
            trainee.mentorId = $(this).find(".mentor").find(".mainList:selected").val();
            //trainee.specId = $(this).find(".spec").find(".mainList:selected").val();

            if (trainee.traineeId) {

                newItem.Team.Trainees.push(trainee);
            }
        })

        function pick(path) {

            return $(popupWrapper + " " + path).val();
        }
        function pickDate(path) {

            //return $(popupWrapper + " " + path).datepicker().data("datepicker").$el[0].value;

            if ($(popupWrapper + " " + path).length) {

                var date = $(popupWrapper + " " + path).datepicker().data("datepicker").selectedDates[0];

                if (!date) {

                    dateMass = $(popupWrapper + " " + path).val().split("-");
                    date = new Date(dateMass[2], dateMass[1] - 1, dateMass[0])
                }

                return date.getTime() - date.getTimezoneOffset() * 60000;
            }            
        }
        
        newItem.projectStartDate = pickDate(".projDate .start");
        newItem.projectFinishDate = pickDate(".projDate .end");
        newItem.projectName = pick(".projName input");

        newItem.projectId = $(".popupManager" + popupWrapper).attr("data-prId");

        newItem.Team.teamName = pick(".teamName input");
        newItem.Team.teamStartDate = pickDate(".teamDate .start");
        newItem.Team.teamFinishDate = pickDate(".teamDate .end");
        newItem.Team.scrumId = pick(".scrumName select .mainList:selected");
        newItem.Team.teamId = $(popupWrapper).attr("data-teamId");

        return newItem;
    }

    function checkTrainee(d, it) {

        var repeatId = []

        d.map(function (item, ind) {

            var cond = $(it).closest(".traineeToTeam").siblings(".traineeToTeam");

            if (cond.length) {

                cond.each(function (indx, it) {

                    var value = $(this).find(".trainee").find(".mainList:selected").val();

                    if (item.id == value) {

                        repeatId.push(item.id);
                    }
                })
            }
        });

        return repeatId;
    }

    function exit() {

        $(document).on("click", popupWrapper + " #exit," + popupWrapper + " #back", function () {

            var modal = $("#" + popupWrapper.substr(1) + "ManagerExit");
            modal.modal("show");
            modal.find(".saveModal").on("click", function () {

                hidePopup();
                modal.modal("hide");
            })
        })
    }

    function hidePopup() {

        $(".popupManager").remove();
    }

    return ({
        init: function () {

            // plugins
            calendar();
            masks();
            //events
            specBinder();
            scrumBinder();
            filledTraineeBinder();
            addTraineeRow();
            deleteRow();
            //send and exit events
            save();
            exit();
        },
        edit: function () {

            editProject();
        }
    })
}

