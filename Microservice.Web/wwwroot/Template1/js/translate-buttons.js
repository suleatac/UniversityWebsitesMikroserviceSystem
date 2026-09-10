

/*===================================*
 01. TRANSLATE BUTTONS
 *===================================*/

function googleTranslateElementInit() {
    new google.translate.TranslateElement({
        pageLanguage: 'tr',
        includedLanguages: "en,tr,fr,es,de,ru,el,zh-CN",
        layout: google.translate.TranslateElement.InlineLayout.SIMPLE
    }, 'google_translate_element');
}

/*----- Google Translate layout guard -----*/
(function () {
    'use strict';

    // body'ye Google'ın yazdığı stiller
    var BODY_PROPS = ['position', 'top', 'min-height', 'height'];

    // tıklamaları engelleyen enjeksiyonlar
    var OVERLAYS = [
        'body > .skiptranslate',        // banner + spinner sarmalayıcısı
        '.goog-te-spinner-pos',
        '.goog-te-spinner-animation',
        '#goog-gt-tt'
    ];

    function stripBodyStyles() {
        var body = document.body;
        if (!body || !body.style) return;

        var dirty = false;
        for (var i = 0; i < BODY_PROPS.length; i++) {
            if (body.style.getPropertyValue(BODY_PROPS[i])) {
                body.style.removeProperty(BODY_PROPS[i]);
                dirty = true;
            }
        }
        if (dirty && !body.getAttribute('style')) {
            body.removeAttribute('style');
        }
    }

    function neutralize(el) {
        // inline + !important -> Google'ın inline stilini de ezer
        el.style.setProperty('display', 'none', 'important');
        el.style.setProperty('pointer-events', 'none', 'important');
    }

    function sweep() {
        stripBodyStyles();

        for (var i = 0; i < OVERLAYS.length; i++) {
            var nodes = document.querySelectorAll(OVERLAYS[i]);
            for (var j = 0; j < nodes.length; j++) {
                var el = nodes[j];
                // navbar içindeki gerçek çeviri gadget'ına dokunma
                if (el.id === 'google_translate_element' ||
                    el.querySelector('#google_translate_element')) continue;
                neutralize(el);
            }
        }
    }

    function start() {
        sweep();

        // Google elemanları asenkron ekliyor/yeniden yazıyor -> gözlemle
        new MutationObserver(function (muts) {
            for (var i = 0; i < muts.length; i++) {
                var m = muts[i];
                if (m.type === 'attributes' && m.target === document.body) { sweep(); return; }
                if (m.type === 'childList' && m.addedNodes.length) { sweep(); return; }
            }
        }).observe(document.documentElement, {
            childList: true,
            subtree: true,
            attributes: true,
            attributeFilter: ['style', 'class']
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', start);
    } else {
        start();
    }
})();
