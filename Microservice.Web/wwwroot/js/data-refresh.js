/*!
 * data-refresh.js
 * SignalR üzerinden "DataChanged" mesajını dinler. Hangfire senkronizasyonları
 * (Personel/Ogrenci) tamamlandığında Web tarafındaki consumer bu mesajı yayar.
 *
 * Strateji: Mevcut sayfa adresi yeniden çekilir (SSR çıktısı tek kaynak olduğu için
 * template HTML'ini istemci tarafında tekrar üretmeye gerek yoktur), parsed HTML
 * içinden aynı [data-refresh-region] anahtarı bulunur ve innerHTML değiştirilir.
 *
 * Kullanım (view tarafı):
 *   data-refresh-region : sayfa içinde BENZERSİZ bölge anahtarı (örn. "personel-list")
 *   data-refresh-scope  : boşlukla ayrılmış dinlenecek scope'lar ("personel", "ogrenci",
 *                        "personel ogrenci"). Yazılmazsa tüm mesajlara tepki verir.
 *   Bölge tazelenince "datarefreshed" custom event'i tetiklenir (isotope vb. yeniden
 *   başlatmak isteyen scriptler için).
 */
(function () {
    "use strict";

    if (typeof signalR === "undefined") {
        return; // SignalR client yüklenmemişse sessiz çık
    }

    function findRegions(scope) {
        return Array.prototype.slice
            .call(document.querySelectorAll("[data-refresh-region]"))
            .filter(function (el) {
                var list = (el.getAttribute("data-refresh-scope") || "all")
                    .toLowerCase().split(/\s+/);
                return list.indexOf("all") > -1 || list.indexOf(scope) > -1;
            });
    }

    function refreshRegions(regions) {
        // Sayfanın tam güncel SSR halini çek (cache consumer tarafından düşürüldüğü
        // için artık taze veri dönecek). ?d= ekleyerek olası proxy/tarayıcı cache'ini aş.
        var url = window.location.href;
        url += (url.indexOf("?") > -1 ? "&" : "?") + "dref=" + Date.now();

        return fetch(url, {
            credentials: "same-origin",
            headers: { "X-Requested-With": "data-refresh" }
        }).then(function (res) {
            if (!res.ok) {
                throw new Error("HTTP " + res.status);
            }
            return res.text();
        }).then(function (html) {
            var doc = new DOMParser().parseFromString(html, "text/html");

            regions.forEach(function (el) {
                var regionKey = el.getAttribute("data-refresh-region");
                var fresh = doc.querySelector(
                    '[data-refresh-region="' + regionKey + '"]');

                if (fresh) {
                    el.innerHTML = fresh.innerHTML;
                    el.dispatchEvent(new CustomEvent("datarefreshed", {
                        bubbles: true,
                        detail: { region: regionKey }
                    }));
                }
            });
        }).catch(function (err) {
            console.warn("[data-refresh] Yenileme başarısız:", err);
        });
    }

    var backoff = [0, 2000, 5000, 10000, 30000];
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/data-refresh")
        .withAutomaticReconnect(backoff)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("DataChanged", function (msg) {
        var scope = (msg && msg.scope ? String(msg.scope) : "all").toLowerCase();
        var regions = findRegions(scope);

        if (!regions.length) {
            return;
        }

        console.info("[data-refresh] " + scope + " verisi değişti, bölgeler tazeleniyor.");
        refreshRegions(regions);
    });

    function start() {
        connection.start().catch(function () {
            // withAutomaticReconnect yeniden dener; ilk deneme hatası fatal değil.
        });
    }

    connection.onreconnecting(function () {
        console.info("[data-refresh] SignalR yeniden bağlanıyor...");
    });

    connection.onreconnected(function () {
        console.info("[data-refresh] SignalR yeniden bağlandı.");
    });

    start();
})();
