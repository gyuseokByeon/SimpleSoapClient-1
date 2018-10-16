namespace SoapClientLib
{
	public interface ISoapResult
	{
		string Data { get; set; }
		SoapError Error { get; set; }
		bool IsSucces { get; set; }
	}
}