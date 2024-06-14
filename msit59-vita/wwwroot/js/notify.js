const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

function getUnreadNum() {
	$.ajax({
		url: '/Notify/GetUnreadNum',
		type: 'POST',
		success: function (res) {
			if (res != 0) {
				$('#notifyNum').removeClass('d-none');
				$('#notifyNum').addClass('d-block');
				$('#notifyNum').text(res);
			} else {
				$('#notifyNum').removeClass('d-block');
				$('#notifyNum').addClass('d-none');
			}
		}
	})
}

function getNotifyList() {
	$.ajax({
		url: '/Notify/GetNotifyList',
		type: 'GET',
		success: function (res) {
			$(".notify-list").each((idx, item) => $(item).html(res));
			getUnreadNum();
		},
		error: function (xhr, status, error) {
			console.error(error);
			alert("出現無法預期的問題，請稍後再試。")
		}
	});
}

$(document).ready(function () {
	getNotifyList();

	$('#offcanvasNotify, #offcanvasNotifyMobile').on('hidden.bs.offcanvas', function () {
		$.ajax({
			url: '/Notify/SetRead',
			type: 'POST',
			success: function (res) {
				$(".notify-list").each((idx, item) => $(item).html(res));
				getUnreadNum();
			},
			error: function (xhr, status, error) {
				console.error(error);
				alert("出現無法預期的問題，請稍後再試。")
			}
		})
	})
});

connection.start().then(function () {
	console.log("start chat");
}).catch(function (err) {
	return console.error(err.toString());
});

connection.on("StatusChange", function (CustomerId, OrderId, CustomerOrderStatus) {
	if ($('#StoreCustomerId').data('customerid') == CustomerId) {
		getNotifyList();
	};
});