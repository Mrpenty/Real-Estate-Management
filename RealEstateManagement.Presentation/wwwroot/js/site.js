// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
connection.on("ReceiveNotification", function (notification) {
    console.log("🔔 Nhận notification:", notification);

    const dot = document.getElementById("chat-notification-dot");
    if (dot) {
        dot.classList.remove("hidden"); // ✅ Gỡ ẩn
    }
});
