# TrackGenerator

An important part of making a game where there is trains or roads or any kinds of curvy shapes is to be able to define those shapes effectively. We don't want to create a different mesh for every single type of curve of a railroad track. One way to create these "curvy meshes" is to use extrusion along a predefined curve. This is what this C# project aims to accomplish. Right now it assumes it is used with unity.

Right now you can model an **Wavefront mesh** in plane, save it in folder **Track/res as TestMesh.obj** and it will be extruded along a test path. The test path is defined in **TrackTester** class, and it consist only of points in **xz -plane** and the rotations of first and last set of mesh duplicants. The next step is to generate this path from parameters of a curve.

##Installing##

When installing this project you should clone or download the files in your Unity Assets -folder, so that you have following structure: *Assets/TrackGenerator/Track..*.

You can insert your curvy object into unity by adding the **TrackTester.cs** to some 3D object in your Unity scene, and the final shape will appear when the game is started. The mesh the script was added to wont show now.

##Format of the mesh##

The mesh should be made so that all vertices are in **yz-plane**. Only edge and vertex data is used so the file you export from a 3D software should include those. The only supported file type is **Wavefront .obj**. If you use Blender, first make sure the mesh consist of only vertices and edges in yz-plane. Then export as OBJ and make sure to set exporting option **Forward** as **Y Forward** and **UP** as **Z Up** to get correct orientation.
