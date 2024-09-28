using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	/// <summary>
	/// Represents a network simulation for managing a graph and simulating data transfer.
	/// </summary>
	public class NetworkSimulation
	{
		private Dictionary<int, List<Edge>> graph = new Dictionary<int, List<Edge>>();  // Adjacency list for the graph
		private int numVertices;  // Number of vertices in the graph

		/// <summary>
		/// Represents an edge in the graph with a target vertex and weight.
		/// </summary>
		public class Edge
		{
			public int Vertex { get; }
			public int Weight { get; }

			/// <summary>
			/// Initializes a new instance of the <see cref="Edge"/> class.
			/// </summary>
			/// <param name="vertex">The target vertex of the edge.</param>
			/// <param name="weight">The weight of the edge.</param>
			public Edge(int vertex, int weight)
			{
				Vertex = vertex;
				Weight = weight;
			}
		}

		/// <summary>
		/// Reads a minimum spanning tree from a file and initializes the graph.
		/// </summary>
		/// <param name="filename">The name of the file containing the graph data.</param>
		public void ReadGraphFromFile(string filename)
		{
			var lines = File.ReadAllLines(filename);

			// Process the first line containing the number of vertices and edges
			var firstLine = lines[0].Split();
			numVertices = int.Parse(firstLine[0]);  // Read the number of vertices

			// Read all edges from the file
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
				graph[v2].Add(new Edge(v1, weight));  // Since the graph is undirected
			}
		}

		/// <summary>
		/// Implements Dijkstra's algorithm to find the shortest path from a starting vertex to all other vertices.
		/// </summary>
		/// <param name="start">The starting vertex.</param>
		/// <returns>A dictionary with the shortest distances from the starting vertex to all other vertices.</returns>
		public Dictionary<int, int> Dijkstra(int start)
		{
			var distances = new Dictionary<int, int>();
			var priorityQueue = new SortedSet<(int, int)>();  // (distance, vertex)
			var visited = new HashSet<int>();

			// Initialize all vertices with infinite distances
			for (int i = 0; i < numVertices; i++)
			{
				distances[i] = int.MaxValue;
			}

			distances[start] = 0;
			priorityQueue.Add((0, start));

			while (priorityQueue.Count > 0)
			{
				// Select the vertex with the minimum distance
				var current = priorityQueue.First();
				priorityQueue.Remove(current);

				int currentVertex = current.Item2;
				if (visited.Contains(currentVertex))
					continue;

				visited.Add(currentVertex);

				// Update distances to neighboring vertices
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

			return distances;  // Return the distances from the start vertex to all others
		}

		/// <summary>
		/// Simulates data transfer between nodes in the network.
		/// </summary>
		/// <param name="source">The source vertex for data transfer.</param>
		/// <param name="destination">The destination vertex for data transfer.</param>
		public void SimulateDataTransfer(int source, int destination)
		{
			var distances = Dijkstra(source);
			if (distances[destination] == int.MaxValue)
			{
				Console.WriteLine($"There is no path between {source} and {destination}");
			}
			else
			{
				Console.WriteLine($"The shortest path between {source} and {destination}: {distances[destination]}");
				// Here, additional data transfer simulation logic can be added (e.g., visualization or transfer logic).
			}
		}
	}
}
