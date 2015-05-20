// appacitive storage for user
karma.Storage.Models.User = Appacitive.Object.extend('user');
karma.Storage.Collection.UserCollection = Appacitive.Collection.extend({
    model: karma.Storage.Models.User,
    query: (new Appacitive.Query(karma.Storage.Models.User))
                .orderBy(karma.config.sortBy)
                .isAscending(karma.config.sortAscending)
                .pageSize(karma.config.psize)
});


// backbone model for user
karma.Model.User = Backbone.Model.extend();
karma.Collection.UserCollection = Backbone.Collection.extend({
    model: karma.Model.User,

    fetchAll: function (options) {
        options = options || {};
        options.reset = options.reset ? true : false;
        options.paging = options.paging || { pnum: karma.config.pnum, psize: karma.config.psize };
        options.sorting = options.sorting || { sortBy: karma.config.sortBy, ascending: karma.config.sortAscending };
        options.freeText = options.freeText || '';

        var that = this;
        var users = new karma.Storage.Collection.UserCollection();
        var query = users.query();

        // set the paging info
        query.pageNumber(options.paging.pnum).pageSize(options.paging.psize);

        // set the sorting
        query.orderBy(options.sorting.sortBy).isAscending(options.sorting.ascending);

        // free text search
        query.freeText('*' + karma.utils.escapeAppacitiveSpecialChars(options.freeText) + '*');

        //set the fields
        if (options.fields) query.fields(options.fields);

        // set the filter
        if (options.filter) query.filter(options.filter);

        // fetch all users
        users.fetch().then(function (users) {
            // reset the collection
            if (options.reset) that.reset(undefined, { silent: true });

            // add current set of users to collection
            that.add(users.models);

            // set the options on the collection,
            // with total records count inside paging
            options.paging.total = users.query().results.total;
            that.options = $.extend({}, options);

            // raise a reset event
            if (!that.options.silent)
                that.trigger('reset', that, users, that.options);
        }, function (error) {
            that._triggerError(error);
        });

    },

    isValidUserName: function (userName) {
        var that = this;

        var options = {
            reset: true,
            filter: Appacitive.Filter.Property('username').like(userName),
            fields: ['__id']
        };

        this.listenToOnce(this, 'reset', function (users) {
            that.trigger('isUnique', users.length === 0, userName);
        }, function (error) {
            that._triggerError(error);
        });
        this.fetchAll(options)
    },

    _triggerError: function (error) {
        this.trigger('error', this, error);
    }
});