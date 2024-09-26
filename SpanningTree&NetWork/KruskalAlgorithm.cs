using System;
using System.Collections.Generic;

public class KruskalAlgorithm
{
	public List<Edge> FindMST(List<Edge> edges, int verticesCount)
	{
		List<Edge> result = new List<Edge>();
		edges.Sort();  // Сортуємо ребра за вагою

		int[] parent = new int[verticesCount];
		int[] rank = new int[verticesCount];

		// Ініціалізуємо множини для кожної вершини
		for (int i = 0; i < verticesCount; i++)
		{
			parent[i] = i;
			rank[i] = 0;
		}

		// Проходимо по всіх відсортованих ребрах
		foreach (Edge edge in edges)
		{
			int rootSource = Find(parent, edge.Source);
			int rootDestination = Find(parent, edge.Destination);

			// Якщо включення цього ребра не створює цикл
			if (rootSource != rootDestination)
			{
				result.Add(edge);
				Union(parent, rank, rootSource, rootDestination);
			}
		}

		return result;
	}

	// Знаходимо корінь для певної вершини
	private int Find(int[] parent, int i)
	{
		if (parent[i] == i)
			return i;
		return parent[i] = Find(parent, parent[i]);
	}

	// Об'єднуємо дві множини
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

public class Edge : IComparable<Edge>
{
	public int Source, Destination, Weight;

	public Edge(int source, int destination, int weight)
	{
		Source = source;
		Destination = destination;
		Weight = weight;
	}

	public int CompareTo(Edge other)
	{
		return this.Weight.CompareTo(other.Weight);
	}
}