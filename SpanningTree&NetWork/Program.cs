using MassTransit.SagaStateMachine;
using SpanningTree_NetWork;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Main class for the program that provides a user interface for interacting with graph data.
/// </summary>
class Program
{
	/// <summary>
	/// The entry point of the program.
	/// </summary>
	/// <param name="args">Command-line arguments (not used).</param>
	static void Main(string[] args)
	{
		string filePath = "C:\\University_Course2\\SpanningTree&NetWork\\FilesWithDate\\Graph.txt";
		string filePathMST = "C:\\University_Course2\\SpanningTree&NetWork\\FilesWithDate\\ForMST.txt";
		// Create an instance of the class for reading the graph

		string click = " <---";

		int temp = 1;  // Starting position in the menu
		bool running = true;  // Flag for controlling the loop

		while (running)
		{
			Console.Clear();  // Clear the console before displaying the menu

			// Menu options
			string printGraph = "1. Print and create MST";
			string addEdge = "2. Add the edge or vertex";
			string deleteEdge = "3. Delete the Edge";
			string createImage = "4. Create image graphs";
			string simulateNetwork = "5. Simulate network way";
			string exit = "6. Exit";

			// Menu control logic
			switch (temp)
			{
				case 1:
					printGraph += click;
					break;
				case 2:
					addEdge += click;
					break;
				case 3:
					deleteEdge += click;
					break;
				case 4:
					createImage += click;
					break;
				case 5:
					simulateNetwork += click;
					break;
				case 6:
					exit += click;
					break;
			}

			// Display the menu
			Console.WriteLine(printGraph);
			Console.WriteLine(addEdge);
			Console.WriteLine(deleteEdge);
			Console.WriteLine(createImage);
			Console.WriteLine(simulateNetwork);
			Console.WriteLine(exit);

			// Reading key presses
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			if (keyInfo.Key == ConsoleKey.S)
			{
				temp++;
				if (temp > 6) temp = 1;  // Move to the first option if exceeding the number of options
			}
			else if (keyInfo.Key == ConsoleKey.W)
			{
				temp--;
				if (temp < 1) temp = 6;  // Move to the last option if going up past the first
			}

			else if (keyInfo.Key == ConsoleKey.Enter)
			{
				Console.Clear();
				switch (temp)
				{
					case 1:
						// Create an instance of GraphReader to read the graph
						GraphReader graphReader = new GraphReader(filePath);

						// Create an instance of KruskalAlgorithm
						KruskalAlgorithm kruskal = new KruskalAlgorithm();

						// Find the minimum spanning tree (MST)
						List<Edge> mst = kruskal.FindMST(graphReader.Edges, graphReader.VerticesCount);
						Console.WriteLine("MST:");
						int edgeTemp = 0;
						for (int i = 0; i < mst.Count; i++)
						{
							Edge edge = mst[i];
							Console.WriteLine($"{edge.Source} - {edge.Destination}: {edge.Weight}");
							edgeTemp++;
						}
						string[] lines = File.ReadAllLines(filePath);
						string[] firstLine = lines[0].Split();
						int vertices = int.Parse(firstLine[0]);
						using (StreamWriter writer = new StreamWriter(filePathMST))
						{
							writer.WriteLine($"{vertices} {edgeTemp}");
							foreach (Edge edge in mst)
							{

								writer.WriteLine($"{edge.Source} {edge.Destination} {edge.Weight}");
							}
						}
							Console.WriteLine("\nPress any key to go back");
						Console.ReadKey();
						break;

					case 2:
						// Modify the graph by adding an edge or vertex
						GraphModifier modifier = new GraphModifier(filePath);
						modifier.AddStart();
						break;

					case 3:
						// Delete an edge from the graph
						DeletePartGraph deletePartGraph = new DeletePartGraph(filePath);
						deletePartGraph.startDelete();
						break;

					case 4:
						// Generate images of the graphs
						string outputFilePath = "C:\\University_Course2\\SpanningTree&NetWork\\FolderWithImageGraph\\graph.png";
						GraphVisualizer graphVisualizer = new GraphVisualizer(filePath, outputFilePath);
						graphVisualizer.GenerateGraphImage();

						string outputFilePathMST = "C:\\University_Course2\\SpanningTree&NetWork\\FolderWithImageGraph\\GraphMST.png";
						GraphVisualizer graphVisualizerMST = new GraphVisualizer(filePathMST, outputFilePathMST);
						graphVisualizerMST.GenerateGraphImage();
						break;

					case 5:
						// Simulate network data transfer
						NetworkSimulation network = new NetworkSimulation();
						network.ReadGraphFromFile(filePathMST);  // Read the graph from the file

						Console.WriteLine("Enter starting vertex:");
						int start = int.Parse(Console.ReadLine());
						Console.WriteLine("Enter ending vertex:");
						int end = int.Parse(Console.ReadLine());

						// Simulate data transfer between two nodes
						network.SimulateDataTransfer(start, end);

						Console.ReadKey();
						break;

					case 6:
						// Exit the program
						running = false;
						break;
				}
			}
		}
		Console.Clear();
		Console.WriteLine("Program terminated.");
	}

}