$(document).ready(function(){

    var filter = decodeURIComponent(window.location.search);
    filter = filter.replace('?filter=', '')


    var table = $("#example").DataTable({

        "searching":true,
        "ordering": false,
        "paging": false,
        "bLengthChange": false,
        //"pageLength": 12,
        "language": {
            // "info": "Найдено _START_ в _END_ из _TOTAL_ записей",
            // "info": "Показано с _START_ по _END_ из _TOTAL_ записей ",
            "infoEmpty": "Ничего не найдено",
            // "infoFiltered":   "(filtered from _MAX_ total entries)",
            // "infoFiltered": "",
            "zeroRecords": "Ничего не найдено",
            // "lengthMenu": "Показано _MENU_ записей",
            "loadingRecords": "загружается...",
            "processing": "Обрабатывается...",
            "search": ""
        },
        "search": filter
    });


    ProjectTable = $('#example').DataTable();   
    $('#searchTextField').keyup(function(){
          ProjectTable.search($(this).val()).draw() ;
    });


});