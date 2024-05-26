// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


    $(document).ready(function () {
        // 頁面剛載入時隱藏訂房欄，顯示導航欄
        $('.booking-area').hide();
    $('.navbar').show();

    // 初始檢查滾動距離，以確保在頁面載入時的初始狀態正確
    $(window).trigger('scroll');

    // 當滾動頁面時
    $(window).scroll(function () {
                // 獲取目前的滾動距離
                var scrollDistance = $(window).scrollTop();

                // 如果滾動距離超過0，則隱藏導航欄和訂房欄
                if (scrollDistance > 0) {
        $('.navbar').hide();
    $('.booking-area').hide();
                } else {
        // 如果滾動距離小於等於0，則顯示導航欄，隱藏訂房欄
        $('.navbar').show();
    $('.booking-area').hide();
                }

                // 如果滾動的距離超過500px，則顯示訂房欄
                if (scrollDistance > 500)
    $('.booking-area').show();
    else // 如果滾動距離小於等於500，則隱藏訂房欄
    $('.booking-area').hide();


    if (scrollDistance = 0) {  // 回到原點時
        $('.navbar').show();
    $('.booking-area').hide();
                }
            });
        });


