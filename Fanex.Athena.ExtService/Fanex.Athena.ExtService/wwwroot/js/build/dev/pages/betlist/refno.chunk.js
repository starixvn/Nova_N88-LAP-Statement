/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	function webpackJsonpCallback(data) {
/******/ 		var chunkIds = data[0];
/******/ 		var moreModules = data[1];
/******/ 		var executeModules = data[2];
/******/
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [];
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(Object.prototype.hasOwnProperty.call(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(data);
/******/
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 		// add entry modules from loaded chunk to deferred list
/******/ 		deferredModules.push.apply(deferredModules, executeModules || []);
/******/
/******/ 		// run deferred modules when all chunks ready
/******/ 		return checkDeferredModules();
/******/ 	};
/******/ 	function checkDeferredModules() {
/******/ 		var result;
/******/ 		for(var i = 0; i < deferredModules.length; i++) {
/******/ 			var deferredModule = deferredModules[i];
/******/ 			var fulfilled = true;
/******/ 			for(var j = 1; j < deferredModule.length; j++) {
/******/ 				var depId = deferredModule[j];
/******/ 				if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 			}
/******/ 			if(fulfilled) {
/******/ 				deferredModules.splice(i--, 1);
/******/ 				result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 			}
/******/ 		}
/******/
/******/ 		return result;
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// Promise = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"wwwroot/js/build/dev/pages/betlist/refno": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push([0,"wwwroot/js/build/dev/pages/lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./www/js/source/components/jquery.flatpopup.js":
/*!******************************************************!*\
  !*** ./www/js/source/components/jquery.flatpopup.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿
!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($) {
    let $blockBody;
    
    const _methods = {
        init: function (inputOptions) {
            const defaultOptions = {
                width: 640,
                height: 480,
                padding: 0,
                id: 'popup-content',
                className: '',
                title: '&nbsp;',
                container: 'body',
                modal: true,
                onOpen: null,
                onOpened: null,
                onClose: null
            };

            $blockBody = $('.popup-block-body');
            const options = $.extend(defaultOptions, inputOptions || {});
            return this.each(function () {
                let $self = $(this);
                const $container = $(options.container);

                if (options.modal && $blockBody.length === 0) {
                    $blockBody = $('<div class="popup-block-body" style="display: none"></div>');
                    $container.append($blockBody);
                }
                
                const $popupElement = $('#' + options.id);
                if ($popupElement.length === 0 || $popupElement.hasClass('flat-popup') === false) {
                    $self.attr('id', options.id);
                    $self.addClass('flat-popup ' + options.className);
                    $self.children().wrapAll('<div class="wrapContent" style="padding:' + options.padding + 'px"></div>');
                    $self.hide()
                         .prepend('<div class="title-bar popup-title"><div class="txt-title">'
                             + options.title
                             + '</div><a class="popup-close" href="javascript:;"><i class="icon-close"></i></a></div>');

                    $self.find('a.popup-close').on('click', function () {
                        if (options.onClose) {
                            options.onClose.apply($self);
                        }

                        if ($self.find('iframe').length > 0) {
                            $self.find('.wrapContent').empty();
                        }
                        _methods.hide.apply($self);
                    });

                    $container.append($self);
                }
                else {
                    $self = $('#' + options.id);
                    $self.find('.txt-title').text(options.title);
                    const $wrapContent = $self.find('.wrapContent');
                    $wrapContent.append($self.children().not('.popup-title'));
                }

                $self.width(options.width);
                $self.height(options.height);

                if (options.onOpen) {
                    options.onOpen.apply($self);
                }

                _methods.show.call($self, options.modal);

                if (options.onOpened) {
                    setTimeout(options.onOpened.apply($self), 350);
                }
            });
        },

        show: function (modal) {
            const $self = this;
            //Set the center alignment padding + border see css style
            const left = ($(window).width() - $self.width()) / 2;
            let top = ($(window).height() - $self.height()) / 2;

            if (top > 300) {
                top -= 100;
            }

            $self.css({
                'top': (top < 0 ? 0 : top) + 'px',
                'left': (left < 0 ? 0 : left) + 'px'
            });

            if (modal === true || modal === undefined) {
                $blockBody.show();
            }

            $self.fadeIn(300);
        },

        hide: function () {
            $(this).fadeOut(300);
            $blockBody.hide();
            return false;
        }
    };

    $.fn.flatPopup = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            // Default to 'init'
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.flatPopup');
        }
    };
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/extensions/prototype.js":
/*!***********************************************!*\
  !*** ./www/js/source/extensions/prototype.js ***!
  \***********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
﻿///
///
/*
* @Alex-1/11/2013
* The below function will work adequatly for the six simple types I mentioned,
* as long as data in the objects and arrays form a tree structure.That is, there isn't
* more than one reference to the same data in the object.
*
*/

//#region String Prototype

/* jshint ignore:start */
String.prototype.format = function () {
    /// <summary>
    /// simply format method
    /// </summary>
    /// <returns type="string">formated string</returns>
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        formatted = formatted.replace("{" + i + "}", arguments[i]);
    }
    return formatted;
};
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, '');
};

String.prototype.ltrim = function () {
    return this.replace(/^\s*/g, '');
};

String.prototype.rtrim = function () {
    return this.replace(/\s*$/g, '');
};
String.prototype.replaceAll = function (token, newToken, ignoreCase) {
    var _token;
    var str = this + "";
    var i = -1;

    if (typeof token === "string") {
        if (ignoreCase) {
            _token = token.toLowerCase();

            while ((
                i = str.toLowerCase().indexOf(
                    token, i >= 0 ? i + newToken.length : 0
                )) !== -1
            ) {
                str = str.substring(0, i) +
                    newToken +
                    str.substring(i + token.length);
            }
        } else {
            return this.split(token).join(newToken);
        }
    }
    return str;
};

//#endregion

//#region Array prototype

Array.prototype.remove = function (value, fieldName) {
    /// <summary>
    ///
    /// Remove specific element in array
    /// </summary>
    /// <param name="value"> element or value of field in object</param>
    /// <param name="fieldName">Name of a field in object</param>
    for (var i = 0; i < this.length; ++i) {
        if (typeof this[i] == 'object' && fieldName !== undefined) {
            if (this[i][fieldName] !== undefined && this[i][fieldName] == value) {
                this.splice(i, 1);
                return;
            }
        } else {
            if (this[i] == value) {
                this.splice(i, 1);
                return;
            }
        }
    }
};
Array.prototype.insertAfter = function (value, after) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="after">position</param>
    this.splice(after + 1, 0, value);
};
Array.prototype.insertBefore = function (value, before) {
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">element</param>
    /// <param name="before">position</param>
    this.splice(before, 0, value);
};

//#endregion

/* jshint ignore:end */

/***/ }),

/***/ "./www/js/source/pages/betlist/betlist.js":
/*!************************************************!*\
  !*** ./www/js/source/pages/betlist/betlist.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed"), __webpack_require__(/*! common/site */ "./www/js/source/common/site.js"), __webpack_require__(/*! common/querystring */ "./www/js/source/common/querystring.js"), __webpack_require__(/*! common/popup */ "./www/js/source/common/popup.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function ($, Site, QueryString, Popup) {
        'use strict';
        const BETTYPE_CASHOUT = 911;
        const BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29, BETTYPE_MIXPARLAY_BETRADAR = 2799;
        const MIN_BETTYPE_FASTMARKET = 221;
        const MAX_BETTYPE_FASTMARKET = 227;
        const SPORT_RNG_GAME = 208;
        const SPORT_RNG_MINIGAME = 209;
        const SPORT_RNG_MOBILE = 210;
        const SPORT_ALLBET = 211;
        const SPORT_VOIDBRIDGE = 212;
        const SPORT_PP = 248;
        const SPORT_GLOBALGAMING = 219;
        const SPORT_CARDCLUB = 165;
        const SPORT_SG = 247;

        let _sortColumnName, _sortOrder;

        function showDetailPopUp(url, option) {
            option.id = 'betlist-popup-detail';
            Popup.openPopupWithIframe(url, option);
        }

        function showResultPopUp(url, option) {
            option.id = 'betlist-popup-result';
            Popup.openPopupWithIframe(url, option);
        }

        function isFastMarket(bettype) {
            return bettype >= MIN_BETTYPE_FASTMARKET && bettype <= MAX_BETTYPE_FASTMARKET;
        }
        function getBaseCurrency() {

            return $('#baseCurrency').val();
        }

        const _me = {
            getSortParams: function () {
                if (!_sortColumnName) {
                    return null;
                }

                return {
                    SortBy: _sortColumnName,
                    SortOrder: _sortOrder
                };
            },
            submiting: true,
            initialize: function () {
                Site.initialize();

                _sortColumnName = _sortColumnName || $('#SortBy').val();
                _sortOrder = _sortOrder || $('#SortOrder').val();

                _me.registerBetListGlobalFunctions();
            },

            getMixParlayResultUrl: function (refno, winlostdate, bettype) {
                let url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, 'refno', refno);
                url = QueryString.setParam(url, 'winlossdate', winlostdate);
                url = QueryString.setParam(url, 'bettype', bettype);
                url = QueryString.setParam(url, 'matchid', 1);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                return url;
            },

            getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
                let url = Site.resolveClientUrl('ResultReport/ResultPopup');
                url = QueryString.setParam(url, 'matchid', matchid);
                url = QueryString.setParam(url, 'bettype', bettype);
                url = QueryString.setParam(url, 'raceno', raceNo);
                url = QueryString.setParam(url, 'sporttype', sporttype);
                url = QueryString.setParam(url, 'isoutright', isOutright);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                if (isFastMarket(bettype)) {
                    url = QueryString.setParam(url, 'handicap1', extension1);
                    url = QueryString.setParam(url, 'handicap2', extension2);
                }
                else {
                    url = QueryString.setParam(url, 'betid', extension1);
                }

                return url;
            },

            viewRngCasinoResult: function (title, refno) {
                let url = Site.resolveClientUrl('ResultReport/ShowRngCasinoSingleWalletResult');

                const params = {
                    'refNo': refno,
                    'lang': $("#lang").val()
                };

                $.ajax({
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: JSON.stringify(params),
                    cache: false,
                    url: url,
                    success: function (response) {
                        const result = response.Result;
                        if (response.Success) {
                            if (result === '') {
                                return;
                            }

                            showResultPopUp(result, { width: 800, height: 380, title: title });
                        }
                        else {
                            alert(Site.getSingleResource('ErrorProcessingMsg'));
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                });
            },

            viewVoidbridgeResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/VoidbridgeResult') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewPPResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/PPResultPopup') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewSGResult: function (custId, gamePlayId, title) {
                let url = Site.resolveClientUrl('ResultReport/SGResultPopup') + '?custid=' + custId + '&gamePlayId=' + gamePlayId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewCardClubResult: function (custId, gameTypeId, transId3rd, title) {
                let url = Site.resolveClientUrl('ResultReport/CCResultPopup') + '?custId=' + custId + '&gameTypeId=' + gameTypeId + '&transId3rd=' + transId3rd;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewAllBetResult: function (trasdesc, betcheck, bettype, title) {
                let url = Site.resolveClientUrl('ResultReport/AllBetResult?transdesc=' +
                    encodeURIComponent(trasdesc) + '&betcheck=' + betcheck + '&bettype=' + bettype);
                url += "&lang=" + $("#lang").val();
                url += "&access_token=" + $("#access_token").val();

                Site.blockUI();
                showResultPopUp(url, { width: 800, height: 380, title: title });
                Site.unBlockUI();
            },


            viewGlobalGamingResult: function (custId, gameId, title) {
                let url = Site.resolveClientUrl('ResultReport/GlobalGamingResult') + '?custid=' + custId + '&gameId=' + gameId;
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                Site.blockUI();
                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },
            viewResult: function (matchid, raceNo, bettype, sporttype, refno,
                username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) {
                const title = Site.getSingleResource('Result');

                if (sporttype === SPORT_RNG_GAME || sporttype === SPORT_RNG_MINIGAME || sporttype === SPORT_RNG_MOBILE) {
                    _me.viewRngCasinoResult(title, refno);
                }
                else if (sporttype === SPORT_VOIDBRIDGE) {
                    _me.viewVoidbridgeResult(custid, extension2, title);
                }
                else if (sporttype === SPORT_PP) {
                    _me.viewPPResult(custid, extension2, title);
                }
                else if (sporttype === SPORT_CARDCLUB) {
                    _me.viewCardClubResult(custid, bettype, extension2, title);
                }
                else if (sporttype === SPORT_SG) {
                    _me.viewSGResult(custid, extension2, title);
                }
                else if (sporttype === SPORT_ALLBET) {
                    _me.viewAllBetResult(transdecs, extension2, bettype, title);
                }
                else if (sporttype === SPORT_GLOBALGAMING) {
                    _me.viewGlobalGamingResult(custid, extension2, title);
                }
                else {
                    const numberBettype = Number(bettype);
                    const isParlay = numberBettype === BETTYPE_MIXPARLAY || numberBettype === BETTYPE_SYSTEMPARLAY || numberBettype === BETTYPE_MIXPARLAY_BETRADAR;
                    const url = isParlay
                        ? _me.getMixParlayResultUrl(refnoMixParlay, winlostdate, numberBettype)
                        : _me.getSportBookResultUrl(matchid, numberBettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

                    const height = 450;
                    const width = 750;

                    showResultPopUp(url, { width: width + 25, height: height + 70, title: title });
                }
            },

            showMixParlayDetail: function (transId, divEvent, custid) {
                let url = Site.resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId + '&custId=' + custid);
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());

                if (divEvent.css('display') === 'none') {
                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            let choice = '';
                            const len = response.length;
                            $.each(response, function (index, item) {
                                choice += item;
                                if (index < len - 1) {
                                    choice += '<div class="line"></div>';
                                }
                            });

                            divEvent.html(choice);
                            divEvent.css('display', '');
                        },
                        beforeSend: function () {
                            Site.blockUI();
                        },
                        complete: function () {
                            Site.unBlockUI();
                        }
                    });
                }
                else {
                    divEvent.css('display', 'none');
                    divEvent.html('');
                }
            },

            showNewParlayDetail: function (transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
                const $divEvent = $('#divEvent_' + transId + '' + transIndex);
                const $nameDiv = $('#combinationOfferName_' + transId + transIndex);
                $divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket

                if ($divEvent.is(':hidden')) {
                    let url = Site.resolveClientUrl('api/MixParlayBetList/NewParlayDetail');
                    url = QueryString.setParam(url, 'transId', transId);
                    url = QueryString.setParam(url, 'refNo', refNo);
                    url = QueryString.setParam(url, 'handicap1', handicap1);
                    url = QueryString.setParam(url, 'handicap2', handicap2);
                    url = QueryString.setParam(url, 'betcheck', betcheck);
                    url = QueryString.setParam(url, 'betteam', betteam);
                    url = QueryString.setParam(url, 'custId', custId);
                    url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                    url = QueryString.setParam(url, 'lang', $("#lang").val());
                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            let length = response.data === null ? 0 : response.data.length;
                            if (length > 0) {
                                let choice = '';
                                $.each(response.texts, function (index, item) {
                                    choice += item;
                                });

                                choice += '<div class="line"></div>';

                                $.each(response.data, function (index, item) {
                                    choice += item;
                                    if (index < length - 1) {
                                        choice += '<div class="line"></div>';
                                    }
                                });

                                $divEvent.html(choice).show();
                                $nameDiv.hide();
                            }
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    });
                }
                else {
                    $divEvent.hide().html('');
                    $nameDiv.show();
                    $('.ui-jqgrid-bdiv').css('height', 'auto');
                }
            },

            showSingleMatchParlayDetail: function (transId, custId, status, transIndex) {
                const $divEvent = $('#divEvent_' + transId + '' + transIndex);

                if ($divEvent.is(':hidden')) {
                    let url = Site.resolveClientUrl('api/SingleMatchParlayBetList/Detail');
                    url = QueryString.setParam(url, 'transId', transId);
                    url = QueryString.setParam(url, 'custId', custId);
                    url = QueryString.setParam(url, 'status', status);
                    url = QueryString.setParam(url, 'access_token', $('#access_token').val());
                    url = QueryString.setParam(url, 'lang', $('#lang').val());

                    $.ajax({
                        type: 'GET',
                        contentType: 'application/json',
                        dataType: 'json',
                        cache: false,
                        url: url,
                        success: function (response) {
                            const length = response === null ? 0 : response.length;
                            if (length > 0) {
                                let choice = '<div class="single-match-parlay-detail">';
                                $.each(response, function (index, item) {
                                    choice += item;
                                });

                                choice += '</div>';
                                $divEvent.html(choice).show();
                            }
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    });
                }
                else {
                    $divEvent.hide().html('');
                    $('.ui-jqgrid-bdiv').css('height', 'auto');
                }
            },

            showSystemParlayDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
                if (Number(transId) === 0) { return; }

                let url = Site.resolveClientUrl('SystemParlayBetList/Detail');
                url = QueryString.setParam(url, 'refno', refNo);
                url = QueryString.setParam(url, 'betteam', betTeam);
                url = QueryString.setParam(url, 'basecurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());

                showDetailPopUp(url, { width: 775, height: 500, title: betTeam });
            },

            showAGCasinoDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('AGCasino') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 870, height: popH + 50, title: title });
            },

            showRnGCasinoDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'hastotal', 1);

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductCS') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },

            showVoidBridgeDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showAllBetDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LC_Detail');
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductAB') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 870, height: popH + 50, title: title });
            },
            showColossusBetsDetails: function (refNo, winlostDate, transId, custId, betCheck) {
                let url = Site.resolveClientUrl('ColossusBetBetList/Detail');
                url = QueryString.setParam(url, 'refNo', refNo);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'transid', transId);
                url = QueryString.setParam(url, 'custid', custId);
                url = QueryString.setParam(url, 'betCheck', betCheck);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('TicketDetailOf').replace('{0}', refNo);

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showGlobalGamingDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'haslicWl', 1);

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductFW') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showPPDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductPP') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            showSGDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductSG') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            ViewCasinoHandHistory: function (refNo) {
                let url = Site.resolveClientUrl('HandHistory/RNGCasino');
                const urlParam = $('#casino-hand-history-' + refNo).attr('url-param');

                url += urlParam;
                Site.redirect(url);
            },

            viewCashOutHistory: function (transId, refNo, betType, betdaqId) {
                const baseCurrency = $('#baseCurrency').val();

                const isCashOut = betdaqId < 1 || betType === BETTYPE_CASHOUT;
                let url = Site.resolveClientUrl('CashOutBetList/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'refNo', refNo);
                url = QueryString.setParam(url, 'betType', betType);
                url = QueryString.setParam(url, 'betdaqId', betdaqId);
                url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'isCashout', isCashOut);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                const popH = Math.min($(window).height() - 100, 550);
                const title = isCashOut ? Site.getSingleResource('CashOutDetails') : Site.getSingleResource('BoughtDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },

            showBonusDetails: function (refCode, transId) {
                const baseCurrency = $('#baseCurrency').val();

                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'RefCode', refCode);
                url = QueryString.setParam(url, 'TransId', transId);
                url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_LicenseeBonus_Detail');
                url = QueryString.setParam(url, 'haslicWl', 1);

                const popH = Math.min($(window).height() - 100, 550), popW = 840;
                const title = Site.getSingleResource('TransactionDetail');

                showDetailPopUp(url, { width: popW + 50, height: popH + 50, title: title });
            },

            showVideoPokerGameDetails: function (transId, winlostDate, custId, transType) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'transType', transType);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_RNG_Detail');

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductCC') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },
            registerBetListGlobalFunctions: function () {
                window.ViewResult = _me.viewResult;
                // showMP and showCombMP are only for old tickets
                window.showMP = function (transId, refNo, custId) {
                    const divEvent = $('#divEvent_' + transId);
                    divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent, custId);
                };

                window.showCombMP = function (transId, index, refNo, custId) {
                    const divEvent = $('#divEvent_' + transId + '' + index);
                    divEvent.addClass('mx-parlay'); //to make line-through for void-parlay-ticket
                    _me.showMixParlayDetail(transId, divEvent, custId);
                };

                window.showNewCombMP = _me.showNewParlayDetail;
                window.showCombMPDetail = _me.showSystemParlayDetail;
                window.ViewCashOutHistory = _me.viewCashOutHistory;
                window.showColossusBetsDetails = _me.showColossusBetsDetails;
                window.showAGCasinoDetails = _me.showAGCasinoDetails;
                window.showRnGCasinoDetails = _me.showRnGCasinoDetails;
                window.showVoidBridgeDetails = _me.showVoidBridgeDetails;
                window.showVoidbridgeResult = _me.viewVoidbridgeResult;
                window.showTodayRunningBetsDetails = _me.showAllBetDetails;
                window.showGlobalGaming = _me.showGlobalGamingDetails;
                window.ShowBonusDetails = _me.showBonusDetails;
                window.showPPDetails = _me.showPPDetails;
                window.showVideoPokerGameDetails = _me.showVideoPokerGameDetails;
                window.showSingleMatchParlayDetail = _me.showSingleMatchParlayDetail;
                window.showSGDetails = _me.showSGDetails;

                const betList = {
                    ViewCasinoHandHistory: _me.ViewCasinoHandHistory
                };
                window.BetList = betList;
            }

        };

        return _me;
    }).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ "./www/js/source/pages/betlist/refno.js":
/*!**********************************************!*\
  !*** ./www/js/source/pages/betlist/refno.js ***!
  \**********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

/* WEBPACK VAR INJECTION */(function($) {var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;﻿!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(/*! ./betlist */ "./www/js/source/pages/betlist/betlist.js")], __WEBPACK_AMD_DEFINE_RESULT__ = (function (BetList) {
    'use strict';

    $(function () {
        BetList.initialize();
    });

  
}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js-exposed")))

/***/ }),

/***/ 0:
/*!****************************************************!*\
  !*** multi ./www/js/source/pages/betlist/refno.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! ./www/js/source/pages/betlist/refno.js */"./www/js/source/pages/betlist/refno.js");


/***/ })

/******/ });
//# sourceMappingURL=refno.chunk.js.map