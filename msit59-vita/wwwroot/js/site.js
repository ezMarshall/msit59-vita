$(function () {
    // 設定最小高度：螢幕高 - header高 - footer高
    let footerHeight = $('footer').height() + 48;
    let windowHeight = $(window).height();
    $('.min-h-custom').css('min-height', (windowHeight - 86 - footerHeight) + 'px');
})
