// central location for all configuration used across the application
window.karma.config = new (function () {
    var _apiBaseUrl = 'http://localhost:5576/api/api.relay/';
    var _environment = 'sandbox';
    var _sortBy = '__utclastupdateddate';
    var _sortAscending = false;
    var _pnum = 1;
    var _psize = 10;
    
    this.apiBaseUrl = _apiBaseUrl;
    this.environment = _environment;
    this.sortBy = _sortBy;
    this.sortAscending = _sortAscending;
    this.pnum = _pnum;
    this.psize = _psize;
})();