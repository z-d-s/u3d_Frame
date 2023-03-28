using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoomTest : MonoBehaviour
{
    //Scale a mesh on a second thread
    void ScaleMesh(Mesh mesh, float scale)
    {
        //Get the vertices of a mesh
        var vertices = mesh.vertices;

        //Run the action on a new thread
        Loom.RunAsync(() =>
        {
            //Loop through the vertices
            for (var i = 0; i < vertices.Length; i++)
            {
                //Scale the vertex
                vertices[i] = vertices[i] * scale;
            }

            //Run some code on the main thread
            //to update the mesh
            Loom.QueueOnMainThread((a) =>
            {
                //Set the vertices
                mesh.vertices = vertices;
                //Recalculate the bounds
                mesh.RecalculateBounds();
            });
        });
    }
}