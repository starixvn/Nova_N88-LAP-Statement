define(
    ['jquery', 'common/site', 'common/querystring', 'common/popup'],
    function ($, Site, QueryString, Popup) {
        'use strict';
        const BETTYPE_CASHOUT = 911;
        const BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29, BETTYPE_MIXPARLAY_BETRADAR = 2799;
        const MIN_BETTYPE_FASTMARKET = 221;
        const MAX_BETTYPE_FASTMARKET = 227;
        const SPORT_RNG_GAME = 208;
        const SPORT_RNG_MINIGAME = 209;
        const SPORT_RNG_MOBILE = 210;
        const SPORT_PP = 248;
        const SPORT_CARDCLUB = 165;
        const SPORT_VRG = 245;
        const SPORT_SXG = 243;
        const SPORT_HAPPY5LINKEDGAMES = 168;
        const SPORT_SPORTSLOTTERY = 169;
        const SPORT_VGA = 171;
        const SPORT_VGA_FreeBets = 173;
        const SPORT_VS3 = 231;

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
                    url = QueryString.setParam(url, 'transdesc', extension2);
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

            viewVRGResult: function (gameId, platform, title) {
                let url = Site.resolveClientUrl('ResultReport/VirtualGamesResultPopup') + '?gameId=' + gameId + '&platform=' + platform;
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
            
            viewVGamingResult: function (custId, gameId, siteId, gameRoundId, title, sporttype, currencyName) {
                let url = Site.resolveClientUrl('ResultReport/VGamingResultPopup') + '?custId=' + custId + '&gameId=' + gameId + '&siteId=' + siteId + '&gameRoundId=' + gameRoundId + '&sportType=' + sporttype + '&currencyName=' + currencyName;
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

            viewThirdPartyProductResult: function (custId, transId3rd, gameId3rd, currencyId, siteId, betTypeId, productPath, title) {
                const param = 'custId=' + custId +
                    '&transId3rd=' + transId3rd +
                    '&gameId3rd=' + gameId3rd +
                    '&siteId=' + siteId +
                    '&currencyId=' + currencyId +
                    '&betTypeId=' + betTypeId +
                    '&productPath=' + productPath;
                const url = Site.resolveClientUrl('ResultReport/ThirdPartyProduct?' + param);
                Site.blockUI();

                $.get(url, function (result) {
                    if (result !== '') {
                        showResultPopUp(result, { width: 800, height: 380, title: title });
                        Site.unBlockUI();
                    }
                });
            },

            viewVS3Result: function (transdecs, sporttype, title) {
                const param = 'transDesc=' + transdecs +
                    '&sporttype=' + sporttype +
                    '&lang=' + $("#lang").val() +
                    '&access_token=' + $("#access_token").val();

                let url = Site.resolveClientUrl('ResultReport/ResultPopup?' + param);

                Site.blockUI();
                showResultPopUp(url, { width: 800, height: 380, title: title });
                Site.unBlockUI();
            },

            viewResult: function (matchid, raceNo, bettype, sporttype, refno,
                currency, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs, siteId) {
                const title = Site.getSingleResource('Result');
                const defaultPlatform = 'd';

                if (sporttype === SPORT_RNG_GAME || sporttype === SPORT_RNG_MINIGAME || sporttype === SPORT_RNG_MOBILE) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'RNGCasino', title);
                }
                else if (sporttype === SPORT_PP) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'PP', title);
                }
                else if (sporttype === SPORT_CARDCLUB) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'CardClub', title);
                }
                else if (sporttype === SPORT_VRG) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'VirtualGames', title);
                }
                else if (sporttype === SPORT_HAPPY5LINKEDGAMES) {
                    _me.viewThirdPartyProductResult(custid, refno, extension2, extension1, siteId, bettype, 'Happy5LinkedGames', title);
                }
                else if (sporttype === SPORT_SPORTSLOTTERY) {
                    _me.viewThirdPartyProductResult(custid, transdecs, extension2, extension1, siteId, bettype, 'SportsLottery', title);
                }
                else if (sporttype === SPORT_VGA || sporttype === SPORT_VGA_FreeBets) {
                    _me.viewVGamingResult(custid, bettype, extension2, transdecs, title, sporttype, currency);
                }
                else if (sporttype === SPORT_VS3) {
                    _me.viewVS3Result(transdecs, sporttype, title);
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

            ViewCasinoHandHistory: function (refNo) {
                let url = Site.resolveClientUrl('HandHistory/RNGCasino');
                const urlParam = $('#casino-hand-history-' + refNo).attr('url-param');

                url += urlParam;
                Site.redirect(url);
            },

            showVGamingJackpotWonDetails: function (transId, custId, betTypeId) {
                let url = Site.resolveClientUrl('BetListDetailByProduct/Detail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'betType', betTypeId);
                url = QueryString.setParam(url, 'BaseCurrency', getBaseCurrency());
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());
                url = QueryString.setParam(url, 'uspkey', 'Lrf_BetList_Jackpot_Detail');
                url = QueryString.setParam(url, 'hastotal', 1);
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVGA') + ' - ' +
                    Site.getSingleResource('JackpotWonDetails');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
            },

            showVGamingDetails: function (transId, winlostDate, custId, transType) {
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
                url = QueryString.setParam(url, 'isDesktopViewOnly', $("#isDesktopViewOnly").val() === "1");
                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('ProductVGA') + ' - ' +
                    Site.getSingleResource('DetailedBetlist');

                showDetailPopUp(url, { width: 890, height: popH + 50, title: title });
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

            ViewNewCashOutHistory: function (transId, custId, winlostDate, currency) {
                const baseCurrency = $('#baseCurrency').val();

                let url = Site.resolveClientUrl('CashOutBetList/NewDetail');
                url = QueryString.setParam(url, 'transId', transId);
                url = QueryString.setParam(url, 'custId', custId);
                url = QueryString.setParam(url, 'date', winlostDate);
                url = QueryString.setParam(url, 'currency', currency);
                url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
                url = QueryString.setParam(url, 'access_token', $("#access_token").val());
                url = QueryString.setParam(url, 'lang', $("#lang").val());

                const popH = Math.min($(window).height() - 100, 550);
                const title = Site.getSingleResource('CashOutHistory');

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

            showDetail: function (id) {
                var e = document.getElementById(id);
                if (e.style.display == 'block') {
                    e.style.display = 'none';
                }
                else {
                    e.style.display = 'block';
                }
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
                window.showRnGCasinoDetails = _me.showRnGCasinoDetails;
                window.ShowBonusDetails = _me.showBonusDetails;
                window.showPPDetails = _me.showPPDetails;
                window.showVideoPokerGameDetails = _me.showVideoPokerGameDetails;
                window.showSingleMatchParlayDetail = _me.showSingleMatchParlayDetail;
                window.showVGamingDetails = _me.showVGamingDetails;
                window.showVGamingJackpotWonDetails = _me.showVGamingJackpotWonDetails;
                window.ViewNewCashOutHistory = _me.ViewNewCashOutHistory;
                window.showDetail = _me.showDetail;

                const betList = {
                    ViewCasinoHandHistory: _me.ViewCasinoHandHistory
                };
                window.BetList = betList;
            }
        };

        return _me;
    });