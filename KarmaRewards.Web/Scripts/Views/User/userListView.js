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
            if (args.filter.excludeNoBirthDay)
                this.options.filter = Appacitive.Filter.And(
                                        Appacitive.Filter.Property('birthdate').isNotNull(),
                                        Appacitive.Filter.Property('birth_days').greaterThanEqualTo(moment().dayOfYear())
                                );
        }

        this.hbDetailTemplateName = 'userListDetailRow';
    },
    // method 
    formatDetailsRow: function (obj) {
        return karma.hb.userListDetailRow(obj);
    }
});