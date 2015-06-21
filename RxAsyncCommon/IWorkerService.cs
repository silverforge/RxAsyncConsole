using System.ServiceModel;

namespace RxAsyncCommon
{
	[ServiceContract]
	public interface IWorkerService
	{
		[OperationContract]
		int GetDataLongWay();

		[OperationContract]
		int GetData1();

		[OperationContract]
		int GetData2();
	}
}
