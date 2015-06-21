using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RxAsyncConsole
{
	public class RxBasics
	{
		public void Example1()
		{
			// Source data sequence
			IObservable<int> observable = Observable.Range(30, 10);

			// Subscriber
			observable
				.Subscribe(next =>
							{
								Console.WriteLine("Next item: {0}", next);
							},
						   ex =>
						   {
							   Console.WriteLine("Exception: {0}", ex.Message);
						   },
						   () =>
						   {
							   Console.WriteLine("Completed.");
						   });
		}

		public void Example2()
		{
			// Source data sequence
			var observable = Observable
				.Create<int>(subj =>
				             	{
				             		subj.OnNext(30);
				             		subj.OnNext(31);
				             		subj.OnNext(32);
				             		subj.OnNext(33);
				             		subj.OnNext(34);
									//subj.OnError(new Exception("Something very bad happened!"));
				             		subj.OnNext(35);
				             		subj.OnNext(36);
				             		subj.OnNext(37);
				             		subj.OnNext(38);
				             		subj.OnNext(39);
				             		subj.OnCompleted();
				             		//return Disposable.Empty;
				             		return Disposable.Create(() =>
				             		                         	{
				             		                         		subj.OnNext(120); // Tricky, but it is not working.
				             		                         		Console.WriteLine("Disposable.Create action");
				             		                         	});
				             	});

			// Subscriber
			observable
				.Finally(() => Console.WriteLine("Finally action"))
				.Subscribe(next => Console.WriteLine("Next item: {0}", next),
						   ex => Console.WriteLine("Exception: {0}", ex.Message),
						   () => Console.WriteLine("Completed."));
		}
	}
}
