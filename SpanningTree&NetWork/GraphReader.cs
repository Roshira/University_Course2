using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// This is class for reading date with .txt file for KruskalAlgoritm
/// </summary>
public class GraphReader
{
	
	public List<Edge> Edges { get; private set; }
	public int VerticesCount { get; private set; }

	public GraphReader(string filePath)
	{
		Edges = new List<Edge>();
		ReadGraphFromFile(filePath);
	}

	private void ReadGraphFromFile(string filePath)
	{


		string[] lines = File.ReadAllLines(filePath);
		string[] firstLine = lines[0].Split();
		VerticesCount = int.Parse(firstLine[0]); // number vertex
		int edgesCount = int.Parse(firstLine[1]); // number edge

		for (int i = 2; i < lines.Length; i++)
		{
			string[] parts = lines[i].Split();
			int source = int.Parse(parts[0]);
			int destination = int.Parse(parts[1]);
			int weight = int.Parse(parts[2]);
			Edges.Add(new Edge(source, destination, weight));
		}
	}
}