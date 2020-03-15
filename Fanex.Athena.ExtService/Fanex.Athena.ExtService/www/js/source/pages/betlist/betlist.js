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
    });