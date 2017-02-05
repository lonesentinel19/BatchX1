using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchX {
	class Commandler {
		/* Replace basic strings, or otherwise capitalize them */
		public string basicReplace(string line) {
			Dictionary<string, int> occurences = new Dictionary<string, int> {};		
			IDictionary<string, string> replacements = new Dictionary<string, string> {
				{ "-START-", "@ECHO OFF"}, 
				{ "//", "REM"}, 
				{ "-STARTX-", "@ECHO off\r\nSETLOCAL ENABLEEXTENSIONS"},
				{ "-END-", "REM BatchX"},
				{ "-ENDX-", "REM BatchX"},
				{ "print", "ECHO" },
				{ "#", "REM" },
				{ "pause", "PAUSE" },
				//{ "for", "FOR" },
				{ "in", "IN" }
			};
			
			foreach (KeyValuePair<string, string> e in replacements)	{
				occurences[e.Key] = 0;
			}
			
			foreach (KeyValuePair<string, string> e in replacements)	{
				//if (extras.GetNthIndex(line, e.Key, occurences[e.Key]) > -1 && line.IndexOf("REM") != 0 )
				if ( line.IndexOf("REM") !=0 && line.IndexOf(e.Key) > -1 )
				{
					line = line.Replace(e.Key, e.Value);
				}
				//ocurrences[e.Key] += 1;
			}
			
			return line;			
		}
		
		/* Typically, I don't condone muting exceptions. But this exception
		 * occurs with zero-length strings, e.g when it cannot grab a 
		 * word to capitalize. 
		 * This function is to be CHANGED. From now on, any command from
		 * ss64 will be replaced, not just any string.
		 */
		public string capitalizeFirstWord(string line) {
			string commands = new string[128];
			foreach ( string command in commands ) { 
			/*
				find = line.Split(' ')[0];
				replace = line.Split(' ')[0].ToUpper();
				line = line.Replace(find, replace);
			*/
			}
			return line;
		}
		
	}
}