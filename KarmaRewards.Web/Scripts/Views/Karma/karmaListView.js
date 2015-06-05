karma.Views.KarmaListView = karma.Views.GridView.extend({

    detailsRowTemplate: '#tmplKarmaListDetailsRow',

    initializeChild: function (args) {
        this.collection = new karma.Collection.UserCollection();

        this.hbDetailTemplateName = 'tourListDetailRow';

    }
});

$(document).ready(function () {

    var $container = $('.content-wrapper'),
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
                             email: 'Email Address',
                             binding: 'email',
                             property: 'email',
                             allowSort: true,
                             style: ''
                         }]
                 }
             };

    new karma.Views.KarmaListView(args).render($container);
});