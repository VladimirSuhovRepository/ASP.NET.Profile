function mask(selector, regexp, allowedSymb) {

    $(selector).on("paste input", function (e) {

        var newValue = e.target.value.replace(regexp, "");

        if (newValue.length > allowedSymb) {

            newValue = newValue.substring(0, allowedSymb);
        }

        e.target.value = newValue;
    })
}