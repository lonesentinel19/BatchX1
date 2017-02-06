using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BatchX.extras;

namespace BatchX {
	class Commandler {
		public int cNum;
		public string cLine;
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
	}
}