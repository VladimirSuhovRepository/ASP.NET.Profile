function cabinetEdit(container) {

    var errors =
        [
            {
                selector: "#name",
                mess: "Это поле обязательно для заполнения",
                reg: /.+/i,
                req: true
            },
            {
                selector: "#phone",
                reg: /[^_]{16}/i,
                mess: "Укажите корректный номер телефона",
                req: false
            },
            {
                selector: "#email",
                reg: /.*[@]+.*[\.]+.*/ig,
                mess: "E-mail должен содержать “@” и “.”",
                req: false
            }
        ]
    
    function changeData() {

        window.location.hash = "";

        $(container + " input").on("input", function () {

            window.location.hash = "#change";
            $(container + " #save").removeClass("disabled");
        })
    }

    function maskInputs() {

        $('#phone').inputmask("375-99-999-99-99");
        $("#email").inputmask("Regex");
        $("#skype").inputmask("Regex");
        $("#linkedin").inputmask("Regex");
        $("#name").inputmask("Regex");
        $("#company").inputmask("Regex");
    }

    function validationView(input, regexp, message, req) {

        var inputValue = $(container + " " + input).val();
        var conditional;

        $(container + " " + input).removeClass("error").next(".error").remove();

        if (req) {

            conditional = !inputValue.match(regexp);
        }
        else {

            conditional = inputValue && !inputValue.match(regexp);
        }

        if (conditional) {

            $(container + " " + input).addClass("error").after("<span class='error'>" + message + "</span>");

            return false
        }
        return true
    }

    function validate(err, event) {

        var valid = err.map(function (it, ind) {

            return validationView(it.selector, it.reg, it.mess,it.req);
        });

        if (valid.indexOf(false) < 0 && event) {

            $(container + " #saveData").modal("show");
        }
    }

    function saveData() {

        $(document).on("click", container + " #save:not(.disabled)", function (e) {

            e.preventDefault();
            validate(errors, e);
        })
    }

    function cancelEdit() {

        $(container + " #cancel").on("click", function (e) {

            if (window.location.hash == "#change") {

                e.preventDefault();
                $(container + " #cancelData").modal("show");
            }
        })
    }

    function inputValidate() {

        $(document).on("input", container + " .validate.error", function (e) {

            validate(errors);
        })
    }

    return ({
        init: function () {

            maskInputs();
            saveData();
            inputValidate();
            cancelEdit();
            changeData();
        }
    })
}

$(document).ready(function () {

    var CabinetEdit = cabinetEdit(".cabinet.edit").init();
})

