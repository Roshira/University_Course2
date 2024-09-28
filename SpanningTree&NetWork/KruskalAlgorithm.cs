using System;
using System.Collections.Generic;

/// <summary>
/// Implements Kruskal's algorithm to find the Minimum Spanning Tree (MST) of a graph.
/// </summary>
public class KruskalAlgorithm
{
	/// <summary>
	/// Finds the Minimum Spanning Tree from the given edges and vertex count.
	/// </summary>
	/// <param name="edges">A list of edges in the graph.</param>
	/// <param name="verticesCount">The total number of vertices in the graph.</param>
	/// <returns>A list of edges that form the MST.</returns>
	public List<Edge> FindMST(List<Edge> edges, int verticesCount)
	{
		// Initialize the result list to store MST edges
		List<Edge> result = new List<Edge>();
		edges.Sort();  // Sort edges by weight

		int[] parent = new int[verticesCount];
		int[] rank = new int[verticesCount];

		// Initialize sets for each vertex
		for (int i = 0; i < verticesCount; i++)
		{
			parent[i] = i;
			rank[i] = 0;
		}

		// Iterate through sorted edges
		foreach (Edge edge in edges)
		{
			int rootSource = Find(parent, edge.Source);
			int rootDestination = Find(parent, edge.Destination);

			// If adding this edge doesn't create a cycle
			if (rootSource != rootDestination)
			{
				result.Add(edge);
				Union(parent, rank, rootSource, rootDestination);
			}
		}

		return result; // Return the MST edges
	}

	/// <summary>
	/// Finds the root of the set containing the specified vertex.
	/// </summary>
	/// <param name="parent">Array representing the parent of each vertex.</param>
	/// <param name="i">The vertex to find the root for.</param>
	/// <returns>The root of the set.</returns>
	private int Find(int[] parent, int i)
	{
		if (parent[i] == i)
			return i;
		return parent[i] = Find(parent, parent[i]);
	}

	/// <summary>
	/// Unites two sets by linking their roots.
	/// </summary>
	/// <param name="parent">Array representing the parent of each vertex.</param>
	/// <param name="rank">Array representing the rank of each vertex.</param>
	/// <param name="x">The root of the first set.</param>
	/// <param name="y">The root of the second set.</param>
	private void Union(int[] parent, int[] rank, int x, int y)
	{
		int rootX = Find(parent, x);
		int rootY = Find(parent, y);

		if (rank[rootX] < rank[rootY])
			parent[rootX] = rootY;
		else if (rank[rootX] > rank[rootY])
			parent[rootY] = rootX;
		else
		{
			parent[rootY] = rootX;
			rank[rootX]++;
		}
	}
}

/// <summary>
/// Represents an edge in the graph with a source, destination, and weight.
/// Implements IComparable for sorting edges by weight.
/// </summary>
public class Edge : IComparable<Edge>
{
	public int Source, Destination, Weight;

	/// <summary>
	/// Initializes a new edge with the specified source, destination, and weight.
	/// </summary>
	/// <param name="source">The starting vertex of the edge.</param>
	/// <param name="destination">The ending vertex of the edge.</param>
	/// <param name="weight">The weight of the edge.</param>
	public Edge(int source, int destination, int weight)
	{
		Source = source;
		Destination = destination;
		Weight = weight;
	}

	/// <summary>
	/// Compares this edge with another edge based on their weights.
	/// </summary>
	/// <param name="other">The other edge to compare with.</param>
	/// <returns>A comparison value indicating the order of the edges.</returns>
	public int CompareTo(Edge other)
	{
		return this.Weight.CompareTo(other.Weight);
	}
}
