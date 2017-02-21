function getData(link, callbackSuccess, callbackComplete, callbackError) {

    $.ajax({
        type: "GET",
        url: link,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            setTimeout(function () { // not for implement

                if (typeof (callbackSuccess) == "function") {

                    callbackSuccess(data);
                }
            }, 1000) // not for implement


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

function sendData(link, dat, callbackSuccess, callbackComplete, callbackError) {

    $.ajax({
        type: "POST",
        data: dat,
        url: link,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            setTimeout(function () {  // not for implement

                if (typeof (callbackSuccess) == "function") {

                    callbackSuccess(data);
                }

            }, 1000)    // not for implement
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

function projectTable(container) {

    function locate() {

        window.onhashchange = function () {

            loadTable();
        }
    }

    function loadTable() {

        if (location.hash && location.hash != "#") {

            var hash = location.hash.split("=")[1];
            getTeam(hash, viewTable);
        }
        else if (location.hash == "#" || !location.hash) {

            clearTable();
            $(container + " .projects .projectListItem").removeClass("active").find("ul").removeClass("in");
        }
    }

    function clearTable() {

        $(container + " .teamTable").hide().find("tbody").empty();
    }

    function viewTable(team) {

        var rows = team.Trainee.map(function (it, ind) {

            return (
                '<tr><td class="col-xs-1"><span>' + ++ind + '</span></td><td class="col-xs-4"><a href="/Profile/View/' + it.Id + '">' + it.FullName + '</a></td><td class="col-xs-1 col-sm-2"><span>' + it.Specialization + '</span></td><td class="col-xs-5"><a href="#">' + it.MentorName + '</span></td></tr>'
            )
        });

        var table = $(container + " .teamTable");
        table.find("#scrumMaster").text(team.ScrumName);
        table.find(".crumps").children(".project").text(team.ProjectName).siblings(".team").text(team.TeamName);
        table.fadeIn().find("tbody").empty().append(rows);
    }

    function getTeam(url, callb) {

        getData("../../Scripts/Custom/jsonTest/" + url + ".json", callb);
        $(container + " .navigation").find("[href='#id=" + url + "']").closest("li").addClass("active").siblings().removeClass("active").end().closest("ul").addClass("in").closest(".projectListItem").addClass("active").siblings().removeClass("active").find("ul").removeClass("in");
    }

    function highlighted() {

        $(container + " .navigation").on("click", ".projectListItem .panel-title a", function () {

            var checkClass = $(this).closest("li").hasClass("active");

            if (checkClass) {
                $(this).closest("li").removeClass("active");
                location.hash = "";
            }
            else {
                $(this).closest("li").addClass("active").siblings().removeClass("active").find("ul").removeClass("in");
                location.hash = $(this).attr("data-hash");
            }
        });
    }

    function deleteProject() {

        $(container + " .removeTeam").on("click", function () {

            var modal = $("#deleteTeam");
            var teamId = location.hash.split("=")[1];
            modal.modal("show");
            modal.find(".saveModal").on("click", function () {

                var teamId = location.hash.split("=")[1];

                // dataMenu - menulayout after deleting project
                //getData("URL/FORDELETETEAM&id=" + teamId, , function (dataMenu) {           ----

                //    modal.modal("hide");
                //    location.hash = "";
                //    $(container + " .projects").empty().append(dataMenu);
                //});                                                            ---- FUNCTION FOR IMPLEMENT            
                modal.modal("hide"); // DELETE IT WITH IMPLEMENT
                location.hash = "";  // DELETE IT WITH IMPLEMENT
                getData("projectmenu.html", function (dataMenu) {

                    $(container + " .projects").empty().append(dataMenu);
                })
            })
        })
    }

    return ({
        init: function () {
            // visible changing events 
            highlighted();
            // events
            loadTable();
            locate();
            deleteProject();

        },
        // dataMenu = taken project layout after posting new project

        addProject: function (dataMenu) {

            $(container + " .projects").empty().append(dataMenu);

        },
        openPopup: function () {

            $(container).on("click", ".openPopup", function (e) {

                var popClass = $(e.target).attr("data-popupClass");
                var teamId = location.hash.split("=")[1];
                var projId = $(this).attr("data-prId");

                switch (popClass) {

                    case "addProj":

                        // simple url for adding new project

                        getData("popup.html", function (d) {

                            $(container).append(d);
                            $(".popupManager").show().addClass(popClass);

                            var addProject = addPopup("." + popClass, "../../Scripts/Custom/jsonTest/", "LINK_FOR_POST", "projectmenu.html").init();
                        })

                        break

                    case "addTeam":

                        // project id for url -- projId 
                        // link for GET looks like - "Manager/url&id=" + projId

                        getData("popuphidden.html", function (d) {

                            $(container).append(d);
                            $(".popupManager").show().addClass(popClass).attr("data-prId", projId);

                            var addTeam = addPopup("." + popClass, "../../Scripts/Custom/jsonTest/", "LINK_FOR_POST", "projectmenu.html");
                            addTeam.init();
                        })

                        break

                    case "editTeam":

                        // team id for url -- teamId
                        // link for GET looks like - "Manager/url&id=" + teamId

                        getData("popupFilled.html", function (d) {

                            $(container).append(d);
                            $(".popupManager").show().addClass(popClass);

                            var editTeam = addPopup("." + popClass, "../../Scripts/Custom/jsonTest/", "LINK_FOR_POST", "projectmenu.html");
                            editTeam.init();
                            editTeam.edit();

                        })

                        break
                }
            });
        }
    })
}

$(document).ready(function () {

    var ProjectsList = projectTable(".manageProject");
    ProjectsList.init();
    ProjectsList.openPopup();
})





