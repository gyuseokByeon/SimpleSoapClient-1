using System;

namespace SimpleSoapClient.Helpers
{
	public class ArgumentParser
	{
		public ArgumentParser()
		{
		}

		public ParamsDto ParseArguments(string[] arguments)
		{
			if (arguments == null || arguments.Length == 0)
			{
				throw new ArgumentNullException(nameof(arguments));
			}

			var paramsDto = new ParamsDto
			{
				ActionUrl = arguments[2] ?? string.Empty,
				EndpointUrl = arguments[1] ?? string.Empty,
				XmlFilePath = arguments[0] ?? string.Empty,
				ParralelThreadsCount = 1 // default
			};

			if (arguments.Length == 4)
			{
				int.TryParse(arguments[3], out var parralelThreadsCount);
				paramsDto.ParralelThreadsCount = parralelThreadsCount;
			}

			return paramsDto;
		}
	}


}
