function cabinetView(container) {

    function location() {

        if (window.location.hash == "#modalView") {

            $(container + " #confirm").modal("show");
            window.location.hash = "";
        }
    }

    return ({
        init: function () {

            location();
        }
    })
}

$(document).ready(function () {

    var CabinetView = cabinetView(".cabinet").init();
})

