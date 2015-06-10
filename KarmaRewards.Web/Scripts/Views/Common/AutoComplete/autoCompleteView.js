// view to render error
window.karma.Views.AutocompleteView = Backbone.View.extend({
    //the element to which auto suggest has to be bound. Should be a hidden input field.
    element: '',

    //the target to fire search query for populating auto suggest field
    target: '',

    //DOM  container of auto suggest element
    container: '',

    //default options
    options:
    {
        entity: '',
        placeholder: "Select a value",
        cssClass: '',
        itemTemplate: '',
        allowAddNew: false,
        allowViewList: false,
        modalContainer: '',
        onLoad: function () {
        },
        onSearch: function (term, responseCallback) {

        },
        onSelect: function (selectedOption) {

        },
        onAdd: function () {

        },
        onViewFullList: function () {

        },
        escapeMarkup: function (m) {
            return m;
        }
    },

    //this function is called after plugin has been loaded
    initialize: function (options) {
        this._validateAutosuggestOptions(options.element, options.target, options.options);
        this.$modalContainer = $(this.options.modalContainer);
        this._bindAutocomplete();
        this._setDropDownListFormat(); // this adds appropriate classes and appends links to dropdown list

        if (this.options.onLoad && _.isFunction(this.options.onLoad)) {
            this.options.onLoad();
        }
        this._bindDropDownEvents();
    },

    _setDropDownListFormat: function () {
        $(this.container).addClass('form-control');
    },

    _bindDropDownEvents: function () {
        var that = this;

        // render popup for adding new entity
        $('.lnkViewFullList', this.target).on('click', function () {
            that.container.select2('close');
            that.options.onViewFullList();
        });

        // render popup for viewing full list
        $('.lnkAddNew', this.target).on('click', function () {
            that.container.select2('close');
            that.options.onAdd();
        });
    },

    _openModal: function (addData) {

        // compile modal template
        if (!karma.hb.commonModalView)
            karma.hb.commonModalView = Handlebars.compile($('#commonModalTemplate').html());

        this.$modalContainer.html(karma.hb.commonModalView(addData));

        // show the modal
        $('.modal', this.$modalContainer).modal({ 'show': true, 'backdrop': 'static' });

    },

    _bindAutocomplete: function () {
        var that = this;

        //set auto-complete plugin options
        var autocomplete_options = this._setAutosuggestOptions();
        //bind auto suggest dropdown
        this.container = $(this.element, this.target).select2(autocomplete_options)
            .on('change', function (val) {
                that.options.onSelect(val.added);
            })
            .parent().find('.select2-container');

    },

    _setAutosuggestOptions: function () {
        var that = this;

        var autocomplete_options = {};
        //  autocomplete_options.data = data;
        autocomplete_options.containerCssClass = this.options.cssClass;
        autocomplete_options.placeholder = this.options.placeholder;
        autocomplete_options.formatInputTooShort = "Start typing to select..";
        if (this.options.itemTemplate && this.options.itemTemplate != '') {

            var itemTemplate = Handlebars.compile(that.options.itemTemplate);

            var format = function (item) {
                return itemTemplate(item.attributes || item);
            };

            // format of results in dropdown list
            autocomplete_options.formatResult = format

            // if needed to format of result selected
            autocomplete_options.formatSelection = format
        }

        var onEnter = function (e) {
            if (e.keyCode === 13) {
                that.container.select2('close');
                that.options.onAdd(e.currentTarget.value);
            }
        };

        if (this.options.allowAddNew) {
            autocomplete_options.formatNoMatches = function () {
                $('.select2-actions').remove();
                var inp = $('#' + $(that.element, that.target).select2('container').find('input').attr('id') + '_search');
                inp.unbind('keyup', onEnter).bind('keyup', onEnter);
                return "Press enter to Add New";
            };
        }

        var both = false;
        if (that.options.allowAddNew && that.options.allowViewList) both = true;

        if (this.options.allowAddNew || this.options.allowViewList) {
            autocomplete_options.formatInputTooShort = function () {
                $('.select2-actions').remove();

                var html = 'Start typing to select. You can also ';

                if (that.options.allowAddNew) {
                    html += '<a class="lnkAddNew pointer"  onclick="window.select2AddNew();">Add New</a>'
                }

                if (both) {
                    html += ' or ';
                }

                if (that.options.allowViewList) {
                    html += '<a class="lnkViewFullList pointer" onclick="window.select2ViewList()">View Full List</a>';
                }

                html = '<div>' + html + '</div>';

                $('#' + $(that.element, that.target).select2('container').find('input').attr('id') + '_search').unbind('onEnter');

                window.select2AddNew = function () {
                    that.container.select2('close')
                    that.options.onAdd();
                    return false;
                };

                window.select2ViewList = function () {
                    that.options.onViewFullList();
                    return false;
                };

                return html;
            }
        }
        autocomplete_options.allowClear = true;

        if (!_.isEmpty(this.options.data)) {
            autocomplete_options.data = this.options.data;
        } else {
            autocomplete_options.minimumInputLength = 2;

            // add debounce to search to reduce number of events fired
            var lazySearch = _.debounce(

                function (query) {
                    $('.select2-actions').remove();

                    that.options.onSearch(query.term, function (results) {
                        $('.select2-actions').remove();

                        var queryData = { results: results };
                        query.callback(queryData);
                        var inp = $('#' + $(that.element, that.target).select2('container').find('input').attr('id') + '_search');
                        inp.unbind('onEnter');
                        if (results && results.length > 0 && inp.val().length > 1) {

                            if (that.options.allowAddNew || that.options.allowViewList) {

                                var html = '<li class="select2-actions">';

                                if (that.options.allowAddNew) {
                                    inp.unbind('keyup', onEnter).bind('keyup', onEnter);
                                    html += 'Press enter to Add New';
                                }

                                if (that.options.allowViewList) {

                                    if (!that.options.allowAddNew) {
                                        html += '<a class="pointer" onclick="select2ViewList();">View Full list</a>';
                                    } else {
                                        html += ' or <a class="lnkViewFullList pointer" onclick="window.select2ViewList()">view full list</a>';
                                    }
                                    window.select2ViewList = function () {
                                        that.container.select2('close');
                                        that.options.onViewFullList();
                                        return false;
                                    };

                                }
                                inp.closest('.select2-drop').find('.select2-results').append(html + '</li>');
                            }
                        }
                    });
                }, 0);

            autocomplete_options.query = function (query) {
                lazySearch(query);
            };

        };
        return autocomplete_options;
    },

    _validateAutosuggestOptions: function (element, target, options) {

        //throws error if auto suggest is not bound to any element
        if (_.isNull(element) || element == '') {
            throw Error("No element defined for auto suggest");
        }
        this.element = element;

        //throws an error if no target is specified to populate data in dropdown list
        if (_.isNull(target) || target == '') {
            throw Error("No source defined for auto suggest options to get populated");
        }
        this.target = target;

        //throws an error if a div for modal is not specified
        if (_.isNull(options.modalContainer) || options.modalContainer == '') {
            throw Error("Please specify a container div for popups");
        }
        this.options.modalContainer = options.modalContainer;

        this.options = $.extend({}, this.options, options);

        //if no placeholder is defined, set it based on entity
        if (options.entity && options.entity != '' && (options.placeholder == null || options.placeholder == '')) {
            this.options.placeholder = "Select a " + this.options.entity;
        }
    }
});