(function (global) {
    global.utils = {};
    global.utils.escapeAppacitiveSpecialChars = function (strText) {
        strText = strText || '';
        return strText.replace(/([%^!~\[\]()":])/g, '\\$1').replace(/([/])/g, '/$1');
    };
})(window.karma);

(function ($) {
    $.fn.showLoader = function (options) {
        options = options || {};
        $(this).block({
            message: '<div class="loading-message"><div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></div>',
            baseZ: options.zIndex ? options.zIndex : 1000,
            centerY: options.cenrerY !== undefined ? options.cenrerY : false,
            css: {
                top: '10%',
                border: '0',
                padding: '0',
                backgroundColor: 'none'
            },
            overlayCSS: {
                backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                opacity: options.boxed ? 0.05 : 0.1,
                cursor: 'wait'
            }
        });
    };
    $.fn.hideLoader = function () {
        var that = this;
        $(this).unblock({
            onUnblock: function () {
                $(that).css('position', '');
                $(that).css('zoom', '');
            }
        });
    };
    $.fn.initUniform = function () {
        //iCheck for checkbox and radio inputs
        $('input[type="checkbox"].minimal, input[type="radio"].minimal', $(this)).iCheck({
            checkboxClass: 'icheckbox_minimal-blue',
            radioClass: 'iradio_minimal-blue'
        });
    };
}(jQuery));