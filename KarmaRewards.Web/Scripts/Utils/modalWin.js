var ModalWin = new (function () {

    this.template = '#tmplModal';

    var ops = {
        buttons: [],
        canClose: true,
        title: 'Your sample dialog title'
    };

    var modalSize = {
        "small": "modal-sm",
        "medium": "modal-md",
        "large": "modal-lg"
    };

    this.showLoader = function () {
        $('.modal-content', this.$modal).showLoader();
    };

    this.hideLoader = function () {
        $('.modal-content', this.$modal).hideLoader();
    };

    this.showBodyLoader = function () {
        $('.modal-body', this.$modal).showLoader();
    };

    this.hideBodyLoader = function () {
        $('.modal-body', this.$modal).hideLoader();
    };

    this.disableFooterButtons = function () {
        $('input[type=button]', $('.modal-footer', this.$modal)).each(function () {
            this.attr('disabled', 'disabled');
        });
    };

    this.enableFooterButtons = function () {
        $('input[type=button]', $('.modal-footer', this.$modal)).each(function () {
            this.removeAttr('disabled');
        });
    };

    this.render = function (el, options) {

        options = $.extend({}, ops, options);

        ModalWin.deactivate();

        if (options.buttons.length > 0) {
            $.each(options.buttons, function (i, button) {
                if (button.cssClass) {
                    button.cssclass = button.cssClass;
                } else if (button.isCancel) {
                    button.cssclass = "btn-default"
                } else {
                    button.cssclass = "btn-primary";
                }

                if (!button.id) button.id = "btn" + (parseInt(Math.random() * 100000));
            });
        }

        options.modalSize = modalSize[options.modalSize] || '';

        if (options.modalClass) options.modalSize += " " + options.modalClass;

        if (!this.hbTemplate) this.hbTemplate = Handlebars.compile($(this.template).html());

        $modal = $(this.hbTemplate(options));

        this.el = el;
        this.$modal = $modal;

        if (options.contentString) el = $('<div></div>').html(options.contentString);
        el.appendTo($modal.find('.modal-body'));

        if (options.disableFooterButtons) {
            this.disableFooterButtons();
        };

        $modal.appendTo('body');

        $modal.modal({ 'show': true, 'backdrop': 'static' });

        var executionContext = options.context

        options.buttons.forEach(function (btn) {
            var handler = btn.onClick;
            var $button = $("#" + btn.id, $modal);
            if (btn.isOK && !btn.cssclass || btn.cssclass.indexOf('btn-') === -1)
                $button.addClass('btn-primary')
            if (btn.cssclass) $button.addClass(btn.cssclass)
            var closeAfterClick = btn.closeAfterClick || true;
            if (handler || btn.isCancel) {
                $button.data("options", btn);
                if (btn.isCancel && !handler) {
                    $button.click(ModalWin.deactivate);
                    return;
                }

                $button.click(function (e) {
                    options = $(this).data("options");
                    handler = btn.onClick;
                    handler.apply(executionContext || {}, [e]);
                    if (btn.closeAfterClick || btn.isCancel) {
                        ModalWin.deactivate();
                    }
                });
            } else {
                $button.click(function () {
                    ModalWin.deactivate();
                })
            }
        });

        $('#lnkModalClose', $modal).bind('click', function () {
            if (typeof options.onClose == 'function') options.onClose.apply(executionContext, []);
            if (!options.doNotCloseAfterClick) ModalWin.deactivate();
        });

        // focus the first textbox/textarea
        setTimeout(function () {
            var $inputs = $('textarea:visible, input[type="text"]:visible', $modal);
            if ($inputs.length > 0) $inputs.first().focus();
        }, 250);

        return $modal;
    };

    this.deactivate = function () {
        $('.modal').find('select').each(function (i, a) {
            $(a).select2('close');
        });

        $('.modal').remove();
        $('.modal-backdrop').remove();
        $('body').removeClass("modal-open");
    };


});


(function ($) {

    $.fn.shroom = function (options) {
        ModalWin.render(this, options);
    };

})(jQuery);
