using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Server.Hubs
{
	public class ChatHub : Hub
	{

		private static string PathFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public async Task Join()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");

		}

		public async Task Send(string generatedKey, string message)
		{
			var generatedKeyList = generatedKey.Split('_');
			if (generatedKeyList != null && generatedKeyList.Length > 1)
			{
				string chatKey = generatedKeyList[0];
				string userName = generatedKeyList[1];
				List<ChatMessage> messageList = LoadMessageList(chatKey);
				var nesMess = new ChatMessage()
				{
					Name = userName,
					Message = message,
					Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
				};
				messageList.Add(nesMess);
				SaveMessageListInFile(chatKey, messageList);
				// Call the addNewMessageToPage method to update clients$.connection.
				await Clients.Group(chatKey).SendCoreAsync("addNewMessageToPage", new object[] { nesMess });
			}
		}

		public async Task InitChat(string generatedKey)
		{
			var generatedKeyList = generatedKey.Split('_');
			if (generatedKeyList != null && generatedKeyList.Length > 1)
			{
				string chatKey = generatedKeyList[0];
				string userName = generatedKeyList[1];
				await Groups.AddToGroupAsync(Context.ConnectionId, chatKey);
				List<ChatMessage> messageList = LoadMessageList(chatKey);
				// Call the addNewMessageToPage method to update clients.
				await Clients.Caller.SendCoreAsync("loadMessagesToPage", new object[] { messageList });
			}
		}

		protected List<ChatMessage> LoadMessageList(string chatKey)
		{
			string path = System.IO.Path.Combine(PathFile, $"ChatHubContent_{chatKey}.json");
			if (System.IO.File.Exists(path))
			{
				string fileContent = System.IO.File.ReadAllText(path);
				var tasksList = JsonConvert.DeserializeObject<List<ChatMessage>>(fileContent);

				return tasksList;
			}
			else
				return new List<ChatMessage>();
		}

		protected void SaveMessageListInFile(string chatKey, List<ChatMessage> tasksList)
		{
			if (tasksList != null)
			{
				string json = JsonConvert.SerializeObject(tasksList);
				json = json.Replace("},{", "},\r\n{").Replace("[{", "[\r\n{").Replace("}]", "}\r\n]");

				string path = System.IO.Path.Combine(PathFile, $"ChatHubContent_{chatKey}.json");
				System.IO.File.WriteAllText(path, json);
			}
		}
	}


	public class ChatMessage
	{
		public string Name { get; set; }
		public string Message { get; set; }
		public string Date { get; set; }
	}


}

