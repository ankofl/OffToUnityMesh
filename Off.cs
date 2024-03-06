#if Unity
using UnityEngine;
#else
using System.Numerics;
#endif

using System.Globalization;
using System.Collections.Generic;
using System;
using System.IO;

namespace OffIO
{
	public class Off
	{
		public List<Vector3> Vertices;
		public List<int> Indices;

		public Off Load(string path)
		{
			if (!File.Exists(path))
			{
				throw new Exception("not found" + path);
			}

			Vertices = new List<Vector3>();

			Indices = new List<int>();

			string[] lines = File.ReadAllLines(path);

			for (int i = 3; i < lines.Length; i++)
			{
				string line = lines[i];

				string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length == 3) // Vertex coordinates
				{
					float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
					float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
					float z = float.Parse(parts[2], CultureInfo.InvariantCulture);

					Vertices.Add(new Vector3(x, y, z));
				}
				else if (parts.Length > 3 && parts[0] == "3") // Triangle face
				{
					int v1 = int.Parse(parts[1], CultureInfo.InvariantCulture);
					int v2 = int.Parse(parts[2], CultureInfo.InvariantCulture);
					int v3 = int.Parse(parts[3], CultureInfo.InvariantCulture);

					Indices.Add(v1);
					Indices.Add(v2);
					Indices.Add(v3);
				}
			}

			return this;
		}

		#if Unity
		public Mesh ToMesh()
		{
			Mesh mesh = new Mesh()
			{
				vertices = Vertices.ToArray(),
				triangles = Indices.ToArray(),
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			return mesh;
		}
		#endif
	}
}
