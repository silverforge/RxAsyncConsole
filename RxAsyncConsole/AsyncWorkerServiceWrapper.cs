using System;
using System.Reactive.Concurrency;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Threading;
using RxAsyncConsole.AsyncWorkerServiceReference;

namespace RxAsyncConsole
{
	public class AsyncWorkerServiceWrapper
	{
		private readonly AsyncWorkerServiceClient asyncClient = new AsyncWorkerServiceClient();

		/// <summary>
		/// Example1. Good old way.
		/// </summary>
		public void Example1()
		{
			Console.WriteLine("Example1:");
			asyncClient.GetData1Completed += (sender, args) =>
			{
				if (args.Error == null && !args.Cancelled)
					Console.WriteLine("data1 has arrived: {0}", args.Result);
			};

			asyncClient.GetData1Async();
			Console.WriteLine("Waiting for data1...");
		}

		/// <summary>
		/// Example2. Hard to synchronize.
		/// </summary>
		public void Example2()
		{
			Console.WriteLine("Example2:");

			var data1HasArrived = false;
			var data2HasArrived = false;

			var data1 = 0;
			var data2 = 0;

			asyncClient.GetData1Completed += (sender, args) =>
			{
				data1 = args.Result;
				data1HasArrived = true;
				Console.WriteLine("data1 has arrived: {0}", data1);
				if (data2HasArrived)
					Console.WriteLine("Result of data1 + data2: {0}", data1 + data2);
			};

			asyncClient.GetData2Completed += (sender, args) =>
			{
				data2 = args.Result;
				data2HasArrived = true;
				Console.WriteLine("data2 has arrived: {0}", data2);
				if (data1HasArrived)
					Console.WriteLine("Result of data1 + data2: {0}", data1 + data2);
			};

			asyncClient.GetData1Async();
			Console.WriteLine("Waiting for data1...");
			asyncClient.GetData2Async();
			Console.WriteLine("Waiting for data2...");
		}

		/// <summary>
		/// Example3. FromAsyncPattern. ThreadId: the observer runs on new thread every time.
		/// </summary>
		public void Example3()
		{
			Console.WriteLine("Example3:");
			Console.WriteLine("ThreadId of Example3: {0}", Thread.CurrentThread.ManagedThreadId);
			Console.WriteLine("Waiting for data1...");

			Observable
				.FromAsyncPattern<int>(asyncClient.BeginGetData1, asyncClient.EndGetData1)()
				.ObserveOn(Scheduler.ThreadPool)
				.SubscribeOn(Scheduler.CurrentThread)
				.Do(data1 => Console.WriteLine("ThreadId of observable: {0}", Thread.CurrentThread.IsThreadPoolThread))
				.Subscribe(data1 => Console.WriteLine("data1 has arrived: {0}", data1));
		}

		/// <summary>
		/// Example4. Easy to synchronize.
		/// </summary>
		public void Example4()
		{
			Console.WriteLine("Example4:");
			Console.WriteLine("Waiting for data1...");
			Console.WriteLine("Waiting for data2...");

			Func<IObservable<int>> getData1Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData1, asyncClient.EndGetData1);
			Func<IObservable<int>> getData2Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData2, asyncClient.EndGetData2);

			Plan<int> plan = getData1Async()
				.And(getData2Async())
				.Then((data1, data2) =>
						{
							Console.WriteLine("data1 has arrived: {0}", data1);
							Console.WriteLine("data2 has arrived: {0}", data2);
							return data1 + data2;
						});

			Observable
				.When(plan)
				.Subscribe(result => Console.WriteLine("Result of data1 + data2: {0}", result));
		}

		/// <summary>
		/// Example4A. Easy to synchronize. Fastest is the winner.
		/// </summary>
		public void Example4A()
		{
			Console.WriteLine("Example4A:");
			Console.WriteLine("Waiting for data1...");
			Console.WriteLine("Waiting for data2...");
			Console.WriteLine("Waiting for data3...");
			Func<IObservable<int>> getData1Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData1, asyncClient.EndGetData1);
			Func<IObservable<int>> getData2Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData2, asyncClient.EndGetData2);
			Func<IObservable<int>> getData3Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData3, asyncClient.EndGetData3);

			var plan1 = getData1Async()
				.Then(data1 =>
				      	{
				      		Console.WriteLine("Done data1: {0}", data1);
				      		return data1;
				      	});
			var plan2 = getData2Async()
				.Then(data2 =>
				      	{
				      		Console.WriteLine("Done data2: {0}", data2);
				      		return data2;
				      	});
			var plan3 = getData3Async()
				.Then(data3 =>
				      	{
				      		Console.WriteLine("Done data3: {0}", data3);
				      		return data3;
				      	});

			Observable
				.When(plan1, plan2, plan3)
				.Subscribe(next => Console.WriteLine("Next: {0}", next));

		}

		/// <summary>
		/// Example4B. Easy to synchronize. Fastest is the winner. Use fiddler to determine which was the true winner.
		/// </summary>
		public void Example4B()
		{
			Console.WriteLine("Example4B:");
			Console.WriteLine("Waiting for data1...");
			Console.WriteLine("Waiting for data2...");
			Console.WriteLine("Waiting for data3...");
			Func<IObservable<int>> getData1Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData1, asyncClient.EndGetData1);
			Func<IObservable<int>> getData2Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData2, asyncClient.EndGetData2);
			Func<IObservable<int>> getData3Async = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData3, asyncClient.EndGetData3);

			getData1Async()
				.Amb(getData2Async())
				.Amb(getData3Async())
				.Subscribe(next => Console.WriteLine("The winner is: {0}", next));
		}

		/// <summary>
		/// Example5. Nested calls.
		/// </summary>
		public void Example5()
		{
			Console.WriteLine("Example5:");
			var getData1 = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData1, asyncClient.EndGetData1)();
			var getData2 = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData2, asyncClient.EndGetData2)();

			Console.WriteLine("Waiting for data1...");
			getData1.Subscribe(data1 =>
								{
									Console.WriteLine("data1 has arrived: {0}", data1);
									Console.WriteLine("Waiting for data2...");
									getData2.Subscribe(data2 =>
														{
															Console.WriteLine("data2 has arrived: {0}", data2);
															var result = data1 + data2;
															Console.WriteLine("Result of data1 + data2: {0}", result);
														});
								});
		}

		/// <summary>
		/// Example6. Polling async service.
		/// </summary>
		public void Example6()
		{
			Console.WriteLine("Example6:");
			Console.WriteLine("Waiting for data3...");

			var getData3A = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData3, asyncClient.EndGetData3)();
			Observable
				.Interval(TimeSpan.FromSeconds(1))
				.Subscribe(value => getData3A
										.Subscribe(data3 => Console.WriteLine("data3 has arrived: {0}", data3)));

			#region Working solution

			//var getData3B = Observable.FromAsyncPattern<int>(asyncClient.BeginGetData3, asyncClient.EndGetData3);
			//Observable
			//    .Interval(TimeSpan.FromSeconds(1))
			//    .Subscribe(value => getData3B()
			//                            .Subscribe(data3 => Console.WriteLine("data3 has arrived: {0}", data3)));

			#endregion

			#region Working sync solution (Show at the end)

			//Observable
			//    .Interval(TimeSpan.FromSeconds(1))
			//    .Subscribe(next =>
			//                {
			//                    var data3 = asyncClient.GetData3();
			//                    Console.WriteLine("data3 has arrived: {0}", data3);
			//                });

			#endregion
		}
	}
}