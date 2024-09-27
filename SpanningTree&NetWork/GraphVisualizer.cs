using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	public class GraphVisualizer
	{
		private string inputFilePath;
		private string outputFilePath;

		public GraphVisualizer(string inputFilePath, string outputFilePath)
		{
			this.inputFilePath = inputFilePath;
			this.outputFilePath = outputFilePath;
		}

		public void GenerateGraphImage()
		{
			try
			{
				// Читаємо файл з даними графа
				string[] lines = File.ReadAllLines(inputFilePath);

				// Отримуємо кількість вершин та ребер
				string[] firstLine = lines[0].Split(' ');
				int vertexCount = int.Parse(firstLine[0]);
				int edgeCount = int.Parse(firstLine[1]);

				// Створюємо файл .dot для Graphviz
				string dotFilePath = "graph.dot";
				using (StreamWriter writer = new StreamWriter(dotFilePath))
				{
					writer.WriteLine("graph G {");

					// Додаємо ребра з файлу
					for (int i = 1; i <= edgeCount; i++)
					{
						string[] edge = lines[i].Split(' ');
						int from = int.Parse(edge[0]);
						int to = int.Parse(edge[1]);
						int weight = int.Parse(edge[2]);

						writer.WriteLine($"    {from} -- {to} [label={weight}];");
					}

					writer.WriteLine("}");
				}

				// Викликаємо команду dot для створення зображення
				Process process = new Process();
				process.StartInfo.FileName = "dot";
				process.StartInfo.Arguments = $"-Tpng {dotFilePath} -o {outputFilePath}";
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				process.WaitForExit();

				Console.WriteLine($"Graph create finish {outputFilePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error when program create graph: {ex.Message}");
			}
		}
	}
}
