using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;

namespace RxAsyncConsole
{
	public class Program
	{
		public static void Main()
		{
			var rxBasics = new RxBasics();
			rxBasics.Example1();
			//rxBasics.Example2();

			var asyncWorkerServiceWrapper = new AsyncWorkerServiceWrapper();
			//asyncWorkerServiceWrapper.Example1();
			//asyncWorkerServiceWrapper.Example2();
			//asyncWorkerServiceWrapper.Example3();
			//asyncWorkerServiceWrapper.Example4();
			//asyncWorkerServiceWrapper.Example4A();
			//asyncWorkerServiceWrapper.Example4B();
			//asyncWorkerServiceWrapper.Example5();
			//asyncWorkerServiceWrapper.Example6();

			var workerServiceWrapper = new WorkerServiceWrapper();
			//workerServiceWrapper.Example1();
			//workerServiceWrapper.Example2();
			#region Example3
			//Console.WriteLine("Subscribe to Example3");
			//workerServiceWrapper
			//    .Example3()
			//    .Subscribe(data => Console.WriteLine("Example3 next data: {0}", data));

			//Console.WriteLine("Waiting for 5 seconds");
			//Thread.Sleep(5000);
			//workerServiceWrapper.StartExample3();
			//Console.WriteLine("Example3 has been started.");
			//Console.WriteLine("Waiting for 10 seconds");
			//Thread.Sleep(10000);
			//workerServiceWrapper.StopExample3();
			//Console.WriteLine("Example3 has been stopped.");
			#endregion

			Console.ReadKey();
		}
	}
}
