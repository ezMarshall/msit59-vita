
//function adjustValue(button, increment) {
//    var inputField = $(button).closest('.input-group').find('.product-quantity-input');
//    var currentValue = parseInt(inputField.val());
//    var newValue = currentValue + increment;
//    if (newValue < 1) {
//        newValue = 1;
//    }
//    inputField.val(newValue);
//    calculateTotalPrice();
//}


////計算商品 total price
//function calculateTotalPrice() {
//    var totalPrice = 0;
//    $('.cart-item').each(function () {
//        var quantity = parseInt($(this).find('.product-quantity-input').val());
//        var price = parseInt($(this).find('.product-price').data('price'));
//        if (!isNaN(quantity) && !isNaN(price)) {
//            totalPrice += quantity * price;
//        }
//    });
//    if (!isNaN(totalPrice)) {
//        $('#totalPrice').text('$' + totalPrice);
//        // $('#totalPriceMobile').text('$' + totalPrice);
//    }
//}

////刪除商品
//// $(document).on('click', '.product-delate', function () {
////     $(this).closest('.cart-item').remove();
//// });

//$('.product-delate').on('click', function () {
//    $(this).closest('.cart-item').remove();
//});


//// 初始化時計算總金額
//$(document).ready(function () {
//    calculateTotalPrice();
//});

//// 商品數量改變時重新計算總金額
//$('.product-quantity-input').bind('input', function () {
//    console.log('change');
//    calculateTotalPrice();
//});




