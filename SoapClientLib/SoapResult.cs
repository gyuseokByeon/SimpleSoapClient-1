using System.Security.Cryptography.X509Certificates;

namespace SoapClientLib
{
	public class SoapResult : ISoapResult
	{
		public bool IsSucces { get; set; }

		public SoapError Error { get; set; }

		public string Data { get; set; }
	}
}