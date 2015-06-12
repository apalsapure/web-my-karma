// appacitive storage for karma
karma.Storage.Models.Point = Appacitive.Object.extend('points');
karma.Storage.Collection.PointCollection = Appacitive.Collection.extend({
    initialize: function () {
        this.query(new Appacitive.Query(karma.Storage.Models.Point)
                  .orderBy(karma.config.sortBy)
                  .isAscending(karma.config.sortAscending)
                  .pageSize(karma.config.psize));
    },
    model: karma.Storage.Models.Point
});


// backbone model for points
karma.Model.Point = Backbone.Model.extend();
karma.Collection.PointCollection = Backbone.Collection.extend({
    model: karma.Model.Point,

    fetchAll: function (options) {
        options = options || {};
        options.reset = options.reset ? true : false;
        options.paging = options.paging || { pnum: karma.config.pnum, psize: karma.config.psize };
        options.sorting = options.sorting || { sortBy: karma.config.sortBy, ascending: karma.config.sortAscending };
        options.freeText = options.freeText || '';

        var that = this;
        var points = new karma.Storage.Collection.PointCollection();
        var query = points.query();

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
        //else query.filter('');

        // fetch all points
        points.fetch().then(function (points) {
            // reset the collection
            if (options.reset) that.reset(undefined, { silent: true });

            // add current set of points to collection
            var pointsArr = [];
            _.forEach(points.models, function (point) {
                var p = point.toJSON();
                pointsArr.push(new karma.Model.Point(p));
            });
            that.add(pointsArr);

            // set the options on the collection,
            // with total records count inside paging
            options.paging.total = points.query().results.total;
            that.options = $.extend({}, options);

            if (that.models.length === 0) {
                that._triggerReset();
                return;
            }

            // fetch the user information
            // get the ids
            var ids = that.pluck('from');
            ids = _.union(ids, that.pluck('to'));
            ids = _.union(ids, that.pluck('moderated_by'));
            ids = _.filter(ids, function (id) { return !_.isEmpty(id) });
            // remove duplicates (not required as such as appacitive returns unique results anyways)
            ids = _.unique(ids);
            // now fetch, by passing fields in options, only these fields will be return by appacitive
            that._fetchUserByIds(ids, {
                reset: true,
                useCache: true,
                fields: ['firstname', 'lastname']
            });
        }, function (error) {
            that._triggerError(error);
        });
    },

    // gets the user info for the given set of ids
    _fetchUserByIds: function (ids, options) {
        var that = this;

        // handles error raised by company collection
        var handleError = function (model, error) {
            that._triggerError(error);
        };

        // create and bind events
        if (!that.userCollection) {
            // create collection
            that.userCollection = new karma.Collection.UserCollection();
            // bind events
            this.listenTo(that.userCollection, 'reset', this._mergeCollection);
            this.listenTo(that.userCollection, 'error', handleError);
        }
        else {
            // unbind events
            that.userCollection.reset(undefined, { silent: true });
        }

        that.userCollection.fetchByIds(ids, options);
    },

    // when user information is retrieved, merge it with point
    _mergeCollection: function (userCollection) {
        var that = this;

        // iterate over user collection, and set user info for respective point
        _.forEach(userCollection.models, function (user) {
            _.forEach(that.where({ 'to': user.get('__id') }), function (point) {
                point.set('to_name', user.get('name'));
            });
            _.forEach(that.where({ 'from': user.get('__id') }), function (point) {
                point.set('from_name', user.get('name'));
            });
            _.forEach(that.where({ 'moderated_by': user.get('__id') }), function (point) {
                point.set('moderated_by_name', user.get('name'));
            });
        });

        // now render the tours
        that._triggerReset();
    },

    // will trigger 'reset' event on the current collection
    _triggerReset: function () {
        if (_type.isFunction(this.options.success)) this.options.success.apply(this.options.context, [this, this.response, this.options]);
        // raise a reset event
        if (!this.options.silent)
            this.trigger('reset', this, this.response, this.options);
    },

    _triggerError: function (error) {
        error = new karma.Model.Error(error);
        this.trigger('error', this, error);
    }
});