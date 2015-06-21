using System;
using System.Threading;
using RxAsyncCommon;

namespace RxAsyncService.Services
{
	public class AsyncWorkerService : IAsyncWorkerService
	{
		private readonly Random rndNumber = new Random();
		private readonly Random rndSecond = new Random();

		public int GetData1()
		{
			Thread.Sleep(TimeSpan.FromSeconds(rndSecond.Next(0, 2)));
			return rndNumber.Next(0, 10);
		}

		public int GetData2()
		{
			Thread.Sleep(TimeSpan.FromSeconds(rndSecond.Next(1, 3)));
			return rndNumber.Next(10, 15);
		}

		public int GetData3()
		{
			return rndNumber.Next(100, 300);
		}
	}
}
