var connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

connection.on("ReceiveMessage", function (user, message, recipeName) {
    alert(`${user} added a comment with message ${message} go to ${recipeName} comment page to see the comment`);
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});
var sendMessage = document.getElementById('sendMessage');
if (sendMessage) {
    sendMessage.addEventListener("click", function (event) {
        var user = document.getElementById("SenderMail").value;
        var message = document.getElementById('CommentMessageArea').value;
        var recipeName = document.getElementById('RecipeName').value;
        connection.invoke("SendMessage", user, message, recipeName).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}

