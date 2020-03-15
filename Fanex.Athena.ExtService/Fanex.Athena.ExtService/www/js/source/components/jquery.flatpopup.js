'use strict';
define(['jquery'], function ($) {
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
});