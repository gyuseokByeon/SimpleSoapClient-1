using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSoapClient
{
	public class ParamsDto
	{
		public string XmlFilePath { get; set; }

		public string EndpointUrl { get; set; }

		public string ActionUrl { get; set; }

		public int ParralelThreadsCount { get; set; }
	}
}
