// initialize the name space
window.karma = window.karma || {};

// referencing filter from appacitive SDK
window.karma.Filter = Appacitive.Filter;

$(function () {
    Appacitive.initialize({
        apikey: "101010101010",
        env: window.karma.config.environment,
        appId: "1010101010"
    });


    // change the API URL which is used by SDK
    Appacitive.config.apiBaseUrl = window.karma.config.apiBaseUrl;
});