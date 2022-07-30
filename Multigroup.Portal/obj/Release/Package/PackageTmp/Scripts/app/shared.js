function BuilNotifications(callback, btnProcess, url, container) {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.notificationHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (message, mensaje) {
        // Add the message to the page.
        CallBackGet(function (data) {
            $(container).html(data);
        }, url, null)
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        $(btnProcess).click(function () {
            //ProccessFile();
            callback();
            // Call the Send method on the hub.
            //chat.server.send();
        });
    });
}

function SaveFile(container, urlPost) {
    //OnStart(); 
    var fileProfile = $("#" + container).val();
    if (fileProfile) {
        var file = document.getElementById(container).files[0];
        var formData = new FormData();
        formData.append(file.name, file);
        var xhr = new XMLHttpRequest();

        xhr.open('POST', urlPost, true);
        xhr.onload = function (e) {
            var response = $.parseJSON(e.target.response);
            //ResponseMessage(xhr.status, xhr.statusText);
        };
        xhr.onerror = function () {
            ResponseMessage(xhr.status, xhr.statusText);
        };
        xhr.send(formData);
    }
}



