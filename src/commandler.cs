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
		public string functionReplace(string line) {
			string[] functionList = new string[] { "arread", "arrval" };
			int i = 0;
			Dictionary<int,string> oc = new Dictionary<int, string>{};
			Dictionary<int,int> occ = new Dictionary<int, int>{};
			SortedDictionary<int,string> o4 = new SortedDictionary<int,string>{};
			List<int> c = extras.AllIndexesOf(line, ")");
			
			foreach ( string function in functionList ) {
				List<int> o = extras.AllIndexesOf(line, function);
				foreach ( int t in o ) {
					oc[t] = function;
					occ[t] = i;
					i++;
				}
			}

			var oc_list = oc.Keys.ToList();
			oc_list.Sort();

			foreach ( var key in oc_list ) {
				o4[i-occ[key]] = line.Substring(key, Convert.ToInt32(c[occ[key]]) - key + (i-occ[key]));
			}
				
			foreach ( KeyValuePair<int,string> e in o4 ) {
				string functionizerParam = e.Value.Substring(0, e.Value.Length - i + e.Key);
				line = functionizer(functionizerParam, line);
			}
			
			return line;
		}
		
		public string functionizer(string extract, string line) {
			string name = extract.TrimEnd(')').Split('(')[0]; // we don't need final parentheses
			string parameter = extract.TrimEnd(')').Split('(')[1];
			
			MethodInfo m = this.GetType().GetMethod(name);
			string r = m.Invoke(this, new string[] { parameter, line }).ToString();

			return r;
		}
		
		public string arread(string param, string line){return "arrval";}
		public string arrval(string param, string line){return "";}
	}
}