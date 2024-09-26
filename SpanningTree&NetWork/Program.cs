using System;
using System.Collections.Generic;

class Program
{
	static void Main(string[] args)
	{
		string filePath = "graph.txt";  // Ім'я файлу з графом

		// Створюємо екземпляр класу для зчитування графу
		GraphReader graphReader = new GraphReader(filePath);

		// Створюємо екземпляр класу для алгоритму Краскала
		KruskalAlgorithm kruskal = new KruskalAlgorithm();

		// Знаходимо мінімальне кістякове дерево
		List<Edge> mst = kruskal.FindMST(graphReader.Edges, graphReader.VerticesCount);

		// Виведення результату
		Console.WriteLine("Мінімальне кістякове дерево:");
		foreach (Edge edge in mst)
		{
			Console.WriteLine($"{edge.Source} - {edge.Destination}: {edge.Weight}");
		}
	}
}