document.addEventListener("DOMContentLoaded", function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7277").build();  // 将URL替换为正确的后端 SignalR Hub URL

    // 与Server建立连接
    connection.start().then(function () {
        console.log("Hub 连接完成");
    }).catch(function (err) {
        alert('连接错误: ' + err.toString());
    });

    // 更新连接 ID 列表事件
    connection.on("UpdList", function (jsonList) {
        var list = JSON.parse(jsonList);
        $("#IDList li").remove();
        for (i = 0; i < list.length; i++) {
            $("#IDList").append($("<li></li>").attr("class", "list-group-item").text(list[i]));
        }
    });

    // 更新用户个人连接 ID 事件
    connection.on("UpdSelfID", function (id) {
        $('#SelfID').html(id);
    });

    // 更新聊天内容事件
    connection.on("UpdContent", function (msg) {
        $("#Content").append($("<li></li>").attr("class", "list-group-item").text(msg));
    });

    // 发送消息
    $('#sendButton').on('click', function () {
        let selfID = $('#SelfID').html();
        let message = $('#message').val();
        let sendToID = $('#sendToID').val();
        connection.invoke("SendMessage", selfID, message, sendToID).catch(function (err) {
            alert('发送错误: ' + err.toString());
        });
    });
});
