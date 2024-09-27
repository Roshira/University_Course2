using MassTransit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	internal class DeletePartGraph
	{
		private string filePath;
		public DeletePartGraph(string filePath)
		{
			this.filePath = filePath;
		}

		public void startDelete()
		{
			Delete();
		}

		private void Delete()
		{
			int u, v, w;
			int[] date = new int[3];
			GetElementGraph getElementGraph = new GetElementGraph();
			date = getElementGraph.WritingVertexOREdge();
			if (date == null)
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

			string[] lines = File.ReadAllLines(filePath);

			string[] firstLine = lines[0].Split();
			int vertices = int.Parse(firstLine[0]);
			int edges = int.Parse(firstLine[1]);

			bool exit = true;

			for (int i = 1; i < lines.Length; i++)
			{
				string[] edgeParts = lines[i].Split();
				int existingU = int.Parse(edgeParts[0]);
				int existingV = int.Parse(edgeParts[1]);
				int existingW = int.Parse(edgeParts[2]);

				if ((existingU == u && existingV == v) || (existingU == v && existingV == u))
				{
					exit = false;
					break;
				}
			}
			if (exit) {
				Console.WriteLine("We haven't this edge...");
				Console.ReadKey();
				return;
			}
			int uDelete = 0;
			int vDelete = 0;
			for (int i = 1; i < lines.Length; i++)
			{
				string[] InfoParts = lines[i].Split();
				int existingU = int.Parse(InfoParts[0]);
				int existingV = int.Parse(InfoParts[1]);

				if (existingU == u || existingV == u)
					uDelete++;
				if(existingV == v || existingU == v)
					vDelete++;
			}
			if (uDelete == 1)
				vertices--;
			if(vDelete == 1) 
				vertices--;

			using (StreamWriter writer = new StreamWriter(filePath))
			{
				// Записуємо оновлений перший рядок з новою кількістю вершин і ребер
				edges--;
				writer.WriteLine($"{vertices} {edges}");

				// Перезаписуємо існуючі ребра
				for (int i = 1; i < lines.Length; i++)
				{
					if ((lines[i] != $"{u} {v} {w}" )&&( lines[i] != $"{v} {u} {w}"))
					writer.WriteLine(lines[i]);
				}

			}
		}

	}
}
