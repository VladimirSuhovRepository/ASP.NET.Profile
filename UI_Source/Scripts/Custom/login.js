//RegExp for Login validation
var RV_LOGIN = /^\s*[.\w@-]{1,64}\s*$/;

//RegExp for Password validation
var RV_PASS = /^[a-z\d]{6,14}$/i;

$(document).ready(function () {

    function error(element) {
        element.addClass('error');
        element.parent().addClass('has-error');
    }

    function notError(element) {
        element.removeClass('error');
        element.parent().removeClass('has-error');
    }

    function validate(element, regExp) {
        var val = element.val();
        if (val == '') {
            error(element);
        }
        else {
            notError(element);
            if (!regExp.test(val)) {
                element.val('');
                element.addClass('error');
                element.parent().addClass('has-error');
            };
        };
    }

    var errorHelp = '<div id="globalError"><span class="prf-global-error">Введено неправильное имя пользователя / пароль. Попробуйте еще раз</span></div>';

    $('#Login, #Password').removeAttr('required');

    $('.prf-error-box').hide();

    //Set handler on form submit
    $('form').on('submit', function () {
        if ($('#Login').val() == '') {
            error($('#Login'));
            error($('#Password'));

            $('#Password').val('');
            if ($('#globalError').length == 0) {
                $('form').append(errorHelp);
            };
            return false;
        }

        validate($('#Login'), RV_LOGIN);
        validate($('#Password'), RV_PASS);

        if ($('.error').length != 0) {

            if ($('#globalError').length == 0) {
                $('form').append(errorHelp);
            }

            return false;
        } else {
            return true;
        };

    });

});