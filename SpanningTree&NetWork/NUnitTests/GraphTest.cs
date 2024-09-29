using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

[TestFixture]
public class GraphTest
{
	[Test]
	public void TestKruskalAlgorithm()
	{
		string filePathNew = "data_file.txt";
		string filePath = "C:\\University_Course2\\SpanningTree&NetWork\\FilesWithDate\\Graph.txt";
		// Дані для запису у файл
		string data = @"7 6
						4 5 2
						1 2 3
						0 4 4
						2 0 5
						0 3 6
						5 6 7";

		// Запис даних у файл
		File.WriteAllText(filePathNew, data);
		string[] lines = File.ReadAllLines(filePathNew); // Read file lines

		// Parse the first line for vertex and edge counts
		string[] firstLine = lines[0].Split();
		int vertices = int.Parse(firstLine[0]);
		int edges = int.Parse(firstLine[1]);

		GraphReader graphReader = new GraphReader(filePath);

		// Create an instance of KruskalAlgorithm
		KruskalAlgorithm kruskal = new KruskalAlgorithm();

		// Find the minimum spanning tree (MST)
		List<Edge> mst = kruskal.FindMST(graphReader.Edges, graphReader.VerticesCount);
		Assert.That(mst.Count, Is.EqualTo(edges));
		for (int i = 0; i < mst.Count; i++)
		{
			Edge edge = mst[i];
			string[] edgeData = lines[i + 1].Trim().Split(); 
			Assert.That(edge.Source, Is.EqualTo(int.Parse(edgeData[0]))); 
			Assert.That(edge.Destination, Is.EqualTo(int.Parse(edgeData[1]))); 
			Assert.That(edge.Weight, Is.EqualTo(int.Parse(edgeData[2])));

		}
		if (File.Exists(filePathNew))
		{
			File.Delete(filePathNew);
		}
	}



}