using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchX {
	class Error {
		public static string Throw(int errCode, string errString = "", int line = 0) {
            string err =""; 
			string[] codes = { 
				"File size exceeds 4096 lines, please split into many files.", // 0
				"Too few arguments provided. You must provide at least two arguments.", 
				"Encountered an improper"
				};
            if ( errCode > -1 ) {
                Console.Write("ERROR: " + codes[errCode] + "\r\n\r\n");
            }
            return "REM Error " + codes[errCode];
		}
	}
}