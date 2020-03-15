///
define(['jquery'], function ($) {
    'use strict';

    function parseQueryString(queryString) {
        const args = queryString.split('&');
        const argsList = {};

        for (let i = 0; i < args.length; i++) {
            const arg = unescape(args[i]);
            if (arg.length > 0) {
                if (arg.indexOf('=') === -1) {
                    argsList[$.trim(arg.toLowerCase())] = null;
                }
                else {
                    var keyValuePair = arg.split('=');
                    argsList[$.trim(keyValuePair[0].toLowerCase())] = $.trim(keyValuePair[1]);
                }
            }
        }

        return argsList;
    };

    function getQueryString(url) {
        const index = url.indexOf("?");
        return index < 0 ? "" : url.substring(index + 1, url.length);
    };

    function getRawUrl(url) {
        const index = url.indexOf("?");
        return index < 0 ? url : url.substring(0, index);
    };

    function setParam(url, name, value) {
        const rawUrl = getRawUrl(url);
        const queryString = getQueryString(url);
        const args = parseQueryString(queryString);

        if (name == null) {
            return url;
        }

        // Update value
        let found = false;
        for (let key in args) {
            if (key != null && key.toLowerCase() === name.toLowerCase()) {
                // Does not update data if value = undefined
                if (value !== undefined) {
                    args[key] = value;
                }
                found = true;
            }
        }

        if (!found) {
            args[name.toLowerCase()] = value;
        }

        const queryArray = [];

        // Rebuild query string, ignore null value
        for (let key1 in args) {
            if (args[key1] != null && args[key1] !== undefined) {
                queryArray.push(key1.toLowerCase() + '=' + args[key1]);
            }
        }

        return rawUrl + '?' + queryArray.join('&');
    }

    function setParams(url, parameters) {
        let returnUrl = url;
        for (let property in parameters) {
            if (parameters.hasOwnProperty(property)) {
                returnUrl = setParam(returnUrl, property, parameters[property]);
            }
        }

        return returnUrl;
    }

    return {
        getQueryString: getQueryString,
        getRawUrl: getRawUrl,
        setParam: setParam,
        setParams: setParams,
        getParam: function (url, name) {
            let queryString = getQueryString(url);
            const args = parseQueryString(queryString);

            for (let key in args) {
                if (key != null && key.toLowerCase() === name.toLowerCase()) {
                    return args[key];
                }
            }

            return null;
        }
    };
});