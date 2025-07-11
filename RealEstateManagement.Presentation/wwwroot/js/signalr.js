//window.connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7031/chatHub").build();
//connection.start().catch(err => console.error(err));
// globalSignalR.js
window.globalConnection = null;

function startSignalRConnection(token, onConnectedCallback = () => { }) {
    if (!token) return;

    if (window.globalConnection && window.globalConnection.state === "Connected") {
        onConnectedCallback();
        return;
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7031/chatHub", {
            accessTokenFactory: () => token
        })
        .build();

    connection.start()
        .then(() => {
            console.log("✅ Global SignalR connected");
            window.globalConnection = connection;
            onConnectedCallback();
        })
        .catch(err => console.error("❌ SignalR error:", err));
}
