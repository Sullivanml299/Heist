using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    Mesh mesh;
    MeshFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[vertices.Length]; //must always be the same size as vertices
        int[] triangles = new int[3]; // should be 3 * the number of triangles you want to make. 3 vertices for each triangle

        vertices[0] = new Vector3(0,0);
        vertices[1] = new Vector3(0,100);
        vertices[2] = new Vector3(100, 100);

        //These triangles control the face (front or back) that is showing. To show the front face, always order the triangles in clockwise order. 
        // this will matter for shaders that only shade some faces.
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
