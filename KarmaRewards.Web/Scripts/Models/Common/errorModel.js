// model for the error
window.karma.Model.Error = Backbone.Model.extend({
    defaults: {
        title: 'Oops... No donuts for you',
        message: 'Due to some error, we can not process your request as of now.\r\nPlease try after some time.\r\n\r\n If it still fails please contact our support.',
        code: '400',
        error: '',
        btnLabel: 'Close'
    },

    initialize: function (options) {
        options = options || {};

        // set the error code
        if (_.isNumber(options.code) || !_.isEmpty(options.code)) this.set('code', options.code);

        // set the message
        if (!_.isEmpty(options.message)) {
            var message = options.message;
            if (options.additionalMessages && options.additionalMessages.length) {
                message += '\r\n Additional Information: ' + options.additionalMessages.join('.\r\n');
            }
            this.set('error', message);
        }
    }
});