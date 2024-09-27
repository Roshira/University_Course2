using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	internal class AddVertexOREdgeGraph
	{
		private string filePath;

		public AddVertexOREdgeGraph(string filePath)
		{
			this.filePath = filePath;
		}
		public void WritingVertexOREdge()
		{
			Console.WriteLine("Writing one, two vertex and weight to add edge");
			Console.Write("One vertex: ");
			if (!int.TryParse(Console.ReadLine(), out int OneVertex))
			{
				Console.WriteLine("Invalid input for one vertex. Exiting method.");
				Console.ReadLine();
				return;
			}

			Console.Write("Two vertex: ");
			if (!int.TryParse(Console.ReadLine(), out int TwoVertex))
			{
				Console.WriteLine("Invalid input for two vertex. Exiting method.");
				Console.ReadLine();
				return;
			}

			Console.Write("Weight: ");
			if (!int.TryParse(Console.ReadLine(), out int Weight))
			{
				Console.WriteLine("Invalid input for weight. Exiting method.");
				Console.ReadLine();
				return;
			}
			AddEdge(OneVertex, TwoVertex, Weight);
		}
		private void AddEdge(int u, int v, int w)
		{
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

