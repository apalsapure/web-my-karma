karma.Views.RewardPointView = Backbone.View.extend({
    el: '.content',

    render: function () {
        this.bindPlugins();

        this.bindValidations();
    },

    fetchUser: function () {
        var that = this,
            to = $('#To').val(),
            from = $('#From').val(),
            moderatedBy = $('#ModeratedBy').val(),
            ids = [];
        var userCollection = new karma.Collection.UserCollection();
        this.listenToOnce(userCollection, 'reset', function () {
            if (!_.isEmpty(to)) $('#lblTo').text(userCollection.findWhere({ __id: to }).get('name'));
            if (!_.isEmpty(from)) $('#lblFrom').text(userCollection.findWhere({ __id: from }).get('name'));
            if (!_.isEmpty(moderatedBy)) $('#lblBy').text(userCollection.findWhere({ __id: from }).get('name'));
            that.render();
            that.hideLoader();
        });
        this.listenToOnce(userCollection, 'error', function (collection, error) {
            new karma.Views.ErrorView(error).render();
        });
        if (!_.isEmpty(to)) ids.push(to);
        if (!_.isEmpty(from)) ids.push(from);
        if (!_.isEmpty(moderatedBy)) ids.push(moderatedBy);
        userCollection.fetchByIds(ids);
    },

    bindPlugins: function () {
        var that = this;

        this.$to = $('#To', this.$el);
        this.$point = $('#Points', this.$el);

        this.userCollection = new karma.Collection.UserCollection();
        if (this.$to.is(':visible')) {
            // initialize auto complete plugin
            this.toAutoCompleteView = new window.karma.Views.AutocompleteView({
                element: this.$to,
                target: this.el,
                options: {
                    itemTemplate: "<div class='clearfix'>{{name}}</div>",
                    allowAddNew: false,
                    allowViewList: false,
                    onSearch: function (searchTerm, responseCallback) {
                        var filter = Appacitive.Filter.Or(
                                            Appacitive.Filter.Property('firstname').match(searchTerm),
                                            Appacitive.Filter.Property('lastname').match(searchTerm)
                                        );

                        var op = {
                            filter: filter,
                            paging: { pnum: 1, psize: 10 },
                            success: function () {
                                responseCallback(that.userCollection.models);
                            },
                            reset: true
                        };
                        that.listenTo(that.userCollection, 'error', function (collection, error) {
                            that.toAutoCompleteView.container.select2('close');
                            new karma.Views.ErrorView({ model: error }).render();
                        });
                        that.userCollection.fetchAll(op);
                    }
                }
            });
        }

        if (this.$point.val() === '') this.$point.val('10');
        this.$point.TouchSpin({
            buttondown_class: 'btn btn-grey-cascade',
            buttonup_class: 'btn btn-grey-cascade',
            min: 10,
            max: 10000,
            step: 10
        });

        //apply validation on select2 dropdown value change, this only needed for chosen dropdown integration.
        this.$to.change(function () {
            $(this).valid();//revalidate the chosen dropdown value and show error or success message for the input
        });
    },

    showLoader: function () {
        this.$el.showLoader();
    },

    hideLoader: function () {
        this.$el.hideLoader();
    },

    bindValidations: function () {
        $('form', this.$el).validate({
            rules: {
                'To': 'required',
                'Reason': 'required',
                'Points': 'required',
                'ModerateReason': 'required'
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