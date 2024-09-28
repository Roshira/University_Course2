using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SpanningTree_NetWork
{
	/// <summary>
	/// Class for visualizing a graph by generating an image from a .dot file using Graphviz.
	/// </summary>
	public class GraphVisualizer
	{
		private string inputFilePath;   // Path to the input file containing graph data
		private string outputFilePath;  // Path where the output image will be saved

		/// <summary>
		/// Initializes a new instance of the GraphVisualizer class.
		/// </summary>
		/// <param name="inputFilePath">The file path to read the graph data from.</param>
		/// <param name="outputFilePath">The file path to save the generated graph image.</param>
		public GraphVisualizer(string inputFilePath, string outputFilePath)
		{
			this.inputFilePath = inputFilePath; // Set the input file path
			this.outputFilePath = outputFilePath; // Set the output file path
		}

		/// <summary>
		/// Generates a graph image based on the data from the input file.
		/// </summary>
		public void GenerateGraphImage()
		{
			try
			{
				// Read the graph data from the input file
				string[] lines = File.ReadAllLines(inputFilePath);

				// Extract the number of vertices and edges from the first line
				string[] firstLine = lines[0].Split(' ');
				int vertexCount = int.Parse(firstLine[0]); // Total number of vertices
				int edgeCount = int.Parse(firstLine[1]);   // Total number of edges

				// Create a .dot file for Graphviz
				string dotFilePath = "graph.dot"; // Temporary file for dot representation
				using (StreamWriter writer = new StreamWriter(dotFilePath))
				{
					writer.WriteLine("graph G {"); // Start of the graph definition

					// Add edges from the input file to the dot file
					for (int i = 1; i <= edgeCount; i++)
					{
						string[] edge = lines[i].Split(' ');
						int from = int.Parse(edge[0]); // Starting vertex of the edge
						int to = int.Parse(edge[1]);   // Ending vertex of the edge
						int weight = int.Parse(edge[2]); // Weight of the edge

						// Write the edge definition to the dot file
						writer.WriteLine($"    {from} -- {to} [label={weight}];");
					}

					writer.WriteLine("}"); // End of the graph definition
				}

				// Execute the Graphviz dot command to create the image
				Process process = new Process();
				process.StartInfo.FileName = "dot"; // Graphviz command
				process.StartInfo.Arguments = $"-Tpng {dotFilePath} -o {outputFilePath}"; // Output as PNG
				process.StartInfo.RedirectStandardOutput = true; // Redirect output
				process.StartInfo.UseShellExecute = false; // Do not use shell
				process.StartInfo.CreateNoWindow = true; // Do not create a window
				process.Start(); // Start the process
				process.WaitForExit(); // Wait for the process to finish

				Console.WriteLine($"Graph creation finished: {outputFilePath}"); // Confirmation message
			}
			catch (Exception ex)
			{
				// Handle any exceptions that occur during graph generation
				Console.WriteLine($"Error when creating graph: {ex.Message}");
			}
		}
	}
}