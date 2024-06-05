$(document).ready(function () {

    getCart();

});


//商品數量改變時
$(document).ready(function () {

$('.cart-container').on('input change paste keyup', '.product-quantity-input', function () {
        
    var inputField = $(this);
    var cartItemId = inputField.closest('.cart-item').data('id');
    var maxStock = inputField.closest('.cart-item').data('stock');
    var productId = inputField.closest('.cart-item').data('productid');
    var storeId = inputField.closest('.cart-item').data('storeid');

    var currentValue = parseInt(inputField.val());

    var newValue = currentValue;

    if (isNaN(newValue) || newValue < 1) {
        newValue = 1;
    } else if (newValue > maxStock) {
        newValue = maxStock;
        maxQuantity()
    } else if (newValue > 30) {
        newValue = 30;
        maxQuantity()
    }

    inputField.val(newValue);
    
    calculateTotalPrice();
    countCartItems();

    // 發送 AJAX 請求
    $.ajax({
        url: '/ShoppingCarts/AddToCartInput', // 你的控制器方法的路徑
        type: 'POST',
        data: { productId: productId, quantity: newValue, storeId: storeId },
        success: function (data) {
            // 在這裡處理從控制器返回的數據
            
        },
        error: function () {
            alert('發生錯誤');
        }
    });

});

});






function getCart() {
    $.ajax({
        type: "GET",
        url: "/ShoppingCarts/Index",
        success: function (data) {
            if (!data.success) {
                return;
            }
            /*console.log(data.cart);*/
            renderCartItems(data.cart);

        },
        error: function (xhr, status, error) {
            console.error("Error fetching cart data: ", error);
        }
    });
}

function renderCartItems(cartItems) {
    var cartContainer = $(".cart-container");
    cartContainer.empty();

    cartItems.forEach(function (item) {
        var productImage = item.productImage ? item.productImage : 'image/Common/300x300_default.png';
        var cartItemHtml = `
							<div class="cart-item d-flex justify-content-between pb-3" data-id="${item.shoppingCartId}" data-productid="${item.productId}" data-stock="${item.productUnitsInStock}" data-storeid="${item.storeID}">
								<img src="/${productImage}" class="cart-image img-fluid rounded-2 me-2" alt="食物圖片">
								<div class="cart-details">
									<div class="cart-info d-flex justify-content-between mb-1">
										<div class="me-3">
											<div class="shop-name fs-6 fw-bold">${item.storeName}</div>
											<div class="product-name fs-5 fw-bold">${item.productName}</div>
										</div>
										<div class="product-delete text-success fs-4 mt-1" onclick="deleteCartItem(${item.shoppingCartId})">
											<i class="fa-solid fa-trash"></i>
										</div>
									</div>
									<div class="shop-price d-flex justify-content-around align-items-center">
										<div class="input-group w-50">
											<button class="btn border-0" type="button" onclick="adjustValue(this, -1, ${item.shoppingCartId}, ${item.productUnitsInStock})">
												<i class="fa-solid fa-minus fs-5 fw-bold text-success"></i>
											</button>
											<input type="text" class="form-control fs-6 fw-bold text-center product-quantity-input p-0" value="${item.quantity}" id="number" autocomplete="off" min="1" max="99" oninput="this.value=this.value.replace(/[^0-9]/g,'')">
											<button class="btn border-0" type="button" onclick="adjustValue(this, 1, ${item.shoppingCartId}, ${item.productUnitsInStock})">
												<i class="fa-solid fa-plus fs-5 fw-bold text-success"></i>
											</button>
										</div>
										<div class="fs-5 fw-bold text-center product-price" data-price="${item.price}">${item.price}</div>
									</div>
								</div>
							</div>
						`;
        cartContainer.append(cartItemHtml);
    });

    calculateTotalPrice();
    countCartItems();

}

function calculateTotalPrice() {
    var totalPrice = 0;
    $('.cart-item').each(function () {
        var quantity = parseInt($(this).find('.product-quantity-input').val());
        var price = parseInt($(this).find('.product-price').data('price'));
        if (!isNaN(quantity) && !isNaN(price)) {
            totalPrice += quantity * price;
        }
    });
    if (!isNaN(totalPrice)) {
        $('.totalPriceNum').text('$' + totalPrice);
    }
}


//統計商品數量
function countCartItems() {
    var totalCartItems = 0;
    $('.cart-item').each(function () {
        var quantity = parseInt($(this).find('.product-quantity-input').val());
        if (!isNaN(quantity)) {
            totalCartItems += quantity;
        }
    });

    var cartBadge = $('#cartNum');
    if (totalCartItems > 0) {
        cartBadge.removeClass('d-none');
        if (totalCartItems >= 99) {
            cartBadge.text(99)
        } else {
            cartBadge.text(totalCartItems);
        }
    } else {
        cartBadge.addClass('d-none');
    }

}

// 加減商品數量
function adjustValue(button, increment, cartItemId, maxStock) {
    var inputField = $(button).closest('.input-group').find('.product-quantity-input');
    var currentValue = parseInt(inputField.val());
    var productId = inputField.closest('.cart-item').data('productid');
    var storeId = inputField.closest('.cart-item').data('storeid');
    var newValue = currentValue + increment;
    if (newValue < 1) {
        newValue = 1;
    } else if (newValue > maxStock) {
        newValue = maxStock;
        maxQuantity()
    } else if (newValue > 30) {
        newValue = 30;
        maxQuantity();
    }

    inputField.val(newValue);
    calculateTotalPrice();
    countCartItems();

    //console.log(productId);
    //console.log(newValue);
    //console.log(storeId);
    // 發送 AJAX 請求
    $.ajax({
        url: '/ShoppingCarts/AddToCartInput', // 你的控制器方法的路徑
        type: 'POST',
        data: { productId: productId, quantity: newValue, storeId: storeId },
        success: function (data) {
            // 在這裡處理從控制器返回的數據

            //console.log(data);

        },
        error: function () {
            alert('發生錯誤');
        }
    });

}

// 已達商品加入數量上限
function maxQuantity() {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr["warning"]("商品數量已達上限")
}

// 刪除商品
function deleteCartItem(cartItemId) {
    //console.log(cartItemId);
    $.ajax({
        type: "POST",
        url: "/ShoppingCarts/Delete",
        data: { id: cartItemId },
        success: function (data) {
            //console.log(data);
            if (data.success) {
                $(`.cart-item[data-id=${cartItemId}]`).remove();
                calculateTotalPrice(); // 成功更新後重新獲取購物車資料
                countCartItems();
                //console.log("Item deleted successfully.");
            } else {
                console.error("Error: ", data.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error deleting item: ", error);
        }
    });
}

// 結帳
$(document).ready(function () {
    $('#checkoutBtn').click(function () {
        checkout();
    });

    function checkout() {
        var totalPrice = $('.totalPriceNum').text().replace('$', '');

        $.ajax({
            type: "POST",
            url: "/CheckedPayment/PaymentConfirm",
            success: function (data) {
                //console.log(data);
                if (data.success) {
                    if (totalPrice > 0) {
                        window.location.href = "/CheckedPayment/Payment";
                    }
                    return;
                } else {
                    //跳出登入視窗
                    showLoginModal(data.message);
                    console.error("Error: ", data.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching cart data: ", error);
            }
    });
}
});

function showLoginModal(message) {
    // 您的代碼,用於顯示登入視窗
    // 可以在這裡顯示錯誤消息
    // alert(message);
    // 設置錯誤消息
    $('#loginMessage').text(message);

    // 顯示模態對話框
    $('#loginModal').modal('show');
}
