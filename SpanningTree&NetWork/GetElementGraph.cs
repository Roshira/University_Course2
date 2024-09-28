using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	/// <summary>
	/// Class reader date that users send for program
	/// </summary>
	internal class GetElementGraph
	{
		public int[] WritingVertexOREdge()
		{
			int[] ints = new int[3];
			Console.WriteLine("Writing one, two vertex and weight");
			Console.Write("One vertex: ");
			if (!int.TryParse(Console.ReadLine(), out ints[0]))
			{
				Console.WriteLine("Invalid input for one vertex. Exiting method.");
				Console.ReadKey();
				return null;
			}

			Console.Write("Two vertex: ");
			if (!int.TryParse(Console.ReadLine(), out ints[1]))
			{
				Console.WriteLine("Invalid input for two vertex. Exiting method.");
				Console.ReadKey();
				return null;
			}

			Console.Write("Weight: ");
			if (!int.TryParse(Console.ReadLine(), out ints[2]))
			{
				Console.WriteLine("Invalid input for weight. Exiting method.");
				Console.ReadKey();
				return null;
			}

			return ints;
		}
	}
}
