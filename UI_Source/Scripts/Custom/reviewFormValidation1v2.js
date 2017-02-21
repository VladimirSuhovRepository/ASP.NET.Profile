function ValidateClick() {

    function AllowedSymbolsValidation(value, elem, args) {
        return /^[А-ЯЁа-яёA-Za-z0-9\s`~!@%&*()+|{};'",.<>\/?\\-]+$/.test(value);
    }
    $.validator.addMethod('allowed_symbols', AllowedSymbolsValidation, 'Вы использовали недопустимые символы');

    $('.reviewField').each(function () {
        $(this).rules('add', {
            required: true,
            maxlength: 450,
            allowed_symbols: true,
            messages: {
                required: "Оставьте комментарий к оценке",
                maxlength: "Вы превысили максимальное число символов",
                allowed_symbols: "Вы использовали недопустимые символы"
            }
        });
    });

    $('.commentField').each(function () {
        $(this).rules('add', {
            required: true,
            maxlength: 650,
            allowed_symbols: true,
            messages: {
                required: "Заполните поле",
                maxlength: "Вы превысили максимальное число символов",
                allowed_symbols: "Вы использовали недопустимые символы"
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

$("#reviewForm").validate({
  submitHandler: function(reviewForm) {
    PostReview();
    // do other things for a valid form
    // form.submit();
  }
});

function NoteReview(traineeId) {
    var el = $("tr#Trainee_" + traineeId);
    el.find('a[type="button"]').addClass("disabled");
    var newHtml = el.html() + ' <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1"><span class="checked glyphicon glyphicon-ok"></span></td>'
    el.html(newHtml);
};


function PostReview() {

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
       SkillGrade.Skill = {};
       SkillGrade.Skill.Name = $(this).find("label").text();
       SkillGrade.Skill.Id = $(this).find('input[name="SkillId"]').val();
       data.SkillGrades.push(SkillGrade);
    });
    var json = JSON.stringify(data);
    $.ajax({
       type: 'POST',
       url: '/Mentor/Review',
       data: json,
       contentType: 'application/json; charset=utf-8',
       success: function (data) {
           NoteReview(traineeId)
            // PLACE FOR CALLBACK

    console.log("ok Ajax");
    window.location.reload();
       },
       error: function (data, error, mess) {

           console.log("error: " + data + " " + error + " " + mess);
       }
    });
};