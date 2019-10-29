# SignalR_Core2.2
This is a sample using SignalR in ASP.NET Core 2.2 web application. It can be launch in a Docker container or in a self-hosted host.
We can enable the Chat CLient to be used from another Web Server by enabling CORS for this server in the startup.cs file. 
The Origin http://127.0.0.1:8000 has been enabled for CORS.

# Instructions:
1. Launch the project in Visual Studio 2017 in debug mode (The Client on the same site of the chat is launched).
2. We will create a new client:
   * Right-click the homePage and "Save As" in a folder of your choice
   * Edit the /Client_files/chat.js.downloaded and change the URL of the Chat Server 
var connection = new signalR.HubConnectionBuilder().withUrl("https://XXXyourserver:yourPortXXX/chatHub").build();
   * Change the connection.ChatKey = "1111_SomeUserName";
   * Run the Client.html with any webServer (ex:web server for chrome) on http://127.0.0.1:8000/Client.html  

