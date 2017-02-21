function mainRoles() {

    function rightTabs() {

        $(".rights .rightItem").on("click", function () {

            $(this).addClass("active").closest("li").siblings().find(".rightItem").removeClass("active");
        })
    }

    function tabNavigation() {

        function hashView() {

            var hash = location.hash.substring(1);

            if (hash) {
                $(".classTab").hide();
                $("." + hash).show();
                $("[href='#" + hash + "']").closest(".rightItem").addClass("active").closest("li").siblings().find(".rightItem").removeClass("active");
            }
            else {
                location.hash = "#set"
            }
        }
        hashView()

        window.onhashchange = function () {

            hashView();
        }
    }

    return ({
        init: function () {

            var set = setRoles(".set").init();
            var edit = editRoles(".edit").init();

            rightTabs();
            tabNavigation();
        }
    })
}

$(document).ready(function () {

    var Rights = mainRoles().init();
})


