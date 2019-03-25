using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using SimpleSoapClient.Helpers;
using SimpleSoapClient.Validators;
using SoapClientLib;

namespace SimpleSoapClient
{
	internal static class Program
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

			var parser = new ArgumentParser();
			var cmdParams = parser.ParseArguments(args);

			var validator = new ParamsValidator();
			validator.Validate(cmdParams);

			var soapEnvelope = File.ReadAllText(cmdParams.XmlFilePath);

			var client = new SoapWebClient();

			var tasks = new List<Task<SoapResult>>();

			// todo define size

			for (var x = 0; x < cmdParams.ParralelThreadsCount; x++)
			{
				tasks.Add(new Task<SoapResult>(() =>
					client.GetResponseSoap(cmdParams.EndpointUrl, cmdParams.ActionUrl, soapEnvelope)));
			}

			tasks.ForEach(x => x.Start());

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
