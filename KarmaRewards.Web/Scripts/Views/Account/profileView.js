karma.Views.BasicProfileView = Backbone.View.extend({
    el: '#tab_1',
    render: function () {
        this.bindPlugin();
        this.bindValidation();
    },

    bindPlugin: function () {
        // bind input mask
        $("#DateOfBirthStr", this.$el).inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    },

    bindValidation: function () {
        $('form', this.$el).validate({
            rules: {
                FirstName: 'required',
                LastName: 'required',
                DateOfBirthStr: 'required'
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