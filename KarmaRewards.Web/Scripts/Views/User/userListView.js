karma.Views.UserListView = karma.Views.GridView.extend({

    detailsRowTemplate: '#tmplUserListDetailsRow',

    initializeChild: function (args) {
        this.options.sorting = {
            sortBy: 'joining_date',
            ascending: karma.config.sortAscending
        };

        this.collection = new karma.Collection.UserCollection();

        this.hbDetailTemplateName = 'tourListDetailRow';

    },
    // method 
    formatDetailsRow: function (obj) {
        return karma.hb.tourListDetailRow(obj);
    }
});

$(document).ready(function () {

    var $container = $('.leader-board-container'),
             args = {
                 showActions: true,
                 allowGlobalSearch: true,
                 allowFilters: true,
                 filter: {
                 },
                 grid: {
                     allowDetails: true,
                     allowSelect: false,
                     allowGroupSelect: false,
                     columns: [
                         {
                             dName: 'Name',
                             binding: 'name',
                             property: 'firstname',
                             allowSort: true,
                             style: '',
                             filter: {
                                 cssClass: 'filter-width-225 name-filter',
                                 type: 'string'
                             }
                         }, {
                             dName: 'Birth Date',
                             binding: 'birthdate',
                             property: 'birthdate',
                             allowSort: true,
                             style: 'width: 160px',
                             format: 'DD-MMM-YYYY',
                             type: 'date'
                         }, {
                             dName: 'Is Enabled',
                             displayColumn: false,
                             binding: 'isenabled',
                             property: 'isenabled',
                             filter: {
                                 cssClass: 'filter-width-225 name-filter',
                                 type: 'boolLabeled',
                                 trueLabel: 'Yes',
                                 falseLabel: 'No'
                             }
                         }, {
                             dName: 'Joining Date',
                             binding: 'joining_date',
                             property: 'joining_date',
                             allowSort: true,
                             style: 'width: 160px',
                             format: 'DD-MMM-YYYY',
                             type: 'date',
                             filter: {
                                 cssClass: 'filter-width-225 name-filter',
                                 type: 'date'
                             }
                         }]
                 }
             };

    new karma.Views.UserListView(args).render($container);
});