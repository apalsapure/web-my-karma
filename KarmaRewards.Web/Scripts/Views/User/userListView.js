karma.Views.UserListView = karma.Views.GridView.extend({

    detailsRowTemplate: '#tmplUserListDetailsRow',

    initializeChild: function (args) {
        // set the sorting
        if (args.sorting)
            this.options.sorting = args.sorting;

        // create the collection
        this.collection = new karma.Collection.UserCollection();

        // if filters are defined construct them
        if (args.filter) {
            // remove user with no points
            if (args.filter.excludeZeroPoints)
                this.options.filter = Appacitive.Filter.And(
                                        Appacitive.Filter.Aggregate('total_points').notEqualTo(0),
                                        Appacitive.Filter.Aggregate('total_points').isNotNull()
                                );

            // remove user who doesn't have birth day set
            if (args.filter.excludeNoBirthDay) {
                var filter = Appacitive.Filter.And(
                                        Appacitive.Filter.Property('birthdate').isNotNull(),
                                        Appacitive.Filter.Property('birth_days').greaterThanEqualTo(moment().dayOfYear())
                                );
                if (this.options.filter) this.options.filter = Appacitive.Filter.And(this.options.filter, filter);
                else this.options.filter = filter;
            }

            // remove disabled users
            if (args.filter.enabled) {
                var filter = Appacitive.Filter.Property('isenabled').equalTo(true);
                if (this.options.filter) this.options.filter = Appacitive.Filter.And(this.options.filter, filter);
                else this.options.filter = filter;
            }
        }

        this.hbDetailTemplateName = 'userListDetailRow';
    },
    // method 
    formatDetailsRow: function (obj) {
        return karma.hb.userListDetailRow(obj);
    }
});