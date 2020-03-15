define(['jquery', 'components/jquery.flatpopup'], function ($) {
    'use strict';
    /*
    var defaultOptions = {
        width: 640,
        height: 480,
        id: "popup-content",
        className: "",
        title: "&nbsp;",
        container: "body",
        modal: DEFAULT_MODAL,
        onOpen: null,
        onClose: null
    };
    */

    const _me = {
        openPopup: function (element, option) {
            return $(element).flatPopup(option);
        },
        closePopup: function (id) {
            $('#' + id).fadeOut(300);
            $('.popup-block-body').hide();
        },
        openPopupWithIframe: function (url, option) {
            let $popUp = $('#' + option.id);
            const iframe = '<iframe id="popupFrame" src="{0}" ></iframe>';
            if ($popUp.length === 0) {
                $popUp = $('<div id="' + option.id + '" class="popup-frame">' + iframe.replace('{0}', url) + '</div>');
            }
            else {
                $popUp.append(iframe.replace('{0}', 'about:blank'));

                const onOpen = option.onOpen;
                option.onOpen = function () {
                    $('#popupFrame').attr('src', url);

                    if (onOpen) {
                        onOpen.apply(this);
                    }
                };
            }

            return $popUp.flatPopup(option);
        },

        updateCurrentPopup: function (url, option) {
            let $popUp;
            const $frame = parent.$("#popupFrame");
            if (option.id) {
                $popUp = $("#" + option.id);
            }

            if (!$popUp || $popUp.length === 0) {
                $popUp = $frame.closest(".popup-frame");
                option.id = $popUp.attr('id');
            }

            if ($popUp.length === 0) {
                return;
            }

            if (url && url !== '') {
                $frame.attr('src', url);
            }

            if (option.title !== "") {
                $popUp.find(".txt-title").html(option.title);
            }

            return $popUp.flatPopup(option);
        },

        show: function ($popup, isModal) {
            $popup.flatPopup("show", isModal);
        },

        /*
        var buttons = [
                {
                    text: "OK",
                    click: function () { },
                    classes: "btn btn-primary",
                    id: "",
                    isClosedOnClick: false
                }];
        */
        openPopupWithButton: function (element, option, buttons) {
            var defaultOption = {
                padding: 15,
                id: "popup-content"
            };

            $.extend(defaultOption, option || {});

            const $element = $(element);
            const $popup = $("#" + defaultOption.id);
            if ($popup.length === 0 || $popup.hasClass('flat-popup') === false) {
                $element.append('<div class="right popup-button"></div>');
                const $blockButton = $element.find(".popup-button");

                var firstButton = true;
                $.each(buttons, function (index, button) {
                    const id = button.id ? ' id="' + button.id + '"' : '';
                    const className = button.classes ? ' class="' + button.classes + '"' : '';
                    const autoFocus = firstButton ? ' autofocus' : '';

                    var $button = $('<button type="button"' + id + className + autoFocus + '>'
                        + button.text + '</button>');
                    $blockButton.append($button);
                    if (button.isClosedOnClick) {
                        $button.on('click', function () {
                            $element.find('a.popup-close').trigger('click');

                            if (typeof button.click === 'function') {
                                button.click();
                            }
                        });
                    }
                    else {
                        $button.on('click', button.click);
                    }

                    firstButton = false;
                });
            }

            return $element.flatPopup(defaultOption);
        }
    };

    return _me;
});