using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class BarycentricWireframe : MonoBehaviour
{
	// This script only stores a reference to the original mesh
	// Mesh generation is handled by the Barycentric Mesh Baker editor tool
	public Mesh meshSource;
}
