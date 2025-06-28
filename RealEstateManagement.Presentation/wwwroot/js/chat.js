"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7031/chatHub").build();
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    li.textContent = `${user}: ${message}`;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    connection.invoke("JoinConversation", conversationId).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", conversationId, user, message).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});
