"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://194.233.81.64:5000/chatHub").build();
let currentConversationId = null;
let userId = null;

let skipCount = 0;
const pageSize = 20;
let isLoading = false;
let allMessagesLoaded = false;

let editingMessageId = null;
// Kết nối tới hub SignalR
connection.start().then(async function () {
    console.log("SignalR connected");
    if (currentConversationId) {
        await connection.invoke("JoinConversation", conversationId.toString());
        await loadMessages(currentConversationId, 0, pageSize, false);
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
    div.innerHTML = renderMessageHtml(message, false);
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
        const res = await fetch("http://194.233.81.64:5000/api/Chat/List-Conversation", {
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
        const listDiv = document.getElementById("conversationItemsContainer");
        listDiv.innerHTML = ""; // Xoá hết item cũ (không đụng vào header)

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
    const listDiv = document.getElementById("conversationItemsContainer");

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

    // ❗ Người đọc KHÔNG cần hiển thị "✔✔ đã đọc"
    if (readerId === userId) return;

    const container = document.getElementById("messagesContainer");

    // ✅ Chỉ xóa icon cũ nếu mình là người gửi
    container.querySelectorAll(".message.right .read-icon").forEach(icon => icon.remove());

    const lastMessage = container.querySelector(`.message.right[data-id='${messageId}']`);
    if (lastMessage) {
        const icon = document.createElement("span");
        icon.className = "read-icon";
        icon.textContent = "✔✔ Đã đọc";
        lastMessage.appendChild(icon);
    }
});


connection.on("MessageDeleted", function (conversationId, messageId) {
    if (conversationId !== currentConversationId) return;

    const msgEl = document.querySelector(`.message[data-id='${messageId}']`);
    if (msgEl) msgEl.remove();
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

    const url = `http://194.233.81.64:5000/api/Chat/conversation/${conversationId}?skip=${skip}&take=${take}`;
    const res = await fetch(url, {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (!res.ok) {
        container.innerHTML = "<p>Không thể tải tin nhắn</p>";
        return;
    }

    const messages = await res.json(); // ✅ đúng vị trí, sau khi có `res`


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
        const isLastRead = lastMyReadMessage && msg.id === lastMyReadMessage.id;
        div.innerHTML = renderMessageHtml(msg, isLastRead);
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

    const token = localStorage.getItem("authToken");

    if (editingMessageId) {
        // 👉 Gửi API cập nhật
        try {
            const res = await fetch(`http://194.233.81.64:5000/api/Chat/edit-message/${editingMessageId}`, {
                method: "PUT",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(content)
            });

            if (!res.ok) {
                const errText = await res.text();
                alert("Lỗi khi cập nhật: " + errText);
                return;
            }

            document.getElementById("messageInput").value = "";
            editingMessageId = null;
            document.getElementById("editBanner").style.display = "none";

            document.getElementById("sendButton").textContent = "Gửi";
        } catch (err) {
            alert("Lỗi khi gọi API sửa: " + err.toString());
        }

    } else {
        // 👉 Gửi tin nhắn mới qua SignalR
        try {
            await connection.invoke("SendMessage", currentConversationId.toString(), userId.toString(), content);
            document.getElementById("messageInput").value = "";
        } catch (err) {
            alert("Gửi tin nhắn thất bại: " + err.toString());
        }
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
    // Sau khi loadConversations xong
    document.getElementById("conversationSearchInput").addEventListener("input", function () {
        const keyword = this.value.toLowerCase();

        document.querySelectorAll(".conversation-item").forEach(item => {
            const name = item.querySelector(".name")?.innerText.toLowerCase() || "";
            const message = item.querySelector(".last-message")?.innerText.toLowerCase() || "";
            const match = name.includes(keyword) || message.includes(keyword);
            item.style.display = match ? "block" : "none";
        });
    });


}
function renderMessageHtml(msg, isLastRead) {
    const isRight = msg.senderId === userId;

    return `
        <div class="message-row ${isRight ? 'right' : 'left'}">
            ${isRight ? renderActions(msg.id, msg.sentAt) + renderMessageBubble(msg) : renderMessageBubble(msg)}
        </div>
        ${isRight && isLastRead ? '<div class="read-icon" style="text-align: right; padding: 0px; font-size: 13px;">✔✔ Đã đọc</div>' : ''}
    `;
}




function renderMessageBubble(msg, isLastRead) {
    return `
        <div class="message-bubble">
            ${msg.content}
            <small class="message-time-inline">${new Date(msg.sentAt).toLocaleString()}</small>
        </div>
    `;
}

function renderActions(messageId, sentAt) {
    const sentTime = new Date(sentAt);
    const now = new Date();
    const diffMinutes = (now - sentTime) / (1000 * 60); // chênh lệch phút

    const allowEdit = diffMinutes <= 15;

    if (!allowEdit) return ""; 

    return `
        <div class="message-actions">
            <button class="action-btn">⋮</button>
            <div class="dropdown-menu">
                <div onclick="editMessage(${messageId})">Edit</div>
                <div onclick="deleteMessage(${messageId})">Delete</div>
            </div>
        </div>
    `;
}
async function deleteMessage(messageId) {
    const confirmDelete = confirm("Bạn có chắc muốn xóa tin nhắn này?");
    if (!confirmDelete) return;

    const token = localStorage.getItem("authToken");
    try {
        const res = await fetch(`http://194.233.81.64:5000/api/Chat/delete-message/${messageId}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!res.ok) {
            const errText = await res.text();
            alert("Lỗi khi xóa: " + errText);
            return;
        }

        // Xóa phần tử tin nhắn khỏi UI
        const msgEl = document.querySelector(`.message[data-id='${messageId}']`);
        if (msgEl) msgEl.remove();
    } catch (err) {
        alert("Xóa thất bại: " + err.toString());
    }
}

function editMessage(messageId) {
    const msgDiv = document.querySelector(`.message[data-id='${messageId}']`);
    if (!msgDiv) return;

    const bubble = msgDiv.querySelector(".message-bubble");
    const textContent = bubble ? bubble.childNodes[0].textContent.trim() : "";

    // Gán giá trị vào input
    document.getElementById("messageInput").value = textContent;

    // Gán ID đang chỉnh sửa
    editingMessageId = messageId;

    // Hiện banner
    document.getElementById("editBanner").style.display = "flex";

    // Focus ô nhập
    document.getElementById("messageInput").focus();
}



connection.on("MessageEdited", function (messageId, newContent) {
    const msgDiv = document.querySelector(`.message[data-id='${messageId}']`);
    if (!msgDiv) return;

    const bubble = msgDiv.querySelector(".message-bubble");
    if (bubble) {
        bubble.innerHTML = `
            ${newContent}
            <small class="message-time-inline">${new Date().toLocaleString()}</small>
        `;
    }
});
document.getElementById("cancelEditBtn").addEventListener("click", function () {
    editingMessageId = null;
    document.getElementById("messageInput").value = "";
    document.getElementById("editBanner").style.display = "none";
});



connection.on("ReceiveNotification", function (notification) {
    console.log("🔔 Nhận notification:", notification);

    const dot = document.getElementById("chat-notification-dot");
    if (dot) {
        dot.classList.remove("hidden"); // ✅ Gỡ ẩn
    }
});

async function startChatWithLandlord(landlordId, propertyid) {
    const token = localStorage.getItem("authToken");
    if (!token) {
        window.location.href = "/Auth/Login";
        return;
    }

    const renterId = getUserIdFromToken(token);
    const dto = { renterId, landlordId, propertyid };

    try {
        const res = await fetch("http://194.233.81.64:5000/api/Chat/create-conversation", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            const err = await res.text();
            alert("Không thể tạo hoặc mở cuộc trò chuyện: " + err);
            return;
        }

        const data = await res.json();
        if (data && data.id) {
            window.location.href = `/Chat/Index?conversationId=${data.id}`;
        }
    } catch (err) {
        console.error("Lỗi khi mở chat:", err);
    }
}
