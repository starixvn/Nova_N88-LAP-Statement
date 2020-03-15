'use strict';
define(['jquery', 'common/site', 'common/querystring'], function ($, Site, QueryString) {
    const me = {};
    const _settings = {}; // store multi setting instances by 'elementcontainer' as key

    const defaultSetting = {
        pageindex: 1,
        pagesize: 50,
        pagelist: [50, 100, 1000],
        totalrow: 1,
        formId: 'formreport',
        elementcontainer: '#page',
        method: 'AJAX',
        ajaxfunction: '',
        alwayshow: false
    };

    function getByName($pagerContainer, name){
        return $('input[name="' + name + '"]', $pagerContainer);
    }

    function goToPage($pagerContainer, method, index) {
        const $pageSize = getByName($pagerContainer, 'PageSize');
        if (method === 'POST') {
            $('#IsExportExcel').val(false);
            const formName = getByName($pagerContainer, 'FormName').val();
            const form = $('#' + formName);
            const pageIndex = '<input type="hidden" value="' + index + '" name="PageIndex" />';
            const pageSize = '<input type="hidden" value="' + $pageSize.val() + '" name="PageSize" />';
            form.append(pageIndex);
            form.append(pageSize);
            Site.blockUI();
            form.submit();
        }
        else if (method === 'AJAX') {
            const $pageIndex = getByName($pagerContainer, 'PageIndex');
            $pageIndex.val(index);
            const fc = getByName($pagerContainer, 'AjaxPagingFunction').val();
            eval(fc); // jshint ignore:line
        } else {
            let url = Site.getCurrentUrl();
            url = QueryString.setParam(url, 'PageSize', $pageSize.val());
            url = QueryString.setParam(url, 'PageIndex', index);

            Site.redirect(url);
        }
    }

    function isVisible(element) {
        return !$(element).hasClass('pagingDisable');
    }

    me.render = function (option) {
        /// <summary>
        ///
        /// </summary>
        /// <param name='pageindex'></param>
        /// <param name='pagesize'></param>
        /// <param name='totalrow'></param>
        /// <param name='elementcontainer'>ex: '#page'</param>
        /// <param name='method'>POST or GET or AJAX</param>
        let pageHtml = '';
        const setting = $.extend({}, defaultSetting, option);
        _settings[setting.elementcontainer] = setting;
        let noOfPage = 1;
        const pagingVisible = setting.totalrow > setting.pagesize;

        if (pagingVisible) {
            noOfPage = parseInt(setting.totalrow / setting.pagesize
         + (setting.totalrow % setting.pagesize === 0 ? 0 : 1), 10);
        }

        if (pagingVisible || setting.alwayshow) {
            const prevPageDisabledHtml = (setting.pageindex === 1 ? ' pagingDisable"' : '"') + '></a>';
            const pageControl = ['<div class="pagingContainer center-align">',
                          '<a href="javascript:;" class="icon pagingFirst' + prevPageDisabledHtml,
                          '<a href="javascript:;" class="icon pagingPrev' + prevPageDisabledHtml,
                          '<span class="pagingSeperator"></span>'];

            const pagingInfo = '<div class="paging-info">' + Site.getSingleResource('Page') +
                '<input  type="text" name="PageIndex" class="pagingCurrent paging-form" maxlength="4" size="2" value="' + setting.pageindex + '" />'
            + '&nbsp;' + Site.getSingleResource('of') + '&nbsp;' + noOfPage + '</div>';

            pageControl.push(pagingInfo);

            pageControl.push('<span class="pagingSeperator"></span>');

            const nextPageDisabled = (setting.pageindex === noOfPage ? ' pagingDisable"' : '"') + '></a>';
            const pageNext = '<a href="javascript:;" class="icon pagingNext' + nextPageDisabled;
            pageControl.push(pageNext);
            const pageLast = '<a href="javascript:;" class="icon pagingLast' + nextPageDisabled;
            pageControl.push(pageLast);

            const pageListLength = setting.pagelist.length;
            let pageSizeSelectHtml = '<select class="pageSizeSelected">';
            const pageSize = parseInt(setting.pagesize, 10);
            for (let i = 0; i < pageListLength; i++) {
                pageSizeSelectHtml += '<option value="' + setting.pagelist[i]
                + '"' + (pageSize === setting.pagelist[i] ? ' selected="selected"' : '')
                + '>' + setting.pagelist[i] + '</option>';
            }
            pageSizeSelectHtml += '</select>';
            pageControl.push(pageSizeSelectHtml);

            pageControl.push('<input type="hidden" name="PageSize" value="' + setting.pagesize + '" />');
            pageControl.push('<input type="hidden" name="Method" value="' + setting.method + '" />');
            pageControl.push('<input type="hidden" name="FormName" value="' + setting.formId + '" />');
            pageControl.push('<input type="hidden" name="NoOfPage" value="' + noOfPage + '" />');
            pageControl.push('<input type="hidden" name="AjaxPagingFunction" value="' + setting.ajaxfunction + '" />');

            pageControl.push('</div>');
            pageHtml = pageControl.join('');
        }

        const $pagerContainer = $(setting.elementcontainer);
        $pagerContainer.html(pageHtml);
        me._bindEvent(setting);
    };
    me.getSetting = function (elementcontainer) {
        const newElementcontainer = elementcontainer || Object.keys(_settings)[0];
        const setting = _settings[newElementcontainer];
        const $pagerContainer = $(newElementcontainer);
        const $pageIndex = getByName($pagerContainer, 'PageIndex');
        const $pageSize = getByName($pagerContainer, 'PageSize');
        setting.pageindex = parseInt($pageIndex.val(), 10);
        setting.pagesize = $pageSize.val();
        return setting;
    };

    // private methods
    me._bindEvent = function (setting) {
        const $pagerContainer = $(setting.elementcontainer);
        const $pageIndex = getByName($pagerContainer, 'PageIndex');
        const method = getByName($pagerContainer, 'Method').val();
        const noOfPage = parseInt($('input[name="NoOfPage"]', $pagerContainer).val(), 10);

        $('.pagingFirst', $pagerContainer).on('click', function () {
            if (isVisible(this)) {
                goToPage($pagerContainer, method, 1);
            }
        });
        $('.pagingPrev', $pagerContainer).on('click', function () {
            const currentPage = parseInt($pageIndex.val(), 10);
            if (isVisible(this) && currentPage > 1) {
                goToPage($pagerContainer, method, currentPage - 1);
            }
        });
        $('.pagingLast', $pagerContainer).on('click', function () {
            if (isVisible(this)) {
                goToPage($pagerContainer, method, noOfPage);
            }
        });
        $('.pagingNext', $pagerContainer).on('click', function () {
            if (isVisible(this)) {
                const currentPage = parseInt($pageIndex.val(), 10);
                goToPage($pagerContainer, method, currentPage + 1);
            }
        });

        $('select.pageSizeSelected', $pagerContainer).on('change', function () {
            const $pageSize = getByName($pagerContainer, 'PageSize');
            const pageSize = $(this).val();
            $pageSize.val(pageSize);
            setting.pagesize = pageSize;
            goToPage($pagerContainer, method, 1);
        });

        $pageIndex.on('keyup', function (e) {
            const code = e.keyCode ? e.keyCode : e.which;
            if (code === 13) {
                const value = parseInt($(this).val(), 10);
                if (value > 0 && value <= noOfPage) {
                    $(this).val(value);
                    goToPage($pagerContainer, method, value);
                }
            }
        });
    };
    return me;
});