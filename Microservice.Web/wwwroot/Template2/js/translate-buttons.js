

/*===================================*
01. TRANSLATE BUTTONS
/*===================================*/


function googleTranslateElementInit() {
    new google.translate.TranslateElement({
        pageLanguage: 'tr',
        includedLanguages: "en,tr,fr,es,de,ru,el,zh-CN",
        layout: google.translate.TranslateElement.InlineLayout.SIMPLE
    }, 'google_translate_element');
}

(function ($) {
    'use strict';


    $('#google_translate_element').on('DOMNodeInserted', function () {
        $('.goog-te-menu-value span:first').html('LANGUAGE');
    });

    $(window).on('load', function () {
        setTimeout(function () {
            $('.goog-te-menu-frame.skiptranslate').contents().find('.goog-te-menu2-item-selected .text').html('LANGUAGE');
        }, 100);
    });
});