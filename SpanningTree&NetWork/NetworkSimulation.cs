using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
		/// Represents an edge in the graph with a target vertex and weight (as maximum data transfer capacity).
		/// </summary>
		public class Edge
		{
			public int Vertex { get; }
			public int Weight { get; }  // Max data transfer capacity (MB/s)

			/// <summary>
			/// Initializes a new instance of the <see cref="Edge"/> class.
			/// </summary>
			/// <param name="vertex">The target vertex of the edge.</param>
			/// <param name="weight">The weight of the edge (as data transfer capacity in MB/s).</param>
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
				int weight = int.Parse(parts[2]);  // Weight is the max data transfer rate in MB/s

				if (!graph.ContainsKey(v1))
					graph[v1] = new List<Edge>();
				if (!graph.ContainsKey(v2))
					graph[v2] = new List<Edge>();

				graph[v1].Add(new Edge(v2, weight));
				graph[v2].Add(new Edge(v1, weight));  // Since the graph is undirected
			}
		}

		/// <summary>
		/// Implements Dijkstra's algorithm to find the shortest path based on minimum transfer time.
		/// </summary>
		/// <param name="start">The starting vertex.</param>
		/// <param name="dataSize">The amount of data to transfer in MB.</param>
		/// <returns>A dictionary with the minimum transfer time from the starting vertex to all other vertices.</returns>
		public Dictionary<int, double> Dijkstra(int start, int dataSize)
		{
			var transferTime = new Dictionary<int, double>();
			var priorityQueue = new SortedSet<(double, int)>();  // (time, vertex)
			var visited = new HashSet<int>();

			// Initialize all vertices with infinite transfer time
			for (int i = 0; i < numVertices; i++)
			{
				transferTime[i] = double.MaxValue;
			}

			transferTime[start] = 0;
			priorityQueue.Add((0, start));

			while (priorityQueue.Count > 0)
			{
				// Select the vertex with the minimum transfer time
				var current = priorityQueue.First();
				priorityQueue.Remove(current);

				int currentVertex = current.Item2;
				if (visited.Contains(currentVertex))
					continue;

				visited.Add(currentVertex);

				// Update transfer time to neighboring vertices
				foreach (var edge in graph[currentVertex])
				{
					int neighbor = edge.Vertex;
					// Calculate the transfer time over this edge
					double edgeTransferTime = (double)dataSize / edge.Weight;  // Time = DataSize / Bandwidth

					double newTime = transferTime[currentVertex] + edgeTransferTime;

					if (newTime < transferTime[neighbor])
					{
						transferTime[neighbor] = newTime;
						priorityQueue.Add((newTime, neighbor));
					}
				}
			}

			return transferTime;  // Return the minimum transfer times from the start vertex to all others
		}

		/// <summary>
		/// Simulates data transfer between nodes in the network.
		/// </summary>
		/// <param name="source">The source vertex for data transfer.</param>
		/// <param name="destination">The destination vertex for data transfer.</param>
		/// <param name="dataSize">The amount of data to transfer in MB.</param>
		public void SimulateDataTransfer(int source, int destination, int dataSize)
		{
			var transferTimes = Dijkstra(source, dataSize);
			if (transferTimes[destination] == double.MaxValue)
			{
				Console.WriteLine($"There is no path between {source} and {destination}.");
			}
			else
			{
				Console.WriteLine($"Time required to transfer {dataSize} MB between {source} and {destination}: {transferTimes[destination]:F2} seconds.");
			}
		}
	}
}
