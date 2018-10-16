namespace SoapClientLib
{
	public interface ISoapWebClient
	{
		SoapResult GetResponseSoap(string url, string action, string soapEnvelope);
	}
}