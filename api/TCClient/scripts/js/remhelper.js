function remhelper(baseWidth, baseFontSize, fontscale) {
    let _baseFontSize = baseFontSize || 100;
    let _fontscale = fontscale || 1;
    let doc = window.document;
    let ua = navigator.userAgent;
    let matches = ua.match(/Android[\S\s]+AppleWebkit\/(\d{3})/i);
    let UCversion = ua.match(/U3\/((\d+|\.){5,})/i);
    let isUCHd = UCversion && parseInt(UCversion[1].split('.').join(''), 10) >= 80;
    let isIos = navigator.appVersion.match(/(iphone|ipad|ipod)/gi);
    let dpr = window.devicePixelRatio || 1;
    if (!isIos && !(matches && matches[1] > 534) && !isUCHd) {
        // 如果非iOS, 非Android4.3以上, 非UC内核, 就不执行高清, dpr设为1;
        dpr = 1;
    }
    let scale = 1 / dpr;
    let metaEl = doc.querySelector('meta[name="viewport"]');
    if (!metaEl) {
        metaEl = doc.createElement('meta');
        metaEl.setAttribute('name', 'viewport');
        doc.head.appendChild(metaEl);
    }
    let browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    //let nowWidth = Math.min(browserWidth * scale, 750);
    let nowWidth = browserWidth * scale;
    metaEl.setAttribute('content', 'width=device-width,user-scalable=no,initial-scale=' + scale + ',maximum-scale=' + scale + ',minimum-scale=' + scale);
    let lastFontSize = (nowWidth / baseWidth) * _baseFontSize / scale * _fontscale;
    doc.documentElement.style.fontSize = lastFontSize + 'px';
    return scale;
}
window.onresize = function () {
    let scale = remhelper(750);
    let browserWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    // if (document.getElementById('app')) {

    //     if (browserWidth * scale > 750) {
    //         document.getElementById('app').style.width = 750 / scale + 'px';
    //     } else {
    //         document.getElementById('app').style.width = '100%';
    //     }
    // }
    if (document.getElementById('allmap')) {
        document.getElementById('allmap').style.zoom = 1 / scale;
    }
};

window.onresize();