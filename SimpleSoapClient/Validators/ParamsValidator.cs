using System;
using System.IO;

namespace SimpleSoapClient.Validators
{
	public class ParamsValidator
	{
		public ParamsValidator()
		{
		}

		public void Validate(ParamsDto paramsDto)
		{
			CheckXmlFilePath(paramsDto.XmlFilePath);

			if (string.IsNullOrWhiteSpace(paramsDto.ActionUrl))
			{
				throw new ArgumentNullException(paramsDto.ActionUrl);
			}
			if (string.IsNullOrWhiteSpace(paramsDto.EndpointUrl))
			{
				throw new ArgumentNullException(paramsDto.EndpointUrl);
			}
			if (paramsDto.ParralelThreadsCount < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(paramsDto.ParralelThreadsCount),
					"Значение должно быть больше 1");
			}
		}

		private void CheckXmlFilePath(string xmlFilePath)
		{
			if (string.IsNullOrWhiteSpace(xmlFilePath))
			{
				throw new ArgumentNullException(nameof(xmlFilePath));
			}

			if (!File.Exists(xmlFilePath))
			{
				throw new FileNotFoundException($"XML файл с телом запроса не найден в {xmlFilePath}");
			}

			if (new FileInfo(xmlFilePath).Length == 0)
			{
				throw new IOException($"XML файл пуст: {xmlFilePath}");
			}
		}
	}

}
