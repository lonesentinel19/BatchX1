using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchX { 
	static class extras {
		
		public static int GetNthIndex(string s, string t, int n)
		{
			int count = 0;
			StringBuilder s2 = new StringBuilder(s);
			for (int i = 0; i < s2.Length; i++)
			{
				if (s2[i] == t)
				{
					count++;
					if (count == n)
					{
						return i;
					}
				}
			}
			return -1;
		}
	}
}