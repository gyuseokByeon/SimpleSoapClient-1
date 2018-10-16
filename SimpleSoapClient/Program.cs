using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using SoapClientLib;

namespace SimpleSoapClient
{
	class Program
	{
		/// <summary>
		/// Can be run with arguments: -pass/to/xml, url, method, -parralel_sends_count (optional)
		/// </summary>
		/// <param name="args"></param>
		static int Main(string[] args)
		{

			if (args.Length > 4 || args.Length < 3)
			{
				Console.WriteLine("Wrong number of params. " +
				                    "Should be supplied 3 or 4 params: " +
				                    "1. xml file path, " +
				                    "2. request url, " +
				                    "3. action url, " +
				                    "4. parralel request count (optional)");
				Console.WriteLine(@"Usage example: text.xml https://somedomain.info:443/SomeService.svc Name.Space/SomeServise/ActionMethod 1");
				return 1;
			}
			var xmlFilePath = args[0];
			var requestUrl = args[1];
			var actionUrl = args[2];

			var parralelsCount = 1;

			if (args.Count() == 4)
			{
				int.TryParse(args[3], out parralelsCount);
			}


			if (!File.Exists(xmlFilePath))
			{
				Console.WriteLine($"File not exists {xmlFilePath}");
				Thread.Sleep(2000);
				return 0;
			}

			var soapEnvelope = File.ReadAllText(xmlFilePath);

			if (string.IsNullOrWhiteSpace(soapEnvelope))
			{
				throw new ArgumentNullException($"File {xmlFilePath} is empty");
			}

			var client = new SoapWebClient();

			var tasks = new List<Task<SoapResult>>();

			// todo define size

			for (var x = 0; x < parralelsCount; x++)
			{
				tasks.Add(new Task<SoapResult>(() =>
					client.GetResponseSoap(requestUrl,actionUrl, soapEnvelope)));
			}

			tasks.ForEach(x=>x.Start());

			Task.WaitAll(tasks.ToArray());

			foreach (var task in tasks)
			{
				if (string.IsNullOrWhiteSpace(task.Result.Data)) { continue; }

				if (!task.Result.IsSucces && task.Result.Error != null)
				{
					Console.WriteLine($"Error: {task.Result.Error.Description}");
				}
				var xml = XDocument.Parse(task.Result.Data ?? task.Result.Error.ResponseData);
				Console.WriteLine(xml.ToString());
				Console.WriteLine("__________________________________________________________\n");
			}

			Console.ReadKey();
			return 0;
		}
	}
}
