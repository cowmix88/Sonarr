'use strict';
define(
    [
        'handlebars',
        'System/StatusModel',
        'underscore'
    ], function (Handlebars, StatusModel, _) {
        Handlebars.registerHelper('poster', function () {

            var placeholder = StatusModel.get('urlBase') + '/Content/Images/poster-dark.png';
            var poster = _.where(this.images, {coverType: 'poster'});

            if (poster[0]) {
                return new Handlebars.SafeString('<img class="series-poster" src="{0}" {1}>'.format(poster[0].url, Handlebars.helpers.defaultImg.call()));
            }

            return new Handlebars.SafeString('<img class="series-poster placeholder-image" src="{0}">'.format(placeholder));
        });

        Handlebars.registerHelper('traktUrl', function () {
            return 'http://trakt.tv/show/' + this.titleSlug;
        });

        Handlebars.registerHelper('imdbUrl', function () {
            return 'http://imdb.com/title/' + this.imdbId;
        });

        Handlebars.registerHelper('tvdbUrl', function () {
            return 'http://www.thetvdb.com/?tab=series&id=' + this.tvdbId;
        });

        Handlebars.registerHelper('tvRageUrl', function () {
            return 'http://www.tvrage.com/shows/id-' + this.tvRageId;
        });

        Handlebars.registerHelper('route', function () {
            return StatusModel.get('urlBase') + '/series/' + this.titleSlug;
        });

        Handlebars.registerHelper('percentOfEpisodes', function () {
            var episodeCount = this.episodeCount;
            var episodeFileCount = this.episodeFileCount;

            var percent = 100;

            if (episodeCount > 0) {
                percent = episodeFileCount / episodeCount * 100;
            }

            return percent;
        });

        Handlebars.registerHelper('seasonCountHelper', function () {
            var seasonCount = this.seasonCount;
            var continuing = this.status === 'continuing';

            if (continuing) {
                return new Handlebars.SafeString('<span class="label label-info">Season {0}</span>'.format(seasonCount));
            }

            if (seasonCount === 1) {
                return new Handlebars.SafeString('<span class="label label-info">{0} Season</span>'.format(seasonCount));
            }

            return new Handlebars.SafeString('<span class="label label-info">{0} Seasons</span>'.format(seasonCount));
        });

        Handlebars.registerHelper('titleWithYear', function () {
            if (this.title.endsWith(' ({0})'.format(this.year))) {
                return this.title;
            }

            if (!this.year) {
                return this.title;
            }

            return new Handlebars.SafeString('{0} <span class="year">({1})</span>'.format(this.title, this.year));
        });
    });
