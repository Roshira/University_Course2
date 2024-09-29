using MassTransit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanningTree_NetWork
{
	/// <summary>
	/// Class for deleting an edge from the graph stored in a file.
	/// </summary>
	public class DeletePartGraph
	{
		private string filePath; // Path to the graph data file

		/// <summary>
		/// Constructor to initialize the file path.
		/// </summary>
		/// <param name="filePath">Path to the graph data file.</param>
		public DeletePartGraph(string filePath)
		{
			this.filePath = filePath;
		}

		/// <summary>
		/// Public method to start the deletion process.
		/// </summary>
		public void startDelete()
		{
			int[] date = new int[3]; // Array to hold input data for the edge
			GetElementGraph getElementGraph = new GetElementGraph(); // Instance to get user input
			date = getElementGraph.WritingVertexOREdge();
			Delete(date); // Calls the private delete method
		}

		/// <summary>
		/// Private method that handles the logic for deleting an edge from the graph.
		/// </summary>
		private void Delete(int[] date)
		{
			int u, v, w; // Variables for the edge to be deleted

			// Get user input for edge details
			if (date == null) // Validate input
			{
				Console.WriteLine("You dont writing correct date"); // Error for invalid input
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

			string[] lines = File.ReadAllLines(filePath); // Read file lines

			// Parse the first line for vertex and edge counts
			string[] firstLine = lines[0].Split();
			int vertices = int.Parse(firstLine[0]);
			int edges = int.Parse(firstLine[1]);

			bool exit = true; // Flag for edge existence

			// Check if the edge exists in the graph
			for (int i = 1; i < lines.Length; i++)
			{
				string[] edgeParts = lines[i].Split();
				int existingU = int.Parse(edgeParts[0]);
				int existingV = int.Parse(edgeParts[1]);

				// Set exit flag if the edge is found
				if ((existingU == u && existingV == v) || (existingU == v && existingV == u))
				{
					exit = false;
					break;
				}
			}
			if (exit) // Edge not found
			{
				Console.WriteLine("We haven't this edge...");
				Console.ReadKey();
				return;
			}

			// Count remaining connections for vertices u and v
			int uDelete = 0;
			int vDelete = 0;
			for (int i = 1; i < lines.Length; i++)
			{
				string[] InfoParts = lines[i].Split();
				int existingU = int.Parse(InfoParts[0]);
				int existingV = int.Parse(InfoParts[1]);

				// Increment counts for vertices connected to u and v
				if (existingU == u || existingV == u)
					uDelete++;
				if (existingV == v || existingU == v)
					vDelete++;
			}

			// Decrement vertex count if only one edge remains for each vertex
			if (uDelete == 1)
				vertices--;
			if (vDelete == 1)
				vertices--;

			// Open file for writing (overwrite data)
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				edges--; // Decrement edge count
				writer.WriteLine($"{vertices} {edges}"); // Write updated counts

				// Rewrite existing edges, excluding the deleted one
				for (int i = 1; i < lines.Length; i++)
				{
					if ((lines[i] != $"{u} {v} {w}") && (lines[i] != $"{v} {u} {w}"))
						writer.WriteLine(lines[i]);
				}
			}
		}
	}
}
