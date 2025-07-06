"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7031/chatHub").build();
let currentConversationId = null;
let userId = null;

let skipCount = 0;
const pageSize = 20;
let isLoading = false;
let allMessagesLoaded = false;

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


connection.on("ReceiveMessage", function (message) {
    if (message.conversationId !== currentConversationId) return;

    const container = document.getElementById("messagesContainer");

    const div = document.createElement("div");
    div.className = "message " + (parseInt(message.senderId) === userId ? "right" : "left");
    div.setAttribute("data-id", message.id);
    div.innerHTML = `
        <div>${message.content}</div>
        <small>${new Date(message.sentAt).toLocaleString()}</small>

    `;
    container.appendChild(div);

    container.scrollTop = container.scrollHeight;
});


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

        // Hàm cắt chuỗi (giới hạn 40 ký tự)
        const truncate = (text, maxLength = 10) => {
            if (!text) return "Chưa có tin nhắn";
            return text.length > maxLength ? text.slice(0, maxLength) + "..." : text;
        };

        conversations.forEach(conv => {
            const div = document.createElement("div");
            div.classList.add("conversation-item");
            div.setAttribute("data-conv-id", conv.id); // để handler ConversationUpdated tìm được

            const displayName = conv.landlordName || conv.renterName || "Đối tác";
            const shortMessage = truncate(conv.lastMessage, 40);
            const timeText = conv.lastSentAt ? new Date(conv.lastSentAt).toLocaleString() : "";

            div.innerHTML = `
        <div class="name">${displayName}</div>
        <div class="last-message">${shortMessage}</div>
        <small class="timestamp">${timeText}</small>
    `;

            div.addEventListener("click", () => loadMessages(conv.id, 0, pageSize, false));
            listDiv.appendChild(div);
        });


    } catch (err) {
        console.error("Lỗi khi fetch danh sách:", err);
        alert("Lỗi kết nối đến server");
    }
}
connection.on("ConversationUpdated", function (update) {
    const { conversationId, lastMessage, sentAt, senderId } = update;
    const listDiv = document.getElementById("conversationList");

    const item = listDiv.querySelector(`[data-conv-id="${conversationId}"]`);

    if (item) {
        // Cập nhật nội dung mới
        const msgDiv = item.querySelector(".last-message");
        const timeDiv = item.querySelector(".timestamp");

        if (msgDiv) msgDiv.innerText = lastMessage;
        if (timeDiv) timeDiv.innerText = new Date(sentAt).toLocaleString();

        // Đưa lên đầu
        if (listDiv.firstChild !== item) {
            listDiv.prepend(item);
        }
    } else {
        // Nếu conversation chưa có (mới tạo), reload toàn bộ
        loadConversations();
    }
});
connection.on("MessageRead", function (conversationId, readerId, messageId) {
    if (conversationId !== currentConversationId) return;

    const container = document.getElementById("messagesContainer");

    // Xóa mọi icon đã đọc cũ
    container.querySelectorAll(".message.right .read-icon").forEach(icon => icon.remove());

    // Tìm đúng tin nhắn cuối đã đọc
    const lastMessage = container.querySelector(`.message.right[data-id='${messageId}']`);
    if (lastMessage) {
        const icon = document.createElement("span");
        icon.className = "read-icon";
        icon.textContent = "✔✔ Đã đọc";
        lastMessage.appendChild(icon);
    }
});


window.loadMessages = async function (conversationId, skip = 0, take = 20, append = false) {
    currentConversationId = conversationId;

    document.querySelectorAll(".conversation-item").forEach(item => item.classList.remove("active"));
    const activeItem = document.querySelector(`[data-conv-id="${conversationId}"]`);
    if (activeItem) activeItem.classList.add("active");

    await connection.invoke("JoinConversation", conversationId.toString());

    const token = localStorage.getItem("authToken");
    const container = document.getElementById("messagesContainer");
    if (!append) container.innerHTML = "";

    const url = `https://localhost:7031/api/Chat/conversation/${conversationId}?skip=${skip}&take=${take}`;
    const res = await fetch(url, {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!res.ok) {
        container.innerHTML = "<p>Không thể tải tin nhắn</p>";
        return;
    }

    const messages = await res.json(); // ✅ đúng vị trí, sau khi có `res`

    console.log("Tin nhắn load:", messages.length, { skip, take });

    if (!append) {
        skipCount = messages.length;
        allMessagesLoaded = false;
    } else if (messages.length > 0) {
        skipCount += messages.length;
    }

    if (messages.length === 0 && !append) {
        container.innerHTML = "<p>Chưa có tin nhắn nào.</p>";
        return;
    }

    const lastMyReadMessage = [...messages].reverse().find(m => m.senderId === userId && m.isRead);
    let previousScrollHeight = container.scrollHeight;

    messages.forEach(msg => {
        const div = document.createElement("div");
        div.className = "message " + (msg.senderId === userId ? "right" : "left");
        div.setAttribute("data-id", msg.id);
        div.innerHTML = `
            <div>${msg.content}</div>
            <small>${new Date(msg.sentAt).toLocaleString()}</small>
            ${lastMyReadMessage && msg.id === lastMyReadMessage.id
                ? '<span class="read-icon">✔✔ Đã đọc</span>' : ''}
        `;
        append ? container.prepend(div) : container.appendChild(div);
    });

    if (append) {
        const newScrollHeight = container.scrollHeight;
        container.scrollTop = newScrollHeight - previousScrollHeight;
    } else {
        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                container.scrollTop = container.scrollHeight;
            });
        });
    }



    if (messages.length < take) {
        allMessagesLoaded = true;
    }

    await connection.invoke("MarkAsRead", conversationId.toString(), userId.toString());
};






document.getElementById("sendButton").addEventListener("click", async function () {
    const content = document.getElementById("messageInput").value.trim();
    if (!content || currentConversationId === null) return;

    try {
        await connection.invoke("SendMessage", currentConversationId.toString(), userId.toString(), content);
        document.getElementById("messageInput").value = "";
    } catch (err) {
        alert("Gửi tin nhắn thất bại: " + err.toString());
    }
});

document.getElementById("messageInput").addEventListener("keydown", function (event) {
    if (event.key === "Enter" && !event.shiftKey) {
        event.preventDefault(); // Ngăn việc xuống dòng
        document.getElementById("sendButton").click();
    }
});
//Lấy token
function getUserIdFromToken(token) {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return parseInt(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
}
//Kiểm tra đã đăng nhập hay chưa
async function initializeChat() {
    const token = localStorage.getItem("authToken");
    if (!token) {
        alert("Bạn chưa đăng nhập!");
        return;
    }

    userId = getUserIdFromToken(token);
    await loadConversations();
    document.getElementById("messagesContainer").addEventListener("scroll", async function () {
        const container = this;
        if (container.scrollTop < 100 && !isLoading && !allMessagesLoaded) {
            isLoading = true;
            const previousHeight = container.scrollHeight;

            await loadMessages(currentConversationId, skipCount, pageSize, true);

            const newHeight = container.scrollHeight;
            container.scrollTop = newHeight - previousHeight;

            skipCount += pageSize;
            isLoading = false;
        }
    });

}
