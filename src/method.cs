using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchX {
	class Method {
		public string[] ExtractParams(string method, string allParams) {
			allParams = allParams.Substring(method.Length, allParams.Length - method.Length).Replace("(", "").TrimEnd(')');
			string[] returnParams = allParams.Split(',');
			return returnParams;
		}
	}
}