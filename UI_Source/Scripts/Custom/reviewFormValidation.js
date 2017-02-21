$(document).ready(function(){

  mask(".reviewField", (/[~@#$%^\]\[&*=|/}{]+/ig),600);
  mask(".commentField", (/[~@#$%^\]\[&*=|/}{]+/ig),800);


    function PostReview() {
        console.log("Ajax sent")
        var data = {};
        data.SkillGrades = [];
        traineeId = $('input[name="TraineeId"]').val();
        data.TraineeId = traineeId;
        data.MentorId = $('input[name="MentorId"]').val();
        data.WorkComment = $('textarea[name="WorkComment"]').val();
        $(".skillgrade").each(function () {
            var SkillGrade = {};
            SkillGrade.Comment = $(this).find('textarea').val();
            SkillGrade.Value = $(this).find("select option:selected").val();
            SkillGrade.SkillId = $(this).find('input[name="SkillId"]').val();
            data.SkillGrades.push(SkillGrade);
        });
        var json = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Mentor/Review',
            data: json,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                NoteReview(traineeId);
                // PLACE FOR CALLBACK
                    console.log("ok Ajax");
                    $('#closeConfirm').modal('hide');
                    HideReviewForm();
            },
            error: function (data, error, mess) {

                console.log("error: " + data + " " + error + " " + mess);
            }
        });
    };


    function NoteReview(traineeId) {
      var el = $("tr#Trainee_" + traineeId);
      el.find('a[type="button"]').addClass("disabled");
      var newHtml = el.html() + ' <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1"><span class="checked glyphicon glyphicon-ok"></span></td>'
      el.html(newHtml);
    };

    function HideReviewForm() {
        $('#trainee1').modal('hide');
    };


  $('#saveButton').click(function () {

      $('.reviewField').each(function () {
          $(this).rules('add', {
              required: true,
              messages: {
                  required: "Оставьте комментарий к оценке",
              }
          });
      });

      $('.commentField').each(function () {
          $(this).rules('add', {
              required: true,
              messages: {
                  required: "Заполните поле",
              }
          });
      });

      $('.markSelect').each(function () {
          $(this).rules('add', {
              required: true,
              messages: {
                  required: "Поставьте оценку"
              }
          });
      });
  });

    $("#reviewForm").validate({
      submitHandler: function(reviewForm) {
        $('#readyConfirm').removeClass('hide');
        $('#readyConfirm').modal('show');  
      }
    });

  $("#cancelButton").click(function () {
      $('#closeConfirm').removeClass('hide');
      $('#closeConfirm').modal('show');
  });    


  $(".confirm_modal_btn").click(function () {
      $('#readyConfirm').addClass('hide');
  });

  $(".submit_btn").click(function () {
      if ($("#reviewForm").valid()) {
          PostReview();
          window.location.reload();
      }
  });

  $(".close_btn_modal").click(function () {
      $('#closeConfirm').addClass('hide');
  })

  $(".close_modal_btn").click(function () {
      $('#closeConfirm').addClass('hide');
      $('#closeConfirm').modal('hide');
      HideReviewForm();
  });
});