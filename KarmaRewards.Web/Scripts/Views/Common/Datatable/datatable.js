var Datatable = function () {

    var tableOptions, dataTable, table, tableContainer, tableWrapper, tableInitialized = false, ajaxParams = {}, rowCount = 0, selectedRowCount = 0, groupCheckbox, that;

    var countSelectedRecords = function () {
        selectedRowCount = $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).size();
    };

    return {
        //main function to initiate the module
        init: function (options) {

            if (!$().dataTable) { return; }

            that = this;

            // default settings
            options = $.extend(true, {
                src: "", // actual table  
                filterApplyAction: "filter",
                filterCancelAction: "filter_cancel",
                resetGroupActionInputOnSuccess: true,
                loadingMessage: 'Loading...',
                dataTable: {
                    "dom": "<'data-tables't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>", // datatable layout
                    "pageLength": karma.config.psize, // default records per page
                    "language": { // language settings
                        // metronic spesific
                        "metronicGroupActions": "_TOTAL_ records selected. ",
                        "metronicAjaxRequestGeneralError": "Could not complete request. Please check your internet connection",

                        // data tables spesific
                        "lengthMenu": "<span class='seperator'>|</span>View _MENU_ records",
                        "info": "<span class='seperator'>|</span>Found total _TOTAL_ record(s)",
                        "infoEmpty": "",
                        "emptyTable": "No records found.",
                        "zeroRecords": "No matching records found",
                        "paginate": {
                            "previous": "Prev",
                            "next": "Next",
                            "last": "Last",
                            "first": "First",
                            "page": "Page",
                            "pageOf": "of"
                        }
                    },

                    "lengthMenu": [
                        karma.config.allowedPageSizes,
                        karma.config.allowedPageSizes // change per page values here 
                    ],

                    "orderCellsTop": true,

                    "pagingType": "bootstrap_extended", // pagination type(bootstrap, bootstrap_full_number or bootstrap_extended)
                    "autoWidth": false, // disable fixed width and enable fluid table
                    "processing": false, // enable/disable display message box on record load
                    "serverSide": true, // enable/disable server side ajax loading

                    "drawCallback": function (oSettings) { // run some code on table redraw
                        $('thead', table).first().find('th').each(function (i, el) {
                            if (_.isEmpty($(el).html().trim()) && $(el).prev().length > 0) {
                                $(el).prev().addClass('no-right-border');
                            }
                        })
                        if (tableInitialized === false) { // check if table has been initialized
                            tableInitialized = true; // set table initialized
                            table.closest('.data-table-container').removeClass('hide'); // display table
                        }

                        // set the row count
                        setTimeout(function () {
                            groupCheckbox = $('.group-checkable', table);
                        }, 100);
                        rowCount = $('tbody > tr', table).size();

                        //Metronic.initUniform($('input[type="checkbox"]', table)); // reinitialize uniform check boxes on each table reload
                        countSelectedRecords(); // reset selected records indicator
                    },

                    "fnCreatedRow": function (nRow, aData, iDataIndex) {
                        if (_.isFunction(options.onRowCreate)) options.onRowCreate.apply(this, arguments);
                    }
                }
            }, options);

            tableOptions = options;

            // create table's jquery object
            table = $(options.src);
            tableContainer = table.parents(".table-container");

            // apply the special class that used to restyle the default data table
            var tmp = $.fn.dataTableExt.oStdClasses;

            $.fn.dataTableExt.oStdClasses.sWrapper = $.fn.dataTableExt.oStdClasses.sWrapper + " dataTables_extended_wrapper";
            $.fn.dataTableExt.oStdClasses.sLengthSelect = "form-control input-xsmall input-sm input-inline";

            // initialize a data table
            dataTable = table.DataTable(options.dataTable);

            // revert back to default
            $.fn.dataTableExt.oStdClasses.sWrapper = tmp.sWrapper;
            $.fn.dataTableExt.oStdClasses.sLengthSelect = tmp.sLengthSelect;

            // get table wrapper
            tableWrapper = table.parents('.dataTables_wrapper');

            // build table group actions panel
            if ($('.table-actions-wrapper', tableContainer).size() === 1) {
                $('.table-group-actions', tableWrapper).html($('.table-actions-wrapper', tableContainer).html()); // place the panel inside the wrapper
                $('.table-actions-wrapper', tableContainer).remove(); // remove the template container
            }

            // change drop down to select2
            tableWrapper.find('.dataTables_length select').select2();

            // handle row's checkbox click
            table.on('change', 'tbody > tr > td:nth-child(1) input[type="checkbox"]', function (e) {
                countSelectedRecords();

                if (options.allowSingleSelect && selectedRowCount > 1) {
                    $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).each(function (i, ele) {
                        if ($(ele).val() == $(e.currentTarget).val()) return;
                        if ($(ele).is(':disabled')) return;
                        $(ele).removeAttr('checked');
                    });
                    countSelectedRecords();
                }


                if (_.isFunction(options.onSelectChange)) {
                    options.onSelectChange($(this).val(), $(this).is(':checked'));
                }

                if (rowCount != selectedRowCount) groupCheckbox.removeAttr('checked');
                else groupCheckbox.attr('checked', 'checked');
                $.uniform.update(groupCheckbox);
                $.uniform.update($('tbody > tr > td:nth-child(1) input[type="checkbox"]'));
            });

            // if format function is provided then only render the details
            if (_.isFunction(options.format)) {
                table.on('click', 'tbody > tr> td span.row-details', function () {
                    $(this).toggleClass('row-details-open');
                    var tr = $(this).closest('tr');
                    var row = dataTable.row(tr);

                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('shown');
                    }
                    else {
                        var html = options.format(row.data());
                        row.child(html);
                        if (options.replaceRow) {
                            row.child().html(html);
                        }
                        // Open this row
                        row.child.show();
                        if (!options.replaceRow)
                            tr.addClass('shown');
                        else
                            tr.addClass('font-16').find('td').addClass('pad15-top pad15-bottom');
                        if (_.isFunction(tableOptions.onRowExpand))
                            tableOptions.onRowExpand(tr, row);
                    }
                });
            }
        },

        bindGroupSelect: function () {
            // handle group check boxes check/ uncheck
            $('.group-checkable', table).unbind('change').change(function () {
                var set = $('tbody > tr > td:nth-child(1) input[type="checkbox"]', table);
                var checked = $(this).is(":checked");
                $(set).each(function () {
                    if (_.isFunction(tableOptions.onSelectChange)) {
                        tableOptions.onSelectChange($(this).val(), checked);
                    }
                    $(this).attr("checked", checked);
                });
                $.uniform.update(set);
                countSelectedRecords();
            });
        },

        setSelectedItemCount: function (count) {
            var text = tableOptions.dataTable.language.metronicGroupActions;
            if (selectedRowCount > 0) {
                $('.table-group-actions > span', table.closest('.data-table-container')).text(text.replace("_TOTAL_", count));
            } else {
                $('.table-group-actions > span', table.closest('.data-table-container')).text("");
            }
        },

        getSelectedRowsCount: function () {
            return $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).size();
        },

        getSelectedRows: function () {
            var rows = [];
            $('tbody > tr > td:nth-child(1) input[type="checkbox"]:checked', table).each(function () {
                rows.push($(this).val());
            });
            return rows;
        },

        getDataTable: function () {
            return dataTable;
        },

        getTableWrapper: function () {
            return tableWrapper;
        },

        gettableContainer: function () {
            return tableContainer;
        },

        getTable: function () {
            return table;
        }
    };

};