using System.Collections.Generic;
using UnityEngine;

public class MeshScript : MonoBehaviour {

    [SerializeField] private Material mat;
    [SerializeField] private int density;
    [SerializeField] private float max;
    private float initialHeight = 0f;
    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;

    private void Start ()
    {
        GameObject obj = new GameObject("Plane Mesh");
        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = mat;
        mesh = obj.GetComponent<MeshFilter>().mesh;

        GeneratePlane();
        AssignMesh();
    }

    List<Vector3> setVertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        float interval =  max / density;
        float tempY = 0;
        for (int i = 0; i <= density; i++)
        {
            float tempX = 0;
            for (int j = 0; j <= density; j++)
            {
                vertices.Add(new Vector3(tempX, initialHeight, tempY));
                tempX += interval;
            }
            tempY += interval;
        }
        return vertices;
    }
    List<int> setTriangles() { 
    
        List<int> triangles = new List<int>();

        for (int i = 0;i < density;i++)
        {
            for (int j = 0; j < density; j++)
            {
                triangles.Add(j + (i * (density + 1)));
                triangles.Add((j + 1) + (i * (density + 1)));
                triangles.Add((j + ((i + 1) * (density + 1))));

                triangles.Add((j + ((i + 1) * (density + 1))));
                triangles.Add((j + 1) + (i * (density + 1)));
                triangles.Add(((j + 1) + ((i + 1) * (density + 1))));

                triangles.Add((j + ((i + 1) * (density + 1))));
                triangles.Add((j + 1) + (i * (density + 1)));
                triangles.Add(j + (i * (density + 1)));

                triangles.Add(((j + 1) + ((i + 1) * (density + 1))));
                triangles.Add((j + 1) + (i * (density + 1)));
                triangles.Add((j + ((i + 1) * (density + 1))));
            }
        }
        return triangles;
    }

    void GeneratePlane()
    {
        vertices = setVertices().ToArray();
        triangles = setTriangles().ToArray();
    }

    void AssignMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
