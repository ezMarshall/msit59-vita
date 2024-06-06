// 取餐時間日期
const today = new Date();
const year = today.getFullYear();
const month = String(today.getMonth() + 1).padStart(2, '0');
const day = String(today.getDate()).padStart(2, '0');
const formattedDate = `${year}/${month}/${day}`;

const datepickerInput = document.getElementById('datepicker');
datepickerInput.value = formattedDate;

// 獲取當前時間
var currentTime = new Date();
var currentHour = currentTime.getHours();
var currentMinute = currentTime.getMinutes();

// 動態生成小時選項
var hoursList = document.getElementById('hours');
for (var i = currentHour; i <= 23; i++) {
    var hourItem = document.createElement('li');
    hourItem.classList.add('list-group-item');
    if (i === currentHour) {
        hourItem.classList.add('active');
    }
    hourItem.textContent = i < 10 ? '0' + i : i;
    hourItem.onclick = function () {
        var selectedHour = this.textContent;
        var selectedMinute = document.querySelector('.minutes .active').textContent;
        document.getElementById('timepicker').value = selectedHour + ':' + selectedMinute;
    };
    hoursList.appendChild(hourItem);
}

// 動態生成分鐘選項
// var minutesList = document.getElementById('minutes');
// for (var i = currentMinute - (currentMinute % 5); i <= 55; i += 5) {
//     var minuteItem = document.createElement('li');
//     minuteItem.classList.add('list-group-item');
//     if (i === currentMinute - (currentMinute % 5)) {
//         minuteItem.classList.add('active');
//     }
//     minuteItem.textContent = i < 10 ? '0' + i : i;
//     minuteItem.onclick = function () {
//         var selectedHour = document.querySelector('.hours .active').textContent;
//         var selectedMinute = this.textContent;
//         document.getElementById('timepicker').value = selectedHour + ':' + selectedMinute;
//     };
//     minutesList.appendChild(minuteItem);
// }


// 監聽小時選擇的變化
var hours = document.querySelectorAll('.hours li');
hours.forEach(function (hourItem) {
    hourItem.addEventListener('click', function () {

        hours.forEach(function (item) {
            item.classList.remove('active');
        });
        hourItem.classList.add('active');

        // 重新生成分鐘選項
        generateMinutes();
    });
});
// 初始化生成分鐘選項
generateMinutes();

// 生成分鐘選項的函數
function generateMinutes() {
    var selectedHour = parseInt(document.querySelector('.hours .active').textContent);
    var minutesList = document.getElementById('minutes');

    // 清空原有的分鐘選項
    minutesList.innerHTML = '';

    // 根據用戶選擇的小時確定分鐘範圍
    var startMinute, endMinute;
    if (selectedHour === currentHour) {
        startMinute = currentMinute - (currentMinute % 5);
        endMinute = 55;
    } else {
        startMinute = 0;
        endMinute = 55;
    }

    // 動態生成分鐘選項
    for (var i = startMinute; i <= endMinute; i += 5) {
        var minuteItem = document.createElement('li');
        minuteItem.classList.add('list-group-item');
        if (i === currentMinute - (currentMinute % 5)) {
            minuteItem.classList.add('active');
        }
        minuteItem.textContent = i < 10 ? '0' + i : i;
        minuteItem.onclick = function () {
            var selectedHour = document.querySelector('.hours .active').textContent;
            var selectedMinute = this.textContent;
            document.getElementById('timepicker').value = selectedHour + ':' + selectedMinute;
        };
        minutesList.appendChild(minuteItem);
    }
}

var hoursList = document.getElementById('hours');
var minutesList = document.getElementById('minutes');
var timePickerInput = document.getElementById('timepicker');

// 將小時和分鐘選項的點擊事件綁定到各自的父元素上
hoursList.addEventListener('click', function (event) {
    var target = event.target;
    if (target.tagName === 'LI') {
        // 取消上一個 active
        var prevActiveHour = document.querySelector('.hours .active');
        if (prevActiveHour) {
            prevActiveHour.classList.remove('active');
        }
        // 設定當前選項為 active
        target.classList.add('active');
        // 更新時間選擇器的值
        updateTimePickerValue();
    }
});

minutesList.addEventListener('click', function (event) {
    var target = event.target;
    if (target.tagName === 'LI') {
        // 取消上一個 active
        var prevActiveMinute = document.querySelector('.minutes .active');
        if (prevActiveMinute) {
            prevActiveMinute.classList.remove('active');
        }
        // 設定當前選項為 active
        target.classList.add('active');
        // 更新時間選擇器的值
        updateTimePickerValue();
    }
});

// 監聽“確定”按鈕的點擊事件
var confirmButton = document.querySelector('.date-time-button');
confirmButton.addEventListener('click', function () {
    var selectedDate = document.getElementById('datepicker').value;
    var selectedTime = document.getElementById('timepicker').value;

    // 將值顯示到<input>元素中
    var timeInput = document.getElementById('timeInput');
    timeInput.value = selectedDate + ' ' + selectedTime;

});

// 更新時間選擇器的值
function updateTimePickerValue() {
    var selectedHour = document.querySelector('.hours .active').textContent;
    var selectedMinute = document.querySelector('.minutes .active').textContent;
    timePickerInput.value = selectedHour + ':' + selectedMinute;
}