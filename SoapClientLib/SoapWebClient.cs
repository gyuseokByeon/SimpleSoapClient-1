using System;
using System.IO;
using System.Net;

namespace SoapClientLib
{
	public class SoapWebClient : ISoapWebClient
	{
		/// <summary> Метод отправки запроса к SOAP сервису и получения от него ответа </summary>
		/// <param name="url">URL SOAP API-сервиса</param>
		/// <param name="action">URN Метода, который вызывается на API сервисе</param>
		/// <param name="soapEnvelope">XML SOAP-конверт (запрос), который будет отправлен к API</param>
		/// <returns>Метод возвращает ответ SOAP сервиса в виде XML</returns>
		public SoapResult GetResponseSoap(string url, string action, string soapEnvelope)
		{
			var result = new SoapResult();
			try
			{
				url = url.Trim('/').Trim('\\');

				var request = WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/soap+xml;charset=utf-8";
				request.Headers.Add("SOAPAction", action);
				request.Headers.Add("UserAgent", "Any");

				var streamWriter = new StreamWriter(request.GetRequestStream());
				streamWriter.Write(soapEnvelope);
				streamWriter.Close();

				using (var response = request.GetResponse())
				{
					var streamReader =
						new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
					result.Data = streamReader.ReadToEnd();
					streamReader.Close();
				}

				result.IsSucces = true;
				return result;
			}
			catch (WebException ex)
			{
				var responseBody = string.Empty;
				using (var stream = ex.Response?.GetResponseStream())
				{
					if (stream != null)
					{
						using (var reader = new StreamReader(stream))
						{
							responseBody = reader.ReadToEnd();
						}
					}
				}

				result.IsSucces = false;
				result.Error = new SoapError
				{
					IsException = true,
					Description = $"{ex.Status} - {ex.Message}",
					Exception = ex,
					ResponseData = responseBody,
				};

				return result;
			}
		}

	}
}
