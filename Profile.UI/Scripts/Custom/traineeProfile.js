$(document).ready(function () {

    var leftSidebar = $('#leftSidebar');

    leftSidebar.hide();

    // Set handler for Left Sidebar
    $('#toggleSidebar').on('click', function () {
        leftSidebar.toggle();
    });

    //Set tooltip
    $('[data-toggle="tooltip"]').tooltip();

    // Call print
    $("#callPrint").on("click", function (e) {
        e.preventDefault();
        window.print();
    });

});