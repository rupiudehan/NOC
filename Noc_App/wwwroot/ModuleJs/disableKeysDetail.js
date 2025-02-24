﻿function disabledEvent(e) {
    if (e.stopPropagation) {
        e.stopPropagation();
    } else if (window.event) {
        window.event.cancelBubble = true;
    }
    e.preventDefault();
    return false;
}

$(document).ready(function () {
    $(document).on("contextmenu", function (e) {
        e.preventDefault();
        alert("Right-click is disabled.");
    });
});

$(document).keydown(function (e) {
    // "C" key
    if (e.ctrlKey && e.shiftKey && e.keyCode == 67) {
        disabledEvent(e);
    }
    // "I" key
    else if (e.ctrlKey && e.shiftKey && e.keyCode == 73) {
        disabledEvent(e);
    }
    // "J" key
    else if (e.ctrlKey && e.shiftKey && e.keyCode == 74) {
        disabledEvent(e);
    }
    // "S" key + macOS
    else if (e.keyCode == 83 && (navigator.platform.match("Mac") ? e.metaKey : e.ctrlKey)) {
        disabledEvent(e);
    }
    // "U" key
    else if (e.ctrlKey && e.keyCode == 85) {
        disabledEvent(e);
    }
    // "F12" key
    else if (event.keyCode == 123) {
        disabledEvent(e);
    }
});