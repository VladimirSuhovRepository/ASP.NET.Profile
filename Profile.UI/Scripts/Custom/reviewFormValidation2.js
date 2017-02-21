$(document).ready(function(){

//ЭТО нужная функция. раскомментировать
     function ValidateClick() {

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
     };

  //$('#saveButton').click(function () {   //  это только для локального использования, потом убрать

  //    $('.reviewField').each(function () {
  //        $(this).rules('add', {
  //            required: true,
  //            messages: {
  //                required: "Оставьте комментарий к оценке",
  //            }
  //        });
  //    });

  //    $('.commentField').each(function () {
  //        $(this).rules('add', {
  //            required: true,
  //            messages: {
  //                required: "Заполните поле",
  //            }
  //        });
  //    });

  //    $('.markSelect').each(function () {
  //        $(this).rules('add', {
  //            required: true,
  //            messages: {
  //                required: "Поставьте оценку"
  //            }
  //        });
  //    });
  //});    

    function PostReview() {
        console.log("Ajax sent")
        var data = {};
        data.SkillGrades = [];
        traineeId = $('input[name="TraineeId"]').val();
        data.TraineeId = traineeId;
        data.ReviewerId = $('input[name="ReviewerId"]').val();
        //data.CommentWeak = $('textarea[name="commentWeak"]').val();
        //data.CommentStrong = $('textarea[name="commentStrong"]').val();
        $(".skillgrade").each(function () {
            var SkillGrade = {};
            SkillGrade.Comment = $(this).find('textarea').val();
            SkillGrade.Mark = $(this).find("select option:selected").val();
            //SkillGrade.Skill = {};
            //SkillGrade.Skill.Name = $(this).find("label").text();
            SkillGrade.SkillId = $(this).find('input[name="SkillId"]').val();
            data.SkillGrades.push(SkillGrade);
        });
        var json = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Trainee/CreateReview',    //ИЗМЕНИТЬ
            data: json,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                NoteReview(traineeId);
                // PLACE FOR CALLBACK
                    console.log("ok Ajax");
                    $('#closeConfirm').modal('hide');
                    HideReviewForm();
                    window.location.reload();
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
        $('#groupmate1').addClass('hide');
        $('#groupmate1').modal('hide');
    };


    // раскомменить 117 118 119
    $('.btn-review').click(function (e) {   //КНОПКА "ОСТАВИТЬ ОТЗЫВ"
        console.log($(this).attr("value"));
         var val = jQuery(this).attr("value");
         $('#trainee-review').load('/Trainee/CreateReview/?id=' + val, function () {  // ССЫЛКУ ИЗМЕНИТЬ

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
                      //window.location.reload();
                  }
              });

              $(".close_btn_modal").click(function () {
                  $('#closeConfirm').addClass('hide');
              })

              $(".close_modal_btn").click(function () {
                  $('#closeConfirm').addClass('hide');
                  $('#closeConfirm').modal('hide');
                  $('#readyConfirm').modal('hide');
                  HideReviewForm();
              });

              // раскомменить
              $('#saveButton').on('click', ValidateClick());  


              $("textarea").keypress( function(e) {
                var chr = String.fromCharCode(e.which);
                  if ("{}[]<>~`\\|/*^%$#@".indexOf(chr) >= 0){
                    return false;
                    } else {
                      return true;
                    }
              });
              // раскомменить
              $('#groupmate1').removeClass('hide');
    // раскомменить 170 171
       });
     });          



});