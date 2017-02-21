$(document).ready(function(){

  $(".panel").mouseup(function(e){
    $(e.target).parents(".panel-title").find(".arrowCollapsed").addClass("hideArrow");
    $(e.target).parents(".panel-title").find(".arrowExpaned").removeClass("hideArrow");
});

  $(".panel-group").mouseup(function (e) {
      panelCollapsed = $(e.target).parents(".panel").find(".collapse").hasClass("in");
      if (panelCollapsed) {
          $(e.target).parents(".panel-title").find(".arrowCollapsed").removeClass("hideArrow");
          $(e.target).parents(".panel-title").find(".arrowExpaned").addClass("hideArrow");
      } else {
          $(".arrowCollapsed").removeClass("hideArrow");
          $(".arrowExpaned").addClass("hideArrow");
          $(e.target).parents(".panel-title").find(".arrowCollapsed").addClass("hideArrow");
          $(e.target).parents(".panel-title").find(".arrowExpaned").removeClass("hideArrow");
      }
  });

  $('.closeEditModal').click(function () {
      location.reload();
  });

    var form = null;

  mask(".PrjShort", (/[\\~@#$%^<>\]\[&*=|/}{]+/ig),150);
  mask(".PrjFull", (/[\\~@#$%^<>\]\[&*=|/}{]+/ig), 1000);
  mask(".TeamTask", (/[\\~@#$%^<>\]\[&*=|/}{]+/ig), 50);
  mask(".TeamWork", (/[\\~@#$%^<>\]\[&*=|/}{]+/ig), 600);

  $("#editProjectForm").validate({
      submitHandler: function (editproject) {
          console.log("validation ok");
          form = editproject;
          $('#readyConfirm').removeClass('hide');
          $('#readyConfirm').modal('show');
      }
  });

  $('#saveButton').click(function () {
      $('.textField').each(function () {
          $(this).rules('add', {
              required: true,
              messages: {
                  required: "Это обязательное поле",
              }
          });
      });
  });

    $("#cancelButton").click(function () {
          $('#closeConfirm').removeClass('hide');
          $('#closeConfirm').modal('show');
    });    


    $(".confirm_modal_btn").click(function () {  //Сохранить изменения? НЕТ
        $('#readyConfirm').addClass('hide');
    });

    $(".submit_btn").click(function () {      // Сохранить изменения? ДА
        if ($("#editProjectForm").valid()) {
            RenewProject();
            window.location.reload();
        }
    });

    $(".submit_btn_edit").click(function () {      // Сохранить изменения? ДА
        if ($("#editProjectForm").valid()) {
            console.log("validation ok");
            form.submit();
        }
    });

    $(".close_btn_modal").click(function () {  //Выйти без изменений? НЕТ
        $('#closeConfirm').addClass('hide');
    })

    $(".close_modal_btn").click(function () {  //Выйти без изменений? ДА
      $('#closeConfirm').addClass('hide');
      $('#closeConfirm').modal('hide');
      $('#readyConfirm').modal('hide');
        HideReviewForm();
    });

    function HideReviewForm() {
        // $('#projectEditModal').addClass('hide')
        $('#projectEditModal').modal('hide');
    };

    function RenewProject() {
        console.log("Ajax sent")
        var data = {};
        //projectId = "projectId887"; //для проверки локально, потом закомментировать
        projectId = $('input[name="ProjectId"]').val(); // раскомментировать. Или не ProjectId, но что-то по чему идентифицируется проект
        data.project = projectId;
        data.projectShortDesc = $('textarea[name="projectShortDescription"]').val();
        data.projectFullDesc = $('textarea[name="pojectFullDescription"]').val();
        data.teamTask = $('textarea[name="teamTask"]').val();
        data.teamWorkDesription = $('textarea[name="teamWorkDesription"]').val();
        var json = JSON.stringify(data);
        console.log(json);
        //$('#readyConfirm').addClass('hide'); //для проверки локально, потом закомментировать
        //$('#readyConfirm').modal('hide'); //повторяет строки внутри success
        $.ajax({
            type: 'POST',
            url: '/ScrumMaster/EditProject', // изменить ссылку
            data: json,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                // PLACE FOR CALLBACK
                    console.log("ok Ajax");
                    $('#readyConfirm').addClass('hide');
                    $('#readyConfirm').modal('hide');
                    location.reload(); // заменить ссылку (move to the project page that we’ve just created)

            },
            error: function (data, error, mess) {

                console.log("error: " + data + " " + error + " " + mess);
            }
        });
    }; 


});