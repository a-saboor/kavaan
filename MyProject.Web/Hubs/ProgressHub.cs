using Microsoft.AspNet.SignalR;

namespace MyProject.Web.Hubs
{
	public class ProgressHub : Hub
	{
		public string GetConnectionId()
		{
			return this.Context.ConnectionId;
		}
	}
}