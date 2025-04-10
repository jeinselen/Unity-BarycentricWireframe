# Unity Barycentric Wireframe Shader Graph
Like many other wireframe rendering solutions in Unity, the script side of this toolset stores barycentric coordinates as tangent vectors in a modified mesh asset.

Unlike others, the shader implementation is built entirely in the Shader Graph, making customisation significantly easier without having to jump into code.

- `Editor/`
  - `BarycentricWireframeEditor.cs`
    - provides a GUI for processing game objects selected in the hierarchy, caching them in the specified folder for efficient use
    - Re-processing the same asset shouldn't break anything, and will simply use the existing cached asset data
- `Scripts/`
  - `BarycentricWireframe.cs`
    - This is automatically attached to game objects that have been processed through the GUI, and stores the source mesh for re-processing (in situations where the source mesh is changed, this allows for updates to be processed more easily)
  - `BarycentricWireframeUtility.cs`
    - Stores the barycentric calculation code for use by any other scripts, including the GUI
- `Shaders/`
  - `Wireframe-Blend.shadersubgraph`
    - Combines the functionality of both subgraphs below for creative use cases where a blend of pixel and polygon relative measurements are needed
  - `Wireframe-Pixel.shadersubgraph`
    - Pixel based wireframe width calculation, works as a pseudo-screen-space effect and renders the same line regardless of distance (though lines at glancing angles will tend to fall off)
  - `Wireframe-Polygon.shadersubgraph`
    - Polygon based wireframe width calculation, works best on meshes with semi-consistent polygon sizes, or for situations where unique effects are needed
  - `Wireframe.shadergraph`
    - Implementation example using the Wireframe-Blend sub-graph for a simple wireframe material



This has not been fully tested (especially updated mesh re-processing), but it seems to be working well in Unity 6.0. No warranty provided, use at your own risk.

Other open source options to check out:

- https://github.com/nobnak/WireframeShaderUnity
  - Implements barycentric wireframe rendering in shader code, and requires the addition of a scoped registry just to load a few lines of support code
- https://github.com/Milun/unity-solidwire-shader
  - Implements wireframe rendering using data from a custom Blender exporter, and requires meshes to be processed through that system instead of within Unity
