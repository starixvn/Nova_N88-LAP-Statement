/*global _language_en_US*/
define(['jquery', 'common/querystring', 'common/popup', 'extensions/prototype',],
    function ($, QueryString, Popup) {// jshint ignore:line
        'use strict';
        let REQUEST_VERIFICATION_TOKEN;
        let _rootUrl = null;

        function setProductCodeListParam(url, productCodeList) {
            return QueryString.setParam(url, 'ProductCodeList', productCodeList);
        }

        function reload() {
            window.location.reload();
        }

        function resolveClientUrl(relativeUrl) {
            return _rootUrl + relativeUrl;
        }
        function clone(obj) {
            /// <summary>
            ///
            /// The below function will work adequatly for the six simple types I mentioned,
            /// as long as data in the objects and arrays form a tree structure.That is, there isn't
            /// more than one reference to the same data in the object.
            /// Handle the 3 simple types, and null or undefined
            /// </summary>
            /// <param name="obj"></param>
            /// <returns type=""></returns>

            if (null == obj || 'object' !== typeof obj) {
                return obj;
            }
            // Handle Date
            if (obj instanceof Date) {
                const date = new Date();
                date.setTime(obj.getTime());
                return date;
            }

            // Handle Array
            if (obj instanceof Array) {
                const array = [];
                for (let i = 0, len = obj.length; i < len; i++) {
                    array[i] = clone(obj[i]);
                }
                return array;
            }

            // Handle Object
            if (obj instanceof Object) {
                const copy = {};
                for (let attr in obj) {
                    if (obj.hasOwnProperty(attr)) {
                        copy[attr] = clone(obj[attr]);
                    }
                }
                return copy;
            }

            throw new Error('Unable to copy obj! Its type isn\'t supported.');
        }
        function initialize() {
            REQUEST_VERIFICATION_TOKEN = $('input[name=__lap_ext_service__]').val();
            initEvent();
            _rootUrl = $('#RootUrl').val();
        }
        function initEvent() {
            $.ajaxSetup({
                cache: false,
                headers: {
                    'X-CSRF-TOKEN-LAP-EXT': REQUEST_VERIFICATION_TOKEN,
                    "Authorization": "Bearer " + $("#access_token").val()
                },
                statusCode: {
                    0: function () {
                        unBlockUI();
                    },
                    400: function () {
                        unBlockUI();
                        alert(getSingleResource('InvalidRequestMsg'));
                    },
                    401: function () {
                        window.location.href = resolveClientUrl('Error/Index?Code=401');
                    },
                    403: function () {
                        unBlockUI();
                        alert(getSingleResource('HttpError_403'));
                    },
                    500: function () {
                        unBlockUI();
                        alert(getSingleResource('ErrorProcessingMsg'));
                    }
                }
            });
        }
     
        function unBlockUI() {
            $('div.block-body').hide();
            $('div.loading').hide();
        }
        function blockUI() {
            $('div.block-body').show();
            $('div.loading').show();
        }
        function getSingleResource(enghlishName) {
            if (enghlishName == null) {
                return enghlishName;
            }

            const keyName = '_language_' + _pageLanguage.replace('-', '_');

            let resources = window[keyName];
            if (!resources) {
                resources = window._language_en_US; // jshint ignore:line
            }

            const mapResource = function (key) {
                let item = resources[key];
                if (item === undefined) {
                    item = window._language_en_US[key]; // jshint ignore:line
                    return item !== undefined ? item : key;
                } else {
                    return item;
                }
            };
            if (enghlishName.indexOf('@') > -1) {
                const keys = enghlishName.match(/\@{1}\[{1}[a-zA-Z\d\_]+\]{1}/g);
                if (keys != null) {
                    let temp = enghlishName;
                    for (var i = 0; i < keys.length; i++) {
                        var realKey = keys[i].replace('@[', '').replace(']', '');
                        temp = temp.replace(keys[i], mapResource(realKey));
                    }
                    return temp;
                }
                return mapResource(enghlishName);
            }
            else {
                return mapResource(enghlishName);
            }
        }

        function getResources(arrayText) {
            if (arrayText == null) {
                return null;
            }
            if (arrayText.constructor !== Array) {
                return null;
            }

            const newArray = [];
            for (let i = 0; i < arrayText.length; i++) {
                newArray.push(getSingleResource(arrayText[i]));
            }
            return newArray;
        }

        function redirect(url) {
            window.location.href = url;
        }

        function getCurrentUrl() {
            return window.location.href;
        }

        function getLocation() {
            return window.location;
        }
      
        function getXsrfToken() {
            return REQUEST_VERIFICATION_TOKEN;
        }

        return {
            initialize: initialize,
            setProductCodeListParam: setProductCodeListParam,
            reload: reload,
            resolveClientUrl: resolveClientUrl,
            unBlockUI: unBlockUI,
            blockUI: blockUI,
            clone: clone,
            getSingleResource: getSingleResource,
            getResources: getResources,
            redirect: redirect,
            getCurrentUrl: getCurrentUrl,
            getLocation: getLocation,
            getXsrfToken: getXsrfToken,
        };
    });