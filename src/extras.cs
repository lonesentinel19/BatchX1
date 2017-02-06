using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchX { 
	static class extras {
		/*
		public static IEnumerable<int> AllIndexesOf(this string str, string searchstring) {
			int minIndex = str.IndexOf(searchstring);
			while (minIndex != -1)
			{
				yield return minIndex;
				minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
			}
		}*/
		
		public static List<int> AllIndexesOf(string str, string value) {
			if (String.IsNullOrEmpty(value))
				throw new ArgumentException("the string to find may not be empty", "value");
			List<int> indexes = new List<int>();
			for (int index = 0;; index += value.Length) {
				index = str.IndexOf(value, index);
				if (index == -1)
					return indexes;
				indexes.Add(index);
			}
		}
		
		// Change FIND to uppercase, then replace
		public static string Replace(string str, string find, string replace) {
			try {
				str = str.Replace(find.ToUpper(), replace);
			} catch ( Exception e ) {
				// There is no Error.Throw available yet
			}
			return str;
		}
		
		// Not REM replace
		public static string NotREMReplace(string line, string find, string replace) {
			string splitLine = "", splitLineRest = "";	
			if ( line.IndexOf("REM") > -1 ) {
				splitLine = line.Split(new string[]{"REM"},StringSplitOptions.None)[0];					
				splitLineRest = "REM" + line.Split(new string[]{"REM"},StringSplitOptions.None)[1];					
				} else {
				splitLine = line;
				splitLineRest = "";
			}
			return splitLine + splitLineRest;
		}
	}
}