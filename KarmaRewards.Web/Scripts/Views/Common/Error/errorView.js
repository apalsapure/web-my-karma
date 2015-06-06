// view to render error
window.karma.Views.ErrorView = Backbone.View.extend({
    el: '#divErrorModalContainer',

    template: '#errorModalTemplate',

    render: function () {

        ModalWin.deactivate();

        // if error view template is not compiled,
        // then do it and store a reference
        if (!karma.hb.errorView)
            karma.hb.errorView = Handlebars.compile($(this.template).html());

        // if model is not, set the default one
        if (!this.model) this.model = new karma.Model.Error();

        // check for status code
        var sessionExpired = this._isSessionExpired();

        // get the html from handle bar 
        // and render the error view
        this.$el.html(karma.hb.errorView(this.model.toJSON()));

        // if additional error information is available
        // show it
        if (!_.isEmpty(this.model.get('error'))) {
            this.$el.find('.show-error-details').removeClass('hide');
        }

        // show the modal
        $('.modal', this.$el).modal('show');

        if (sessionExpired) {
            $('.modal', this.$el).on('hidden.bs.modal', function () {
                window.location = '/account/logout?returnurl=' + escape(window.location.pathname + window.location.search);
            });
        }

        this._bindEvents();

        // maintain the chain-ability
        return this;
    },

    _isSessionExpired: function () {
        if (this.model.get('code') !== karma.config.sessionExpiredCode) return false;
        this.model.set('title', 'Your session has expired');
        this.model.set('message', 'Looks like your session has expired. \nFor security reasons; we automatically end sessions after an hour of inactivity. \n\nNow you will be redirected to login page.');
        this.model.set('error', '');
        this.model.set('btnLabel', '<i class="fa fa-key margin5-right"></i>Login ');
        return true;
    },

    _bindEvents: function () {
        var that = this;
        $('a.show-error-details', this.$el).unbind('click').click(function () {
            $('#divErrorDetails', that.$el).toggleClass('hide');
        });
        //19036
    }
});