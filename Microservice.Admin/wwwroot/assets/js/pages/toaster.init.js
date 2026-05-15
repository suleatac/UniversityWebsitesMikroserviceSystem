/**
 * Bootstrap Toast Demo Script'i
 * Bu script demo toast butonlarını ve toast'ları yönetir
 */

// Toast 1: Live Toast
const toastTrigger = document.getElementById("liveToastBtn");
const toastLiveExample = document.getElementById("liveToast");

if (toastTrigger) {
    toastTrigger.addEventListener("click", function () {
        new bootstrap.Toast(toastLiveExample).show();
    });
}

// Toast 2: Bordered Toast 1
const toastTrigger2 = document.getElementById("borderedToast1Btn");
const toastLiveExample2 = document.getElementById("borderedToast1");

if (toastTrigger2) {
    toastTrigger2.addEventListener("click", function () {
        new bootstrap.Toast(toastLiveExample2).show();
    });
}

// Toast 3: Bordered Toast 2
const toastTrigger3 = document.getElementById("borderedToast2Btn");
const toastLiveExample3 = document.getElementById("borderedToast2");

if (toastTrigger3) {
    toastTrigger3.addEventListener("click", function () {
        new bootstrap.Toast(toastLiveExample3).show();
    });
}

// Toast 4: Bordered Toast 3 (Not: ID'de typo var: "borderedTost3Btn")
const toastTrigger4 = document.getElementById("borderedTost3Btn");
const toastLiveExample4 = document.getElementById("borderedTost3");

if (toastTrigger4) {
    toastTrigger4.addEventListener("click", function () {
        new bootstrap.Toast(toastLiveExample4).show();
    });
}

// Toast 5: Bordered Toast 4
const toastTrigger5 = document.getElementById("borderedToast4Btn");
const toastLiveExample5 = document.getElementById("borderedToast4");

if (toastTrigger5) {
    toastTrigger5.addEventListener("click", function () {
        new bootstrap.Toast(toastLiveExample5).show();
    });
}

// Toast Placement Demo
const toastPlacement = document.getElementById("toastPlacement");
const selectToastPlacement = document.getElementById("selectToastPlacement");

if (toastPlacement && selectToastPlacement) {
    selectToastPlacement.addEventListener("change", function () {
        // Orijinal class'ı kaydet
        if (!toastPlacement.dataset.originalClass) {
            toastPlacement.dataset.originalClass = toastPlacement.className;
        }

        // Yeni class ekle
        toastPlacement.className =
            toastPlacement.dataset.originalClass + " " + this.value;
    });
}

// Tüm demo toast'ları otomatik göster (autohide kapalı)
document.querySelectorAll(".bd-example .toast").forEach(function (toastElement) {
    new bootstrap.Toast(toastElement, {
        autohide: false
    }).show();
});