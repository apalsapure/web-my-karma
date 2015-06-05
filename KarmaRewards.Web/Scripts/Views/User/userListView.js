karma.Views.UserListView = karma.Views.GridView.extend({

    detailsRowTemplate: '#tmplUserListDetailsRow',

    initializeChild: function (args) {
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
                     isPublished: hfl.viewPublished === '1',
                     enabledOnly: hfl.viewEnabledOnly === '1',
                     ownerId: hfl.ownerId,
                     ownerType: hfl.ownerType
                 },
                 grid: {
                     allowDetails: true,
                     allowSelect: false,
                     allowGroupSelect: false,
                     columns: [
                         {
                             dName: 'Employee Name',
                             binding: 'name',
                             property: 'firstname',
                             allowSort: true,
                             style: '',
                             filter: {
                                 cssClass: 'filter-width-225 name-filter',
                                 type: 'string'
                             }
                         }]
                 }
             };

    new karma.Views.UserListView(args).render($container);
});