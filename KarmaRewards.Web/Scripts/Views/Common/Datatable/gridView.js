window.karma.Views.GridView = Backbone.View.extend({

    baseTemplate: '#tmplCommonGridList',

    collection: null,
    hbDetailTemplateName: null,

    defaultArgs: {
        allowGlobalSearch: true,
        allowFilters: true,
        grid: {
            allowGroupSelect: false,
            allowSingleSelect: false,
            allowSelect: false,
            allowDetails: true
        }
    },

    args: null,

    options: null,

    filters: null,
    thColumns: null,
    tdColumns: null,

    selected: null,
    disabled: null,

    initialize: function (args) {
        // map the arguments
        this.args = $.extend(true, {}, this.defaultArgs, args);

        // TODO: do validation
        if (!this.args.grid.columns || this.args.grid.columns.length === 0) throw Error('Columns for a grid is required.');

        // set the defaults
        this._setDefaults();

        // give control to child to initialize
        this.initializeChild(args);

        // validation
        if (this.args.grid.allowDetails) {
            if (!this.detailsRowTemplate) throw Error('Detail Row Template is missing.');
            if (!this.hbDetailTemplateName) throw Error('Set handle bar template name for detail row.');

            // compile detail row template
            if (!karma.hb[this.hbDetailTemplateName])
                karma.hb[this.hbDetailTemplateName] = Handlebars.compile($(this.detailsRowTemplate).html());
        }

        // create the columns for the grid
        this._populateColumns();

        // in case of error, show error modal
        this.listenTo(this.collection, 'error', this._handleError);
    },

    // method needs to be over ridden by child
    initializeChild: function (args) { },

    _setDefaults: function () {
        this.filters = [];
        this.thColumns = [];
        this.tdColumns = [];
        this.selected = [];
        this.disabled = [];

        this.options = {
            fields: [],
            paging: { pnum: karma.config.pnum, psize: karma.config.psize },
            sorting: { sortBy: karma.config.sortBy, ascending: karma.config.sortAscending },
            silent: true,
            reset: true
        };
    },

    // create the columns for the grid
    _populateColumns: function () {
        var that = this;
        // add a column if select is allowed
        if (this.args.grid.allowSelect) {
            this.thColumns.push({ cssClass: 'th-check', style: 'width:1%' });
            this.tdColumns.push({ mDataProp: 'checkboxId', orderable: false });
        }

        // add a column if details is allowed
        if (this.args.grid.allowDetails) {
            this.thColumns.push({ cssClass: 'no-bg th-expand', style: 'width:4px' });
            this.tdColumns.push({ defaultContent: '<span class="row-details row-details-close"></span>', orderable: false });
        }

        // now iterate over each column, and populate both the columns
        _.forEach(this.args.grid.columns, function (col) {
            if (col.displayColumn === false) return;
            that.thColumns.push({ style: col.style, label: col.dName });
            that.tdColumns.push({ sTitle: col.dName, mDataProp: col.binding, property: col.property, orderable: col.allowSort });
        });
    },

    // renders the grid in given container
    render: function ($container) {
        if (!$container) throw Error('Container is required for content list view');

        this.$container = this.$el = $container;

        // if list view template is not compiled,
        // then do it and store a reference
        if (!karma.hb.baseGridListView)
            karma.hb.baseGridListView = Handlebars.compile($(this.baseTemplate).html());

        // dump the view html
        this.$el.html(karma.hb.baseGridListView({
            allowGlobalSearch: this.args.allowGlobalSearch,
            allowFilters: this.args.allowFilters,
            thead: this.thColumns
        }));

        // set the table element
        this.$table = $('.dataTable', this.$el);

        // container for the filters
        this.$filterContainer = $('.filter-container', this.$el);

        // give control child to render it's stuff
        this.renderChild();

        // binds the jquery data grid to the collection
        this._bindGrid();

        // render the filters on the grid if allowed
        this._renderFilters();

        // bind the events
        this.bindEvents();

        return this;
    },

    // method needs to be over ridden by child
    renderChild: function () { },

    // method needs to be over ridden by child
    formatDetailsRow: function (obj) { },

    // renders the filters
    _renderFilters: function () {
        // filters are not allowed
        if (!this.args.allowFilters) return;

        var that = this;

        // now iterate over each column, and populate both the columns
        _.forEach(this.args.grid.columns, function (col) {
            if (!col.filter) return;

            // get the view implementation
            var view = karma.Views[col.filter.type + 'FilterView'];
            if (!view) throw Error('Invalid filter type: ' + col.filter.type);

            // build the args
            var args = {
                property: col.property,
                label: col.dName,
                filter: col.filter,
                onFilterChange: function (changeFilter) {
                    // if filter found remove the older one and updated one
                    that.filters = _.reject(that.filters, function (f) { return f.id == changeFilter.id });
                    that.filters.push(changeFilter);
                    that._reDrawGrid();
                }
            }

            // create a container for the filter and dump it on DOM
            var $container = $('<div class="' + (col.filter.cssClass || '') + '"></div>')
            that.$filterContainer.append($container);

            // create view and render
            new view(args).render($container);
        });
    },

    _bindGrid: function () {
        var that = this;

        // create the data table
        this.grid = new Datatable();

        this.grid.init({
            src: that.$table,
            dataTable: {
                "aoColumns": that.tdColumns,
                fnServerData: function (sSource, aoData, fnCallback, oSettings) {
                    // set the free text search
                    if (oSettings.oPreviousSearch)
                        that.options.freeText = oSettings.oPreviousSearch.sSearch;

                    // set the paging
                    that.options.paging.psize = oSettings._iDisplayLength;
                    that.options.paging.pnum = Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength) + 1;

                    // set sorting
                    // oSettings.aaSorting is an array, with column index at zero place and sort direction at first index
                    var sorting = oSettings.aaSorting[0],
                         col = that.tdColumns[sorting[0]],
                         isAscending = sorting[1] === 'asc';

                    if (col && col.orderable) {
                        that.options.sorting.sortBy = col.property;
                        // ascending or not
                        that.options.sorting.ascending = isAscending;
                    }

                    // do a fetch call
                    that.fetch(function (collection, response, options) {
                        // pass the result to the data tables call back
                        // which will render the data table
                        var data = that.collection.toJSON();

                        data = that._formatData(data);

                        // mark disable rows
                        if (that.collection.disabled) {
                            that.disabled = $.extend(true, {}, that.collection.disabled);
                        }

                        // if select is allowed add a dummy json field
                        // so that id is mapped to the checkbox
                        if (that.args.grid.allowSelect) {
                            _.forEach(data, function (d) {
                                var item = _.findWhere(that.selected, { id: d.id }),
                                isDisabled = _.findWhere(that.disabled, { id: d.id });
                                // if the object is in disable state, then set item as disable
                                // so that grid renders that item checked and disabled
                                item = item || isDisabled;
                                d['checkboxId'] = '<input type="checkbox" name="id[]" value="' + d.id + '" ' + (item ? 'checked="checked"' : '') + (isDisabled ? 'disabled="disabled"' : '') + '>';
                            });
                        }

                        // call back provided by data table
                        fnCallback({
                            recordsTotal: options.paging.total,
                            recordsFiltered: options.paging.total,
                            data: data
                        });
                        if (that.args.grid.replaceRow) {
                            $('.row-details').trigger('click').parent().remove();
                            $('.th-expand').remove();
                        }

                        // hide the loader
                        that.hideLoader();

                        // remove sorting icon
                        $('.th-check', that.$table).removeClass('sorting_asc').addClass('sorting_disabled');

                        // if select is allowed and group select is allowed
                        // then don't show group checkbox
                        if (that.args.grid.allowSelect && that.args.grid.allowGroupSelect) {
                            if ($('.group-checkable', that.$table).size() === 0)
                                $('.th-check', that.$table).html('<input type="checkbox" class="group-checkable"/>');

                            $('.group-checkable', that.$table).attr("checked", false);
                            $('.group-checkable', that.$table).initUniform();

                            setTimeout(function () {
                                that.grid.bindGroupSelect();
                            }, 0);
                        }
                    });
                }
            },
            // Formatting function for row details
            // obj is the original data object for the row
            format: function (obj) { return that.formatDetailsRow(obj); },

            onRowCreate: function () { if (_.isFunction(that.args.grid.onRowCreate)) that.args.grid.onRowCreate.apply(this, arguments); },

            onSelectChange: function (id, isChecked) { that._checkboxChange(id, isChecked); },

            onRowExpand: function (rowContainer, dataTableRow) {
                if (_.isFunction(that.options.onRowExpand))
                    that.options.onRowExpand(rowContainer, dataTableRow);
            },

            replaceRow: this.args.grid.replaceRow,

            allowSingleSelect: this.args.grid.allowSingleSelect
        });
    },

    _formatData: function (data) {
        _.forEach(this.args.grid.columns, function (col) {
            switch (col.type) {
                case 'date':
                    if (!col.format) return;
                    _.forEach(data, function (d) {
                        if (!d[col.binding]) d[col.binding] = '-';
                        else d[col.binding] = moment(d[col.binding]).format(col.format);
                    });
                    break;
            }
        });
        return data;
    },

    // check box change handle
    _checkboxChange: function (id, isChecked) {
        if (karma.config.debug) console.log(id + ' ' + isChecked);
        // add or remove the item depending upon isChecked
        if (isChecked) {
            // get the item from collection
            var item = this.collection.findWhere({ id: id });
            if (!item) return;
            if (this.args.grid.allowSingleSelect) {
                this.selected = [item];
            } else {
                this.selected.push(item);
            }
        } else {
            // remove the item
            this.selected = _.reject(this.selected, function (item) { return id === item.get('id'); });
        }

        // set the count on the grid
        this.grid.setSelectedItemCount(this.selected.length);
    },

    // depending upon arguments, bind the events
    bindEvents: function () {
        var that = this;
        // bind filter drop down click event
        if (this.args.allowFilters) {
            $('button.filter,.close-filter-block', this.$el).unbind('click').click(function (e) {
                $(this).toggleClass('open');

                if ($(this).children('i').hasClass('fa-caret-down')) {
                    $(this).children('i').switchClass("fa-caret-down", "fa-caret-up", 900);
                } else {
                    $(this).children('i').switchClass("fa-caret-up", "fa-caret-down", 900);
                }

                $(this).parents('.filter-section').find('.filter-block').toggle().toggleClass('open');
                if (!$(this).parents('.filter-section').find('.filter-block').hasClass('open')) {
                    $(this).parents('.filter-section').find('.filter-thirds > div').css({ "height": "auto" });
                } else {
                    $(this).parents('.filter-section').find('.filter-thirds > div').height($('.filter-thirds', that.$el).height() - 50);
                }
            });
        }

        // bind free text search
        if (this.args.allowGlobalSearch) {
            var $txtSearch = $('.global-search', this.$el);

            // performs free text search
            var freeTextSearch = function () {
                that.grid.getDataTable().search(karma.utils.escapeAppacitiveSpecialChars($.trim($txtSearch.val())));
                that._reDrawGrid();
            }

            // key up event to trap enter
            $txtSearch.keyup(function (e) {
                if (e.keyCode != 13) return;
                freeTextSearch();
            });

            // search icon clicked
            $('.fa-search', this.$el).click(freeTextSearch);
        }
    },

    // helper method
    fetch: function (onSuccess) {
        var that = this;

        // show the loader
        this.showLoader();

        // clone the options
        var options = $.extend(true, {}, this.options);
        options.success = onSuccess;

        var filters = [];
        if (options.filter) filters.push(options.filter);

        // apply filters (if any)
        _.forEach(this.filters, function (filter) {
            if (!filter || !filter.value) return;
            filters.push(filter.value);
        });

        // If no filter defined in options but selected from explicit filter options then set it in options.filter
        if (filters.length == 1 && !options.filter) options.filter = filters[0];
        else if (filters.length > 1) options.filter = Appacitive.Filter.And.apply(Appacitive.Filter.And, filters);

        // do a fetch call
        if (this.options.modelFunctionName) this.collection[this.options.modelFunctionName]($.extend(true, {}, options));
        else this.collection.fetchAll($.extend(true, {}, options));
    },

    // shows loader
    showLoader: function () {
        this.$el.showLoader();
    },

    // hides loader
    hideLoader: function () {
        this.$el.hideLoader();
    },

    // redraws the grid
    _reDrawGrid: function () {
        // trigger a redraw of grid
        this.grid.getDataTable().draw();
    },

    // handle the error
    _handleError: function (model, error) {
        if (error) error.set('message', 'Currently we are not able to access the information.\r\nPlease try again after some time.');
        // show the error inside a modal
        new karma.Views.ErrorView({ model: error }).render();

        this.hideLoader();
    }
});


// model for the filter
karma.Models = karma.Models || {};
karma.Models.filterModel = Backbone.Model.extend({
    defaults: {
        id: null,
        value: null
    }
});

// base view for all filter views
karma.Views.baseFilterView = Backbone.View.extend({
    $text: null,
    query: '',

    initialize: function (args) {
        this.args = args || {};
        if (!this.args.filter) throw Error('Filter not defined');

        this.initializeChild();

        // validation
        if (!this.template) throw Error('Template is required');
        if (!this.hbTemplateName) throw Error('HBTemplate is required');
        if (!args.property) throw Error('Property is required');
        if (!args.label) throw Error('Label is required');

        // set the filter model id
        this.model = new karma.Models.filterModel({
            id: '_filterId-' + _.uniqueId()
        });

        // function which will be called when filter is changed
        this.args.onFilterChange = _.isFunction(args.onFilterChange) ? args.onFilterChange : function () { };

        // compile the template
        if (!karma.hb[this.hbTemplateName]) karma.hb[this.hbTemplateName] = Handlebars.compile($(this.template).html());
    },

    initializeChild: function () { },

    render: function ($container) {
        if (!$container) throw Error('Container is required for string filter view');
        this.$el = $container;

        // dump the html
        $container.html(karma.hb[this.hbTemplateName]({
            id: 'filter-' + _.uniqueId(),
            label: this.args.label
        }));

        this.renderChild();

        // bind the events
        this.bindEvents();

        // to maintain the chain of operation
        return this;
    },

    renderChild: function () { },

    bindEvents: function () { },

    notifyFilterChange: function () { this.args.onFilterChange(this.model.attributes); }
});

// view which will handle string filter view
karma.Views.stringFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplStringFilterTemplate',
    hbTemplateName: 'stringFilterView',

    renderChild: function () {
        var that = this;
        setTimeout(function () {
            $(document).initUniform()
        }, 1000);
    },

    bindEvents: function () {
        var that = this;

        // get the reference of the text box
        this.$text = $('.form-control', this.$el);

        // keyup on search text box (to catch enter)
        this.$text.keyup(function (e) {
            if (e.keyCode != 13) return;
            that._search();
        });

        // on lost focus perform search
        this.$text.blur(function () { that._search(); });

        // on radio buttons change do search
        $('.radio-list input', this.$el).change(function () {
            // this will enforce a search
            that.query = '';
            that._search();
        });
    },

    _search: function () {
        // if text box is empty and name filter is not applied, don't do the search
        var term = $.trim(this.$text.val());
        if ((_.isEmpty(term) && !this.model.get('value')) || this.query == term) return;

        // store the query
        this.query = term;

        // reset the filter
        this.model.set('value', null);

        if (!_.isEmpty(this.query)) {
            var condition = $('.radio-list input:checked', this.$el).val();
            this.model.set('value', karma.Filter.Property(this.args.property)[condition](karma.utils.escapeAppacitiveSpecialChars(this.query)));
        }

        this.notifyFilterChange();
    }
});

// view which will handle date filter view
karma.Views.dateFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplDateFilterTemplate',
    hbTemplateName: 'dateFilterView',

    initializeChild: function () {
        this.args.filter.range = this.args.filter.range || {
            min: karma.config.dateRangePicker.minDate,
            max: karma.config.dateRangePicker.maxDate
        };
    },

    renderChild: function () {
        var that = this;

        // get the reference of the text box
        this.$text = $('.form-control', this.$el);

        this.$text.daterangepicker({
            opens: (that.args.filter.opens || 'right'),
            startDate: moment().subtract(karma.config.dateRangePicker.startDate, 'days'),
            endDate: moment(),
            minDate: that.args.filter.range.min,
            maxDate: that.args.filter.range.max,
            dateLimit: {
                days: 60
            },
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            buttonClasses: ['btn'],
            applyClass: 'green-meadow',
            cancelClass: 'default',
            format: 'MM/DD/YY',
            separator: ' - ',
            locale: {
                applyLabel: 'Apply',
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Custom Range',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                firstDay: 1
            }
        });

        // event raised by the plugin
        this.$text.on('apply.daterangepicker', function (e, datePicker) {
            that.$text.val(datePicker.startDate.format('MM/DD/YY') + ' - ' + datePicker.endDate.format('MM/DD/YY'));
            that._search();
        });
    },

    bindEvents: function () {
        var that = this;

        // keyup on search text box (to catch enter)
        this.$text.keyup(function (e) {
            if (e.keyCode != 13) return;
            that._search();
        });

        // on lost focus perform search
        this.$text.blur(function () { that._search(); });
    },

    // helper function
    _getFormatedDate: function (date) {
        return moment(date, 'MM/DD/YY HH:mm:zz').toDate();
    },

    _search: function () {
        // if text box is empty and name filter is not applied, don't do the search
        var term = $.trim(this.$text.val());
        if ((_.isEmpty(term) && !this.model.set('value')) || this.query == term) return;

        // this will hide the date picker (if visible)
        if ($('.daterangepicker .cancelBtn').is(':visible')) $('.daterangepicker .cancelBtn').trigger('click');

        // store the query
        this.query = term;

        // reset the filter
        this.model.set('value', null);

        // build the filter
        if (term.indexOf('-') !== -1) {
            var range = this.query,
                start = this._getFormatedDate($.trim(range.split('-')[0] + ' 00:00:00')),
                end = this._getFormatedDate($.trim(range.split('-')[1] + ' 23:59:59'));

            if (start != 'Invalid date' && end != 'Invalid date')
                this.model.set('value', karma.Filter.Property(this.args.property).between(start, end));
        }

        this.notifyFilterChange();
    }
});

// view which will handle check box options
karma.Views.multiChoiceFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplMultiChoiceFilterTemplate',
    hbTemplateName: 'multiChoiceFilterView',

    renderChild: function () {
        var that = this;

        // render the check boxes
        // dump the html
        this.$el.html(karma.hb[this.hbTemplateName]({
            id: 'filter-' + _.uniqueId(),
            label: this.args.label,
            items: this.args.filter.choices
        }));

        // apply the metro look
        setTimeout(function () {
            $(document).initUniform()
        }, 1000);
    },

    bindEvents: function () {
        var that = this;

        // on radio buttons change do search
        $('.checkbox-list input', this.$el).change(function () {
            // this will enforce a search
            that.query = '';
            that._search();
        });
    },

    _search: function () {
        var that = this;

        // reset the filter
        this.model.set('value', null);

        // get all checked input boxes
        $('.checkbox-list input:checked', this.$el).each(function () {
            var $input = $(this);
            if (that.model.get('value')) that.model.set('value', that.model.get('value').Or(karma.Filter.Property(that.args.property).equalTo($input.val())));
            else that.model.set('value', karma.Filter.Property(that.args.property).equalTo($input.val()));
        });

        this.notifyFilterChange();
    }
});

// view which will handle boolean filter view
karma.Views.boolFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplBoolFilterTemplate',
    hbTemplateName: 'boolFilterView',

    renderChild: function () {
        var that = this;
        setTimeout(function () {
            $(document).initUniform()
        }, 1000);
    },

    bindEvents: function () {
        var that = this;

        // on radio buttons change do search
        $('.radio-list input', this.$el).change(function () {
            // this will enforce a search
            that.query = '';
            that._search();
        });
    },

    _search: function () {
        var that = this;

        // reset the filter
        this.model.set('value', null);

        var filterValue = $('.radio-list input:checked', this.$el).val();

        if (!_.isEmpty(filterValue)) {
            this.model.set('value', karma.Filter.Property(this.args.property)['equalTo'](filterValue));
        }

        this.notifyFilterChange();
    }
});

// view which will handle boolean filter view
karma.Views.boolLabeledFilterView = karma.Views.boolFilterView.extend({
    template: '#tmplboolLabeledFilterTemplate',
    hbTemplateName: 'boolLabeledFilterView',

    renderChild: function () {
        var that = this;
        // dump the html
        this.$el.html(karma.hb[this.hbTemplateName]({
            id: 'filter-' + _.uniqueId(),
            label: this.args.label,
            bothLabel: this.args.filter.bothLabel || 'Both',
            trueLabel: this.args.filter.trueLabel || 'True',
            falseLabel: this.args.filter.falseLabel || 'False'
        }));
        setTimeout(function () {
            $(document).initUniform();
        }, 1000);
    },

    bindEvents: function () {
        var that = this;

        // on radio buttons change do search
        $('.radio-list input', this.$el).change(function () {
            // this will enforce a search
            that.query = '';
            that._search();
        });
    },

    _search: function () {
        var that = this;

        // reset the filter
        this.model.set('value', null);

        var filterValue = $('.radio-list input:checked', this.$el).val();

        if (!_.isEmpty(filterValue)) {
            if (_.isBoolean(that.args.filter.comparisonValue)) {
                this.model.set('value', karma.Filter.Property(this.args.property)['equalTo'](filterValue));
            } else if (filterValue === "true")
                this.model.set('value', karma.Filter.Property(this.args.property)['equalTo'](that.args.filter.comparisonValue));
            else
                this.model.set('value', karma.Filter.Property(this.args.property)['notEqualTo'](that.args.filter.comparisonValue));
        }

        this.notifyFilterChange();
    }
});

// view which will handle boolean filter view
karma.Views.numberFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplNumberFilterTemplate',
    hbTemplateName: 'numberFilterView',

    renderChild: function () {
        var that = this;

        // get the reference of the text box
        this.$text = $('.form-control', this.$el);
        this.$text.attr('placeholder', this.args.filter.placeholder || 'Number')
        this.args.filter.width = this.args.filter.width || '180px';
        this.$text.parent().css('width', this.args.filter.width);

        setTimeout(function () {
            $(document).initUniform()
        }, 1000);

        this.$text.TouchSpin({
            buttondown_class: 'btn btn-grey-cascade',
            buttonup_class: 'btn btn-grey-cascade',
            min: that.args.filter.min,
            max: that.args.filter.max,
            stepinterval: that.args.filter.step,
            maxboostedstep: 10000000
        });
    },

    bindEvents: function () {
        var that = this;


        // keyup on search text box (to catch enter)
        this.$text.keyup(function (e) {
            if (e.keyCode != 13) return;
            that._search();
        });

        // on lost focus perform search
        this.$text.blur(function () { that._search(); });

        // on radio buttons change do search
        $('.radio-list input', this.$el).change(function () {
            // this will enforce a search
            that.query = '';
            that._search();
        });

        // btn click events
        $('.btn-grey-cascade', this.$el).click(function () {
            that._search();
        });
    },

    _search: function () {
        // if text box is empty and name filter is not applied, don't do the search
        var term = $.trim(this.$text.val());
        if ((_.isEmpty(term) && !this.model.get('value')) || this.query == term || isNaN(term)) return;

        // store the query
        this.query = term;

        // reset the filter
        this.model.set('value', null);

        if (!_.isEmpty(this.query)) {
            var condition = $('.radio-list input:checked', this.$el).val();
            if (_.isEmpty(condition)) return;
            this.model.set('value', karma.Filter.Property(this.args.property)[condition](parseInt(this.query, 10)));
        }

        this.notifyFilterChange();
    }

});

// view that will render autocomplete

karma.Views.autocompleteFilterView = karma.Views.baseFilterView.extend({
    template: '#tmplAutocompleteFilterTemplate',
    hbTemplateName: 'autocompleteFilterView',

    initializeChild: function () {
        if (!this.args.filter) throw Error('Filter Options are required');
        if (!this.args.filter.collection) throw Error('Collection is required');
        else {
            this.queryCollection = this.args.filter.collection;
        };
        if (!this.args.filter.displayColumn) throw Error('Display Column is required');
        else {
            this.displayColumn = this.args.filter.displayColumn;
        };
        if (!this.args.filter.filterColumn) throw Error('Filter Column is required');
        else {
            this.filterColumn = this.args.filter.filterColumn;
        };
        this.collectionOptions = this.args.filter.collectionOptions || {};

    },

    renderChild: function () {
        var that = this;
        setTimeout(function () {
            $(document).initUniform();
        }, 1000);
        this.bindAutocomplete();
    },
    bindAutocomplete: function () {
        var that = this;

        this.$autocompleteContainer = $('.autocomplete-filter', that.$el);
        // initialize autocomplete plugin
        this.selectEl = $('#autocomplete', this.$autocompleteContainer);

        this.autocomplete = new window.karma.Views.AutocompleteView({
            element: this.selectEl,
            target: this.$autocompleteContainer,
            options: {
                placeholder: 'Enter the name',
                entity: 'Tour',
                itemTemplate: " {{" + that.displayColumn + "}}<div class='clearfix'></div>",
                allowAddNew: false,
                allowViewList: false,
                onSearch: function (searchTerm, responseCallback) {
                    //$('.select2-container', that.$search).removeClass('has-error');

                    //var filter = that.getLocationFilter(searchTerm);

                    var op = {
                        filter: that.collectionOptions.intitialFilter ? Appacitive.Filter.And(that.collectionOptions.intitialFilter, Appacitive.Filter.Property(that.displayColumn).match(searchTerm)) : Appacitive.Filter.Property(that.displayColumn).match(searchTerm),
                        paging: { pnum: 1, psize: 10 },
                        success: function () {
                            var result = [];
                            that.queryCollection.forEach(function (m) {
                                // if (!that.collection.findWhere({ id: m.id })) {
                                result.push(m);
                                //}
                            });
                            responseCallback(result);
                        },
                        reset: true
                    };

                    that.queryCollection.fetchAll($.extend(that.collectionOptions, op));

                },
                onSelect: function (selected) {
                    that.model.set('value', null);
                    if (selected)
                        that.model.set('value', karma.Filter.Property(that.filterColumn)['equalTo'](selected.get('id')));
                    that.notifyFilterChange();
                }

            }
        });

        //$(this.selectEl, this.$search).on('select2-clearing', function () {
        //    that._hideErrorMesage();
        //})

    }
});