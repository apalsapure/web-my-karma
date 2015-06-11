karma.Views.PointListView = karma.Views.GridView.extend({

    detailsRowTemplate: '#tmplPointListDetailsRow',

    initializeChild: function (args) {
        // set the sorting
        if (args.sorting)
            this.options.sorting = args.sorting;

        // create the collection
        this.collection = new karma.Collection.PointCollection();

        // if filters are defined construct them
        if (args.filter) {
            this.ownerId = args.filter.ownerId;

            if (args.filter.showFor)
                this.options.filter = Appacitive.Filter.Property('to').equalTo(args.filter.showFor);
        }

        this.hbDetailTemplateName = 'pointListDetailRow';
    },

    // method 
    formatDetailsRow: function (obj) {
        obj['isOwner'] = obj.from === this.ownerId;
        return karma.hb.pointListDetailRow(obj);
    }
});

