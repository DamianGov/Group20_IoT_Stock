function notify(type, message) {
    (() => {
        let n = document.createElement("div");
        let id = Math.random().toString(36).substr(2, 10);
        n.setAttribute("id", id);
        n.classList.add("notification", type);
        n.innerText = message;
        document.getElementById("notification-area").appendChild(n);
        setTimeout(() => {
            var notifications = document.getElementById("notification-area").getElementsByClassName("notification");
            for (let i = 0; i < notifications.length; i++) {
                if (notifications[i].getAttribute("id") == id) {
                    notifications[i].remove();
                    break;
                }
            }
        }, 5000);
    })();
}

function notifySuccess(message) {
    notify("success", message);
}
function notifyError(message) {
    notify("error", message);
}
function notifyInfo(message) {
    notify("info", message);
}