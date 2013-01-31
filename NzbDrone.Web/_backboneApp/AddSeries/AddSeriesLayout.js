﻿/// <reference path="../app.js" />
/// <reference path="AddNewSeries/AddNewSeriesView.js" />
/// <reference path="RootDir/RootDirView.js" />
/// <reference path="../Quality/qualityProfileCollection.js" />
/// <reference path="../Shared/SpinnerView.js" />
/// <reference path="ImportExistingSeries/ImportSeriesView.js" />

NzbDrone.AddSeries.AddSeriesLayout = Backbone.Marionette.Layout.extend({
    template: 'AddSeries/addSeriesLayoutTemplate',

    regions: {
        addNew: '#add-new',
        importExisting: '#import-existing',
        rootFolders: '#root-folders'
    },

    ui: {
        addNewTab: '.x-add-new-tab',
        importExistingTab: '.x-import-existing-tab',
        rootFoldersTab: '.x-root-folders-tab'
    },


    events: {
        'click .x-add-new-tab': 'showAddNew',
        'click .x-import-existing-tab': 'showImport',
        'click .x-root-folders-tab': 'showRootFolders'
    },

    showAddNew: function (e) {
        if (e) {
            e.preventDefault();
        }

        this.ui.addNewTab.tab('show');
        NzbDrone.Router.navigate('series/add/new');

    },

    showImport: function (e) {
        if (e) {
            e.preventDefault();
        }

        this.ui.importExistingTab.tab('show');
        NzbDrone.Router.navigate('series/add/import');
    },

    showRootFolders: function (e) {
        if (e) {
            e.preventDefault();
        }

        this.ui.rootFoldersTab.tab('show');
        NzbDrone.Router.navigate('series/add/rootfolders');
    },

    rootFolderCollection: new NzbDrone.AddSeries.RootDirCollection(),
    qualityProfileCollection: new NzbDrone.Quality.QualityProfileCollection(),


    initialize: function (context, action, query) {
        if (action) {
            this.action = action.toLowerCase();
        }

        if (query) {
            this.query = query.toLowerCase();
        }
    },

    onRender: function () {

        this.qualityProfileCollection.fetch();

        this.addNew.show(new NzbDrone.AddSeries.AddNewSeriesView({ rootFolders: this.rootFolderCollection, qualityProfiles: this.qualityProfileCollection }));
        this.importExisting.show(new NzbDrone.AddSeries.ExistingFolderListView({ collection: this.rootFolderCollection }));
        this.rootFolders.show(new NzbDrone.AddSeries.RootDirView({ collection: this.rootFolderCollection }));

        NzbDrone.vent.listenTo(this.rootFolderCollection, 'add', this.evaluateActions, this);
        NzbDrone.vent.listenTo(this.rootFolderCollection, 'remove', this.evaluateActions, this);
        NzbDrone.vent.listenTo(this.rootFolderCollection, 'reset', this.evaluateActions, this);
    },

    onShow: function () {
        switch (this.action) {
            case 'import':
                this.showImport();
                break;
            case 'rootfolders':
                this.showRootFolders();
                break;
            default:
                this.showAddNew();
        }
    },

    evaluateActions: function () {
        if (this.rootFolderCollection.length === 0) {
            this.ui.addNewTab.hide();
            this.ui.importExistingTab.hide();
            this.showRootFolders();
        } else {
            this.ui.addNewTab.show();
            this.ui.importExistingTab.show();
        }
    },


});