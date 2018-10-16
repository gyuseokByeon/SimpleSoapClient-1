using System;

namespace SoapClientLib
{
	public interface ISoapError
	{
		string Description { get; set; }
		Exception Exception { get; set; }
		bool IsException { get; set; }
		string ResponseData { get; set; }
	}
}