using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	/// <summary>
	/// Class for modifying graph data stored in a file.
	/// </summary>
	internal class GraphModifier
	{
		private string filePath; // Path to the file containing the graph data

		/// <summary>
		/// Constructor to initialize the file path.
		/// </summary>
		/// <param name="filePath">Path to the graph data file.</param>
		public GraphModifier(string filePath)
		{
			this.filePath = filePath;
		}

		/// <summary>
		/// Public method to initiate the addition of a new edge.
		/// </summary>
		internal void AddStart()
		{
			Add(); // Calls the private method to add an edge
		}

		/// <summary>
		/// Private method that handles the logic for adding a new edge to the graph.
		/// </summary>
		private void Add()
		{
			int u, v, w; // Vertices and weight of the new edge
			int[] date = new int[3]; // Array to hold the input data for the edge
			GetElementGraph getElementGraph = new GetElementGraph(); // Instance to get user input

			// Get user input for edge details
			date = getElementGraph.WritingVertexOREdge();
			if (date == null) // Validate input
			{
				Console.WriteLine("You dont writing correct date"); // Error message for invalid input
				Console.ReadKey();
				return;
			}
			else
			{
				u = date[0]; // First vertex
				v = date[1]; // Second vertex
				w = date[2]; // Weight of the edge
			}

			// Check for self-loops or zero-weight edges
			if (u == v || w == 0)
				return;

			// Read all lines from the graph file
			string[] lines = File.ReadAllLines(filePath);

			// Parse the first line for current vertex and edge counts
			string[] firstLine = lines[0].Split();
			int vertices = int.Parse(firstLine[0]);
			int edges = int.Parse(firstLine[1]);

			// Check for existing edges to avoid duplicates
			for (int i = 1; i < lines.Length; i++)
			{
				string[] edgeParts = lines[i].Split();
				int existingU = int.Parse(edgeParts[0]);
				int existingV = int.Parse(edgeParts[1]);
				int existingW = int.Parse(edgeParts[2]);

				// Return if the edge already exists in either direction
				if (existingU == u && existingV == v)
					return;
				else if (existingU == v && existingV == u)
					return;
			}

			edges++; // Increment edge count

			// Update the vertex count if a new vertex is added
			int maxVertex = Math.Max(u, v) + 1;
			if (maxVertex > vertices)
			{
				vertices = maxVertex;
			}

			string newEdge = $"{u} {v} {w}"; // Create a new edge string

			// Open the file for writing (overwrite all data)
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				// Write updated vertex and edge counts
				writer.WriteLine($"{vertices} {edges}");

				// Rewrite existing edges
				for (int i = 1; i < lines.Length; i++)
				{
					writer.WriteLine(lines[i]);
				}

				// Add the new edge to the file
				writer.WriteLine(newEdge);
			}
		}
	}
}
