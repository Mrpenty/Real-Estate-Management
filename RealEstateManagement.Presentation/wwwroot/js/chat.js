"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7031/chatHub").build();
let currentConversationId = null;
let userId = null;

connection.on("ReceiveMessage", function (message) {
    if (currentConversationId === null) return;

    const container = document.getElementById("messagesContainer");

    const div = document.createElement("div");
    div.className = "message " + (parseInt(message.senderId) === userId ? "right" : "left");
    div.innerHTML = `
        <small>${new Date(message.sentAt).toLocaleString()}</small>
        <div>${message.content}</div>
    `;
    container.appendChild(div);

    container.scrollTop = container.scrollHeight;
});

// Kết nối tới hub SignalR
connection.start().then(function () {
    console.log("SignalR connected");
    if (currentConversationId) {
        connection.invoke("JoinConversation", currentConversationId.toString());
    }
    initializeChat();
}).catch(function (err) {
    return console.error(err.toString());
});


async function initializeChat() {
    const token = localStorage.getItem("authToken");
    if (!token) {
        alert("Bạn chưa đăng nhập!");
        return;
    }

    userId = getUserIdFromToken(token);
    await loadConversations();
}

async function loadConversations() {
    const token = localStorage.getItem("authToken");
    if (!token) {
        alert("Bạn chưa đăng nhập!");
        return;
    }

    try {
        const res = await fetch("https://localhost:7031/api/Chat/List-Conversation", {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!res.ok) {
            const errText = await res.text();
            console.error("API lỗi:", errText);
            alert("Không thể tải danh sách cuộc trò chuyện. Lỗi: " + res.status);
            return;
        }

        const conversations = await res.json();
        const listDiv = document.getElementById("conversationList");

        listDiv.innerHTML = "";
        conversations.forEach(conv => {
            const div = document.createElement("div");
            div.classList.add("conversation-item");
            div.innerText = conv.landlordName || conv.renterName || "Đối tác";
            div.addEventListener("click", () => loadMessages(conv.id));
            listDiv.appendChild(div);
        });
    } catch (err) {
        console.error("Lỗi khi fetch danh sách:", err);
        alert("Lỗi kết nối đến server");
    }
}

window.loadMessages = async function (conversationId) {
    currentConversationId = conversationId;

    await connection.invoke("JoinConversation", conversationId.toString());

    const token = localStorage.getItem("authToken");
    

    const res = await fetch(`https://localhost:7031/api/Chat/conversation/${conversationId}`, {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    const container = document.getElementById("messagesContainer");
    container.innerHTML = "";

    if (!res.ok) {
        container.innerHTML = "<p>Không thể tải tin nhắn</p>";
        return;
    }

    const messages = await res.json();

    if (messages.length === 0) {
        container.innerHTML = "<p>Chưa có tin nhắn nào.</p>";
        return;
    }

    messages.forEach(msg => {
        const div = document.createElement("div");
        div.className = "message " + (msg.senderId === userId ? "right" : "left");

        div.innerHTML = `
            <small>${new Date(msg.sentAt).toLocaleString()}</small>
            <div>${msg.content}</div>
    `;

        container.appendChild(div);
    });
   
    container.scrollTop = container.scrollHeight;
};


document.getElementById("sendButton").addEventListener("click", async function () {
    const content = document.getElementById("messageInput").value.trim();
    if (!content || currentConversationId === null) return;

    const token = localStorage.getItem("authToken");

    const res = await fetch(`https://localhost:7031/api/Chat/send`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({
            conversationId: currentConversationId,
            content: content
            
        })
    });

    if (res.ok) {
        // Gửi qua SignalR (realtime)
        await connection.invoke("SendMessage", currentConversationId.toString(), userId.toString(), content);
        await loadMessages(currentConversationId);
        document.getElementById("messageInput").value = "";

    } else {
        alert("Gửi tin nhắn thất bại");
    }
});
document.getElementById("messageInput").addEventListener("keydown", function (event) {
    if (event.key === "Enter" && !event.shiftKey) {
        event.preventDefault(); // Ngăn việc xuống dòng
        document.getElementById("sendButton").click();
    }
});
function getUserIdFromToken(token) {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return parseInt(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
}
