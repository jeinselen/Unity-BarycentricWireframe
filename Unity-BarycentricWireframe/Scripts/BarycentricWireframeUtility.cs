using UnityEngine;
using System.IO;

public static class BarycentricWireframeUtility
{
	public static Mesh CreateBarycentricMesh(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		Vector2[] uv = mesh.uv;
		
		int vertexCount = triangles.Length;
		Vector3[] newVertices = new Vector3[vertexCount];
		Vector3[] newNormals = new Vector3[vertexCount];
		Vector2[] newUVs = new Vector2[vertexCount];
		Vector4[] newTangents = new Vector4[vertexCount];
		int[] newTriangles = new int[vertexCount];
		
		Vector4[] barycentrics = new Vector4[]
		{
			new Vector4(1, 0, 0, 0),
			new Vector4(0, 1, 0, 0),
			new Vector4(0, 0, 1, 0)
		};
		
		for (int i = 0; i < vertexCount; i++)
		{
			int triIndex = triangles[i];
			newVertices[i] = vertices[triIndex];
			newNormals[i] = normals.Length > triIndex ? normals[triIndex] : Vector3.up;
			newUVs[i] = uv.Length > triIndex ? uv[triIndex] : Vector2.zero;
			newTangents[i] = barycentrics[i % 3];
			newTriangles[i] = i;
		}
		
		Mesh newMesh = new Mesh();
		newMesh.name = mesh.name + "_barycentric";
		newMesh.vertices = newVertices;
		newMesh.normals = newNormals;
		newMesh.uv = newUVs;
		newMesh.tangents = newTangents;
		newMesh.triangles = newTriangles;
		
		return newMesh;
	}
}
