karma.Views.AddUserView = Backbone.View.extend({
    initialize: function (options) {
        this.userCollection = new karma.Collection.UserCollection();
        this.listenTo(this.userCollection, 'isUnique', this._handleUnique);
    },

    _handleUnique: function (isUnique) {
        var $email = $('#Email', this.$el),
            $userName = $('#Username', this.$el);

        $userName.removeClass('spinner');

        if (isUnique) {
            $userName.closest('.form-group').removeClass('has-error');
            // set the email address
            if (!_.isEmpty($.trim($email.val()))) return;
            $email.val($userName.val() + '@tavisca.com');
        } else {
            $userName.closest('.form-group').addClass('has-error');
        }
    },

    render: function ($container) {
        this.$el = $container;

        this._bindEvents();       

        this._bindFormValidation();
    },

    _bindEvents: function () {
        var that = this;

        // bind input mask
        $("#JoiningDateStr, #DateOfBirthStr", this.$el).inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });

        // on blur check if user name is unique
        $('#Username', this.$el).blur(function () {
            var $this = $(this),
                userName = $.trim($this.val());

            // user name is empty
            if (_.isEmpty(userName)) return;

            // check if user name is unique
            that.userCollection.isValidUserName(userName);

            // show loader
            $this.addClass('spinner');
        });
    },

    _bindFormValidation: function () {
        $('form', this.$el).validate({
            rules: {
                Username: 'required',
                Email: 'required',
                FirstName: 'required',
                LastName: 'required',
                JoiningDate: 'required',
                Designation: 'required'
            },
            errorPlacement: function (error, element) {
                return true;
            },
            highlight: function (element) {
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            },
            success: function (element) {
                $(element).closest('.form-group').removeClass('has-error');
            }
        });
    }
});