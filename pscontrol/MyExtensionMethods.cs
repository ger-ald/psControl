using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pscontrol
{
	static class MyExtensionMethods
	{
		public static void ClearQueue<T>(this BlockingCollection<T> queue)
		{
			while (queue.Count > 0) queue.Take();
		}
	}
}
