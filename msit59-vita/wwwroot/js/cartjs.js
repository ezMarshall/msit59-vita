$(document).ready(function () {

    getCart();

});

// 商品數量改變時
$('.cart-container').on('input change paste keyup', '.product-quantity-input', function () {
    var inputField = $(this);
    var cartItemId = inputField.closest('.cart-item').data('id');
    var maxStock = inputField.closest('.cart-item').data('stock');

    var currentValue = parseInt(inputField.val());
    // console.log(currentValue);
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
});

function getCart() {
    $.ajax({
        type: "GET",
        url: "/ShoppingCarts/Index",
        success: function (data) {
            // console.log(data);
            renderCartItems(data);
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
									<div class="cart-item d-flex justify-content-between pb-3" data-id="${item.shoppingCartId}" data-stock="${item.productUnitsInStock}" data-storeid="${item.storeID}">
										<img src="/${productImage}" class="cart-image img-fluid rounded-2 me-2" alt="食物圖片">
										<div class="cart-details">
											<div class="cart-info d-flex justify-content-between mb-1">
												<div class="me-3">
													<div class="shop-name fs-6 fw-bold">${item.storeName}</div>
													<div class="product-name fs-5 fw-bold">${item.productName}</div>
												</div>
												<div class="product-delate text-success fs-4 mt-1" onclick="deleteCartItem(${item.shoppingCartId})">
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

// 加減商品數量
function adjustValue(button, increment, cartItemId, maxStock) {
    var inputField = $(button).closest('.input-group').find('.product-quantity-input');
    var currentValue = parseInt(inputField.val());
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

