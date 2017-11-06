window.onload = function () {

    var oWin = document.getElementById("dialog");
    
    //var oBtn = document.getElementsByTagName("button")[0];
    var oClose = document.getElementById("close");
    var oH2 = oWin.getElementsByClassName("title")[0];
    var bDrag = false;
    var disX = disY = 0;
    //oBtn.onclick = function () {
    //    oWin.style.display = "block"
    //};
    oClose.onclick = function () {
        oWin.style.display = "none"

    };
    oClose.onmousedown = function (event) {
        (event || window.event).cancelBubble = true;
    };
    oH2.onmousedown = function (event) {
        var event = event || window.event;
        bDrag = true;
        disX = event.clientX - oWin.offsetLeft;
        disY = event.clientY - oWin.offsetTop;
        this.setCapture && this.setCapture();
        return false
    };
    document.onmousemove = function (event) {
        if (!bDrag) return;
        var event = event || window.event;
        var iL = event.clientX - disX;
        var iT = event.clientY - disY;
        var maxL = document.documentElement.clientWidth - oWin.offsetWidth;
        var maxT = document.documentElement.clientHeight - oWin.offsetHeight;
        iL = iL < 0 ? 0 : iL;
        iL = iL > maxL ? maxL : iL;
        iT = iT < 0 ? 0 : iT;
        iT = iT > maxT ? maxT : iT;

        oWin.style.marginTop = oWin.style.marginLeft = 0;
        oWin.style.left = iL + "px";
        oWin.style.top = iT + "px";
        return false
    };
    document.onmouseup = window.onblur = oH2.onlosecapture = function () {
        bDrag = false;
        oH2.releaseCapture && oH2.releaseCapture();
    };
};