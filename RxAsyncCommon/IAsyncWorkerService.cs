using System.ServiceModel;

namespace RxAsyncCommon
{
	[ServiceContract]
	public interface IAsyncWorkerService
	{
		[OperationContract]
		int GetData1();

		[OperationContract]
		int GetData2();

		[OperationContract]
		int GetData3();
	}
}
