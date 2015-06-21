using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RxAsyncConsole.WorkerServiceReference;

namespace RxAsyncConsole
{
	public class WorkerServiceWrapper
	{
		private readonly WorkerServiceClient client = new WorkerServiceClient();
		private readonly ISubject<int> example3Subject = new Subject<int>();
		private IDisposable example3Subscribe;

		/// <summary>
		/// Example1. Blocks the thread.
		/// </summary>
		public void Example1()
		{
			Console.WriteLine("Example1:");
			Console.WriteLine("Waiting for data...");
			var dataLongWay = client.GetDataLongWay();
			Console.WriteLine("data has arrived: {0}", dataLongWay);
		}

		/// <summary>
		/// Example2. Polling service.
		/// </summary>
		public void Example2()
		{
			Observable
				.Interval(TimeSpan.FromSeconds(1))
				.Subscribe(i =>
				           	{
				           		var data1 = client.GetData1();
				           		Console.WriteLine("data1: {0}", data1);
				           	});
			Console.WriteLine("Method end");
		}

		/// <summary>
		/// Example3. Subscribe to this sequence.
		/// </summary>
		/// <returns></returns>
		public IObservable<int> Example3()
		{
			return example3Subject;
		}

		/// <summary>
		/// Starts the example3.
		/// </summary>
		public void StartExample3()
		{
			example3Subscribe = Observable
				.Interval(TimeSpan.FromSeconds(1))
				.Subscribe(i =>
				           	{
				           		var data1 = client.GetData1();
				           		example3Subject.OnNext(data1);
				           	});
		}

		/// <summary>
		/// Stops the example3.
		/// </summary>
		public void StopExample3()
		{
			example3Subscribe.Dispose();
		}
	}
}