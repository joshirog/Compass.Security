$(document).on('ready', function () {
    $('.js-toggle-password').each(function () {
        new HSTogglePassword(this).init()
    });

    $('.js-validate').each(function() {
        $.HSCore.components.HSValidation.init($(this));
    });

    $('.js-select2-custom').each(function () {
        var select2 = $.HSCore.components.HSSelect2.init($(this));
    });
});

if (/MSIE \d|Trident.*rv:/.test(navigator.userAgent)) 
    document.write('<script src="~/vendor/babel-polyfill/polyfill.min.js"><\/script>');