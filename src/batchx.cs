using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchX {
	class BatchX {
        public static string[] newLines { get; set; }
        public static string newFile { get; set; }
        public static string sversion = "1.0.0";
        public static int version = 100; 
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
				Error.Throw(1);
			}
			newFile = fileIn;
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
            System.IO.StreamReader file = new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                newLines[i] = modify(line, i);
				System.Threading.Thread.Sleep(threadSleep);
                i++;
            }
            file.Close();
        }

		private static string modify(string line, int num) {
			
            check(newFile, num);
            return line;
		}
		
		/// <summary>
		/// Should never be called.
		/// </summary>
		/// <param name="i">Current number of lines read.</param>
		static void check(string file, int i)
		{
			int linec = System.IO.File.ReadLines(file).Count();
			if (linec == i+1)
			{
				pushToFile(file, newLines);
			}
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
	