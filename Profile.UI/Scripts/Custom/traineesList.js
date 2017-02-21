$(document).ready(function () {

    var filter = decodeURIComponent(window.location.search);
    filter = filter.replace('?filter=', '');

    $('span').wrapInner(function () {
        if ($(this).text() == "Не допущен")
            return "<b></b>";
        else
            return "";
    });

    $('#prfMainTable').DataTable({
        scrollY: '60vh',
        scrollCollapse: true,
        paging: true,
        "sScrollX": "100%",
        "bScrollCollapse": true,
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
        "search": {
            "search": filter
        }
    });

    var data_table = $('#prfMainTable').DataTable();
    $('.dataTables_filter label input').addClass(function () {
        return "form-control";
    }).attr("placeholder", "Поиск");

    //to correct table with filter from url
    $(window).resize();

    $('#prfMainTable_wrapper').on('change', 'input[type="checkbox"]', function () {
        if ($(this).is(":checked")) {
            $(this).next('span').text('Допущен');
            data_table.draw();
        }
        else {
            $(this).next('span').html('<b>Не допущен</b>');
            data_table.draw();
        }
    }).on('click', '.checkAdmitted', function (e) {
        if ($(this).is(':hover')) {

            if (e.target.tagName === 'TD') {
                $(this).find('input').click();
            }
        }
    });


    function ErrorHandler(jqXHR, StatusStr, ErrorStr) {
        alert(StatusStr + ' ' + ErrorStr);
    }


    saveOnloadState();

    oldDataTableTraineeAjax = dataTableTraineeAjax;

    /*$(document).on('click', ".prf-submit-dataTable", function () {

        var compareA = [];
        saveOnloadState();

        for (i = 0; i < oldDataTableTraineeAjax.length; i++) {
            for (j = 0; j < dataTableTraineeAjax.length; j++) {
                if (oldDataTableTraineeAjax[i].id === dataTableTraineeAjax[j].id) {
                    if (oldDataTableTraineeAjax[i].IsAllowed !== dataTableTraineeAjax[j].IsAllowed) {
                        compareA.push(dataTableTraineeAjax[j]);
                    }
                }
            }
        }

        $.ajax({
            method: 'POST',
            async: true,
            url: '/Trainee/List',
            data: JSON.stringify(dataTableTraineeAjax),
            success: function (data) {
                if (data.length > 0) {
                    $('#prfMainTable').html(data);
                    oldDataTableTraineeAjax = dataTableTraineeAjax;
                } else {
                    console.log("Empty data")
                }
            },
            error: ErrorHandler
        });
    });*/

});

var dataTableTraineeAjax = [];
var oldDataTableTraineeAjax = [];

function saveOnloadState() {
    dataTableTraineeAjax = [];
    $('.prf-main-table tbody tr').each(function () {
        var attrId = $(this).attr('id');
        var attrChecked;
        ($(this).find("input").is(":checked")) ? attrChecked = true : attrChecked = false;
        dataTableTraineeAjax.push({"id": attrId, "IsAllowed": attrChecked});
    });
}