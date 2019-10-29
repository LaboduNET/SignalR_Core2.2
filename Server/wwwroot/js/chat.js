"use strict";
//Create the webscket connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.ChatKey = "1111_Nico";
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("loadMessagesToPage", function (messages) {
	messages.map(message => {
		AddUIMessage(message);
	});	
});
connection.on("addNewMessageToPage", (message) => {
	// Add the message to the page.
	AddUIMessage(message);
	});


connection.start().then(function () {
	document.getElementById("sendButton").disabled = false;
	connection.invoke("InitChat", connection.ChatKey).catch(function (err) {
		return console.error(err.toString());
	});
}).catch(function (err) {
	return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
	var message = document.getElementById("Message").value;
	connection.invoke("Send", connection.ChatKey, message).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});

function AddUIMessage(message) {
	var encodedMsg = '<strong>' +
		htmlEncode(message.name) +
		'</strong>: (' + htmlEncode(message.date) + ') <br/>' +
		htmlEncode(message.message).split('\n').join('<br/>'); 	
	var li = document.createElement("li");
	li.innerHTML = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
}
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
	var encodedValue = $('<div />').text(value).html();
	return encodedValue;
}
