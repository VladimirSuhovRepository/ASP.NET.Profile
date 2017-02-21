$(document).ready(function () {

    var $page = $('html, body');
    var $toTop = $('#toTop');

    $toTop.hide();

    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $toTop.fadeIn();
        } else {
            $toTop.fadeOut();
        };
    });

    $toTop.on('click', debounce(ToTop, 850, true));

    function debounce(func, wait, immediate) {
        var timeout;
        return function () {
            var context = this, args = arguments;
            var later = function () {
                timeout = null;
                if (!immediate) func.apply(context, args);
            };
            var callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func.apply(context, args);
        };
    };


    function ToTop(e) {
        e.preventDefault();
        $page.animate({ scrollTop: 0 }, 800);
    }

});