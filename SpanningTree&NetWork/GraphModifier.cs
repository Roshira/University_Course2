using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	internal class GraphModifier
	{
		private string filePath;

		public GraphModifier(string filePath)
		{
			this.filePath = filePath;
		}

		internal void AddStart()
		{
			Add();
		}

		private void Add()
		{
			int u, v, w;
			int[] date = new int[3];
		    GetElementGraph getElementGraph = new GetElementGraph();
			date = getElementGraph.WritingVertexOREdge();
			if(date == null)
			{
				Console.WriteLine("You dont writing correct date");
				Console.ReadKey();
				return;
			}
			else
			{
				u = date[0];
				v = date[1];
				w = date[2];
			}
			if (u == v || w == 0)
				return;
			// Читання всіх рядків з файлу
			string[] lines = File.ReadAllLines(filePath);

			string[] firstLine = lines[0].Split();
			int vertices = int.Parse(firstLine[0]);
			int edges = int.Parse(firstLine[1]);

			for (int i = 1; i < lines.Length; i++)
			{
				string[] edgeParts = lines[i].Split();
				int existingU = int.Parse(edgeParts[0]);
				int existingV = int.Parse(edgeParts[1]);
				int existingW = int.Parse(edgeParts[2]);

				if (existingU == u && existingV == v)
					return;
				else if (existingU == v && existingV == u)
					return;
			}


			edges++; 

			int maxVertex = Math.Max(u, v) + 1;
			if (maxVertex > vertices)
			{
				vertices = maxVertex;
			}

			string newEdge = $"{u} {v} {w}";

			// Відкриваємо файл для запису (перезаписуємо всі дані)
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				// Записуємо оновлений перший рядок з новою кількістю вершин і ребер
				writer.WriteLine($"{vertices} {edges}");

				// Перезаписуємо існуючі ребра
				for (int i = 1; i < lines.Length; i++)
				{
					writer.WriteLine(lines[i]);
				}

				// Додаємо нове ребро
				writer.WriteLine(newEdge);
			}
		}

	}
}

