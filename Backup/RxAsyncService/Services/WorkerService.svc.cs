using System;
using System.Threading;
using RxAsyncCommon;

namespace RxAsyncService.Services
{
	public class WorkerService : IWorkerService
	{
		private readonly Random rndNumber = new Random();
		private readonly Random rndMillisecond = new Random();

		public int GetDataLongWay()
		{
			Thread.Sleep(TimeSpan.FromMilliseconds(rndMillisecond.Next(5000, 10000)));
			return rndNumber.Next(0, int.MaxValue);
		}

		public int GetData1()
		{
			Thread.Sleep(TimeSpan.FromMilliseconds(rndMillisecond.Next(100, 200)));
			return rndNumber.Next(0, 10);
		}

		public int GetData2()
		{
			Thread.Sleep(TimeSpan.FromMilliseconds(rndMillisecond.Next(150, 300)));
			return rndNumber.Next(10, 15);
		}
	}
}
