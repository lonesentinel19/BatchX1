using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchX {
	class BatchX {
        public static string[] newLines { get; set; }
        public static string newFile { get; set; }
        public static string readFile { get; set; }
        public static string sversion = "1.0.0";
        public static int version = 100; 
        public static int currentLine = 0; 
		private static int threadSleep = 50; // in milliseconds

		static void Main(string[] args) {
			string fileIn = "", fileOut = "";
			int fsize = 0;
			
			if ( fsize > 4096 ) {
				Error.Throw(0);	
			}
			
			try { 
				fileIn = args[0];
				fileOut = args[1];
			} catch ( Exception e ) {				
				Error.Throw(1, currentLine);
			}
			newFile = fileOut;
			readFile = fileIn;
			
			fsize = System.IO.File.ReadLines(fileIn).Count();
			newLines = new string[fsize];
			read(fileIn);
		}
		
        /// <summary>
        /// This method accepts the filename and reads and modify()'s the file.
        /// </summary>
        /// <param name="filename"></param>
        static void read(string filename)
        {
            int i = 0;
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(readFile);

            while ((line = file.ReadLine()) != null)
            {
                newLines[i] = modify(line, i);
				System.Threading.Thread.Sleep(threadSleep);
                i++;
            }
			pushToFile(newFile, newLines);
            file.Close();
        }

		private static string modify(string line, int num) {
			var Com = new Commandler(line, num);
			/* Replace basic strings first */
			line = Com.preReplace(line);
			line = Com.basicReplace(line);
			
			/* Capitalize first word of commands */
			line = Com.capitalizeFirstWord(line);
			
            return line;
		}

		/// <summary>
		/// pushToFile - This will push the code that is param lines into global variable "newFile"
		/// </summary>
		/// <param name="lines">an array of lines to be written</param>
		static void pushToFile(string newFile, string[] lines)
		{
			System.IO.File.WriteAllLines(newFile, lines);
			Console.Write("Code transpiled and written to " + newFile + "\r\n");
			Console.Write("BatchX v." + sversion);
		}
	}
}
	