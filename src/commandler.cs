using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static BatchX.extras;

namespace BatchX {
	class Commandler {
		
		public int cNum;
		public string cLine;
        public Dictionary<string, string> transpilerVariables = new Dictionary<string, string> { };
		public int cI;
		Method M = new Method();
		
		/* Gets important information regarding current line */
		public Commandler(string line, int num) {
			cNum = num;
			cLine = line;
		}
		
		/* These need to be replaced before the basic replacements */
		public string preReplace(string line) {
			Dictionary<string, string> replacements = new Dictionary<string, string> {
				{ "//", "REM"}, 
				{ "#", "REM" }
			};
			foreach ( KeyValuePair<string,string> e in replacements ) {
				line = line.Replace(e.Key, e.Value);
			}
			return line;
		}

		/* This splits the line by the delimiter REM, if REM occurs in the
		 * line. If so, then it splits and replaces everything BEFORE REM
		 * with the desired replacement. Anything after REM is tacked on
		 * in the return line. If REM does not occur, then it simply defaults
		 * to using the variable line and does not split.
		 */
		public string basicReplace(string line) {
			string splitLine = "", splitLineRest = "";	
			IDictionary<string, string> replacements = new Dictionary<string, string> {
				{ "-START-", "@ECHO OFF"}, 
				//{ "//", "REM"}, 
				{ "-STARTX-", "@ECHO off\r\nSETLOCAL ENABLEEXTENSIONS"},
				{ "-END-", "REM BatchX"},
				{ "-ENDX-", "REM BatchX"},
				{ "PRINT", "ECHO" },
				{ "IMPORT", "CALL" }
			};
			if ( line.IndexOf("REM") > -1 ) {
				splitLine = line.Split(new string[]{"REM"},StringSplitOptions.None)[0];					
				splitLineRest = "REM" + line.Split(new string[]{"REM"},StringSplitOptions.None)[1];					
				} else {
				splitLine = line;
				splitLineRest = "";
			}
			
			foreach (KeyValuePair<string, string> e in replacements)	{				
				if ( splitLine.IndexOf(e.Key) > -1 )
				{
					splitLine = extras.Replace(splitLine, e.Key, e.Value);
				}				
			}
			
			return splitLine + splitLineRest;			
		}
				
		/* Typically, I don't condone muting exceptions. But this exception
		 * occurs with zero-length strings, e.g when it cannot grab a 
		 * word to capitalize. 
		 * This function is to be CHANGED. From now on, any command from
		 * ss64 will be replaced, not just any string.
		 */
		public string capitalizeFirstWord(string line) {
			//string commands = new string[128];
			//foreach ( string command in commands ) { 
			/*
				find = line.Split(' ')[0];
				replace = line.Split(' ')[0].ToUpper();
				line = line.Replace(find, replace);
			*/
			//}
			return line;
		}
		
		/* TRANSPILER VARIABLES */
		public string transpilerSet(string line) {
			if ( line.IndexOf('$') > -1 ) {
				string dollarSplit = line.Split('$')[1];
				string key = dollarSplit.Split('=')[0];
				string val = dollarSplit.Split('=')[1];
				try {
					transpilerVariables[key] = val;
					line = "REM transpiler variable set";
				} catch ( Exception e ) {
					line = Error.Throw(3, cNum, e.ToString());
				}
			}
			return line;
		}
		
		/* Abandon all hope ye who enter here */
		public SortedDictionary<int,string> getAllFunctions(string line) {
			string[] functionList = new string[] { "arread", "arrval" };
			int i = 0;
			string toReturn = "";
			Dictionary<int,string> oc = new Dictionary<int, string>{};
			Dictionary<int,int> occ = new Dictionary<int, int>{};
			SortedDictionary<int,string> o4 = new SortedDictionary<int,string>{};
			List<int> c = extras.AllIndexesOf(cLine, ")");
			
			foreach ( string function in functionList ) {
				List<int> o = extras.AllIndexesOf(cLine, function);
				foreach ( int t in o ) {
					oc[t] = function;
					occ[t] = i;
					i++;
				}
			}

			var oc_list = oc.Keys.ToList();
			oc_list.Sort();

			foreach ( var key in oc_list ) {
				o4[i-occ[key]] = cLine.Substring(key, Convert.ToInt32(c[occ[key]]) - key + (occ[key])-i);
			}
			/*
			foreach ( KeyValuePair<int,string> e in o4 ) {
				if ( e.Key == toGet ) {
					toReturn = e.Value;
				}
			}*/
			return o4;
		}			

		public string functionizer(string extract, string line) {
			string r = String.Empty;
			if ( extract != "" && line != "" ) {
				string name = extract.Split('(')[0];
				MethodInfo m = this.GetType().GetMethod(name);
				r = m.Invoke(this, new string[] { extract, line }).ToString();
			}
			return r;
		}
		
		public int numOfFunctions(string line) {
			List<int> AIOLeftP  = extras.AllIndexesOf(line, "(");
			cI = AIOLeftP.Count();
			return cI;
		}
		
		public string newFunctionReplace(string line, int toGet, int antiToGet) {
			string[] functionList = new string[] { "arread", "arrval", "tar" };
			string parameters = "", functionName = "";
			
			int location = 0;
			List<int> AIOLeftP  = extras.AllIndexesOf(line, "(");
			List<int> AIORightP = extras.AllIndexesOf(line, ")");
			
			int first = AIOLeftP[cI-toGet];
			int last = AIORightP[toGet-1];
			
			Console.WriteLine(first + "-" + last);
			try {
				parameters = line.Substring(first, last-first+1);
				functionName = line.Substring(first-8, last-first+8);
			} catch ( Exception e ) {}
			
			foreach ( string function in functionList ) {
				try {
					if ( line.IndexOf(function) > -1 ) { 
						location = line.IndexOf(function + parameters);
					}
				} catch ( Exception e ) {
					location = -1;
				}
			}
			
			if ( location >-1 ) {
				string completeFunction = line.Substring(location, last-location+1).Replace("))", ")");
				string callFunction = completeFunction.Split('(')[0];
				
				MethodInfo m = this.GetType().GetMethod(callFunction);
				string r = m.Invoke(this, new string[] { completeFunction, line }).ToString();
				
				line = r;
				//line = line.Replace(completeFunction, ">T<");
			}
			
			return line;
		}
		public string tar(string param, string line ) { 
			return line.Replace(param.Trim(), ""); 
			}
			
		public string arread(string param, string line) {
			string[] args = M.ExtractParams("arread", param);
			line = line.Replace(param.Trim(), args[0]);
			return line;
		}
		
		public string arrval(string param, string line) {
			string[] args = M.ExtractParams("arrval", param);
			line = line.Replace(param.Trim(), args[0]);
			return line;
		}
	}
}