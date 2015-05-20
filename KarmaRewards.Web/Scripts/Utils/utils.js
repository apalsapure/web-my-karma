(function (global) {
    global.utils = {};
    global.utils.escapeAppacitiveSpecialChars = function (strText) {
        strText = strText || '';
        return strText.replace(/([%^!~\[\]()":])/g, '\\$1').replace(/([/])/g, '/$1');
    };
})(window.karma);