$(document).ready(function(){

    //$('#example').DataTable();

    $.fn.DataTable.ext.pager.simple_numbers = function (page, pages) {
        return ["previous", _numbers(page, pages), "next"];
    };

    var table = $("#example").DataTable({

        "searching":false,
        "ordering": false,
        "bLengthChange": false,
        "pageLength": 6,
        "language": {
            //     "info": "Найдено _START_ в _END_ из _TOTAL_ записей",
            "info": "Показано с _START_ по _END_ из _TOTAL_ записей ",
            "infoEmpty": "Ничего не найдено",
            //     "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoFiltered": "(отфильтровано _MAX_ записей)",
            "zeroRecords": "Ничего не найдено",
            "lengthMenu": "Показано _MENU_ записей",
            "loadingRecords": "загружается...",
            "processing": "Обрабатывается...",
            "search": "",
            "paginate": {
                "first": "Первая",
                "last": "Последняя",
                "next": "Следующая",
                "previous": "Предыдущая"
            }
        },
    });

    $(".dataTables_paginate").addClass("pageNavigation").find("a").addClass("paginationItem");
});

function _numbers(page, pages) {
    var numbers = [];

    if (pages <= 1)
        return numbers;

    for (var i = page - 2; i <= page + 2; i++) {
        if ((i >= 0) && (i < pages))
            numbers.push(i);
    }

    numbers.DT_el = "span";
    return numbers;
}