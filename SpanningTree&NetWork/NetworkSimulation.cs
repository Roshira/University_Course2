using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	public class NetworkSimulation
	{
		private Dictionary<int, List<Edge>> graph = new Dictionary<int, List<Edge>>();  // Список суміжності для графа
		private int numVertices;  // Кількість вершин

		// Структура для представлення ребра
		public class Edge
		{
			public int Vertex, Weight;
			public Edge(int vertex, int weight)
			{
				Vertex = vertex;
				Weight = weight;
			}
		}

		// Читаємо мінімальне кістякове дерево з файлу
		public void ReadGraphFromFile(string filename)
		{
			var lines = File.ReadAllLines(filename);

			// Обробка першого рядка з кількістю вершин і ребер
			var firstLine = lines[0].Split();
			numVertices = int.Parse(firstLine[0]);  // Читаємо кількість вершин

			// Читаємо всі ребра
			for (int i = 1; i < lines.Length; i++)
			{
				var parts = lines[i].Split();
				int v1 = int.Parse(parts[0]);
				int v2 = int.Parse(parts[1]);
				int weight = int.Parse(parts[2]);

				if (!graph.ContainsKey(v1))
					graph[v1] = new List<Edge>();
				if (!graph.ContainsKey(v2))
					graph[v2] = new List<Edge>();

				graph[v1].Add(new Edge(v2, weight));
				graph[v2].Add(new Edge(v1, weight));  // Оскільки граф неорієнтований
			}
		}

		// Алгоритм Дейкстри для пошуку найкоротшого шляху від однієї вершини до всіх інших
		public Dictionary<int, int> Dijkstra(int start)
		{
			var distances = new Dictionary<int, int>();
			var priorityQueue = new SortedSet<(int, int)>();  // (distance, vertex)
			var visited = new HashSet<int>();

			// Ініціалізуємо всі вершини нескінченними відстанями
			for (int i = 0; i < numVertices; i++)
			{
				distances[i] = int.MaxValue;
			}

			distances[start] = 0;
			priorityQueue.Add((0, start));

			while (priorityQueue.Count > 0)
			{
				// Вибираємо вузол з мінімальною відстанню
				var current = priorityQueue.First();
				priorityQueue.Remove(current);

				int currentVertex = current.Item2;
				if (visited.Contains(currentVertex))
					continue;

				visited.Add(currentVertex);

				// Оновлюємо відстані до сусідніх вершин
				foreach (var edge in graph[currentVertex])
				{
					int neighbor = edge.Vertex;
					int newDist = distances[currentVertex] + edge.Weight;

					if (newDist < distances[neighbor])
					{
						distances[neighbor] = newDist;
						priorityQueue.Add((newDist, neighbor));
					}
				}
			}

			return distances;  // Повертаємо відстані від стартової вершини до всіх інших
		}

		// Моделювання передачі даних між вузлами
		public void SimulateDataTransfer(int source, int destination)
		{
			var distances = Dijkstra(source);
			if (distances[destination] == int.MaxValue)
			{
				Console.WriteLine($"Немає шляху між {source} та {destination}");
			}
			else
			{
				Console.WriteLine($"Найкоротший шлях між {source} та {destination}: {distances[destination]}");
				// У цьому місці можна додати моделювання передачі даних (наприклад, візуалізацію або логіку передачі).
			}
		}
	}

}
