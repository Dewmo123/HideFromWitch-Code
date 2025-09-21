using ServerCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
	

	class Program
	{
		static void Main(string[] args)
		{
			// DNS (Domain Name System)
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3303);

			Connector connector = new Connector();
			//Thread.Sleep(20000);
			connector.Connect(endPoint, 
				() => { return SessionManager.Instance.Generate(); },
				4);

			while (true)
			{
			}
		}
	}
}
