$(document).ready(function(){
    $("#scrumProjectInfo").validate();

    mask(".textField", (/[~@#$%^\]\[&*=|/}{]+/ig));

    $('.textFieldRequired').each(function () {
        $(this).rules('add', {
            required: true,
            messages: {
                required: "Это обязательное поле",
            }
        });
    });

    $('#saveButton').click(function () {

        if ($("#scrumProjectInfo").valid()) {
            console.log("validation ok");
            $('#readyConfirm').removeClass('hide');
            $('#readyConfirm').modal('show');
        };

    });

    $(".close_modal").click(function () {
        $('#readyConfirm').addClass('hide');
        $('#readyConfirm').modal('hide');
    })

  //$(".submit_btn").click(function () {
  //    console.log("submit ok");
  //    AddNewProject();
  //    //$(this).attr( "disabled", "disabled" );  
  //}) 


    //function AddNewProject() {
    //    console.log("Ajax sent")
    //    var data = {};
    //    projectId = "projectId887"; //для проверки локально, потом закомментировать
    //    //projectId = $('input[name="ProjectId"]').val(); // раскомментировать. Или не ProjectId, но что-то по чему идентифицируется проект
    //    data.project = projectId;
    //    data.projectShortDesc = $('textarea[name="projectShortDescription"]').val();
    //    data.projectFullDesc = $('textarea[name="pojectFullDescription"]').val();
    //    data.teamTask = $('textarea[name="teamTask"]').val();
    //    data.teamWorkDesription = $('textarea[name="teamWorkDesription"]').val();
    //    var json = JSON.stringify(data);
    //    console.log(json);
    //    $('#readyConfirm').addClass('hide'); //для проверки локально, потом закомментировать
    //    $('#readyConfirm').modal('hide'); //повторяет строки внутри success
    //    $.ajax({
    //        type: 'POST',
    //        url: '/Mentor/Review', // изменить ссылку
    //        data: json,
    //        contentType: 'application/json; charset=utf-8',
    //        success: function (data) {
    //            // PLACE FOR CALLBACK
    //                console.log("ok Ajax");
    //                $('#readyConfirm').addClass('hide');
    //                $('#readyConfirm').modal('hide');
    //                window.location.href = "http://www.it-academy.by/" // заменить ссылку (move to the project page that we’ve just created)

    //        },
    //        error: function (data, error, mess) {

    //            console.log("error: " + data + " " + error + " " + mess);
    //        }
    //    });
    //}; 


});