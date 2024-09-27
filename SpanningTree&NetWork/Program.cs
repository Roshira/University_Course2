using MassTransit.SagaStateMachine;
using SpanningTree_NetWork;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
	static void Main(string[] args)
	{
		string filePath = "C:\\University_Course2\\SpanningTree&NetWork\\Graph.txt";  // Ім'я файлу з графом

		// Створюємо екземпляр класу для зчитування графу
		
		string click = " <---";

		int temp = 1;  // Стартова позиція в меню
		bool running = true;  // Флаг для контролю циклу

		while (running)
		{
			Console.Clear();  // Очищуємо консоль перед виведенням меню

			// Меню
			string printGraph = "1. Print MST";
			string addEdge = "2. Add the edge or vertex";
			string addVertex = "3. Delete the Edge";
			string deleteEdge = "4. Not realized";
			string deleteVertex = "5. Not realized";
			string exit = "6. Exit";

			// Логіка управління меню
			switch (temp)
			{
				case 1:
					printGraph += click;
					break;
				case 2:
					addEdge += click;
					break;
				case 3:
					addVertex += click;
					break;
				case 4:
					deleteEdge += click;
					break;
				case 5:
					deleteVertex += click;
					break;
				case 6:
					exit += click;
					break;
			}

			// Виведення меню
			Console.WriteLine(printGraph);
			Console.WriteLine(addEdge);
			Console.WriteLine(addVertex);
			Console.WriteLine(deleteEdge);
			Console.WriteLine(deleteVertex);
			Console.WriteLine(exit);


			// Зчитування натискання клавіш
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			if (keyInfo.Key == ConsoleKey.S)
			{
				temp++;
				if (temp > 6) temp = 1;  // Переходить на перший пункт, якщо перевищує кількість пунктів
			}
			else if (keyInfo.Key == ConsoleKey.W)
			{
				temp--;
				if (temp < 1) temp = 6;  // Переходить на останній пункт, якщо йде вгору за перший
			}

			else if (keyInfo.Key == ConsoleKey.Enter)
			{
				Console.Clear();
				switch (temp)
				{
					case 1:

						GraphReader graphReader = new GraphReader(filePath);

						// Створюємо екземпляр класу для алгоритму Краскала
						KruskalAlgorithm kruskal = new KruskalAlgorithm();

						// Знаходимо мінімальне кістякове дерево
						List<Edge> mst = kruskal.FindMST(graphReader.Edges, graphReader.VerticesCount);
						Console.WriteLine("MST:");
						foreach (Edge edge in mst)
						{
							Console.WriteLine($"{edge.Source} - {edge.Destination}: {edge.Weight}");
						}
						Console.WriteLine("\nPress any key to go back");
						Console.ReadKey();
						break;
					case 2:
						GraphModifier modifier = new GraphModifier(filePath);
						modifier.AddStart();
						break;
					case 3:
						DeletePartGraph deletePartGraph = new DeletePartGraph(filePath);
						deletePartGraph.startDelete();
						
						break;
					case 4:
						string inputFilePath = "C:\\University_Course2\\SpanningTree&NetWork\\Graph.txt";
						string outputFilePath = "C:\\University_Course2\\SpanningTree&NetWork\\graph.png";
						GraphVisualizer graphVisualizer = new GraphVisualizer(inputFilePath, outputFilePath);

						graphVisualizer.GenerateGraphImage();
						break;
					case 5:

						break;
					case 6:
						running = false; 
						break;
				}
			}
		}
		Console.Clear();
		Console.WriteLine("Програма завершена.");
	}
}
