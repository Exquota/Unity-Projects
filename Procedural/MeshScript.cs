using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshScript : MonoBehaviour
{

    [SerializeField] private Material mat;
    [SerializeField] private int density;
    [SerializeField] private float max;
    public float[] amplitudes;
    public float[] wavelengths;

    private float initialHeight = 0f;
    private Vector3[] vertices;
    private int[] triangles;
    private Mesh mesh;

    private void Start()
    {
        ProcessConstants();

        GameObject obj = new GameObject("Plane Mesh");
        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = mat;
        mesh = obj.GetComponent<MeshFilter>().mesh;

        GeneratePlane();
        AssignMesh();
    }

    private void Update()
    {
        WaterFunction(Time.timeSinceLevelLoad);
        AssignMesh();
    }

    void ProcessConstants()
    {

        //Pads arrays
        if (amplitudes.Length > wavelengths.Length)
        {
            for (int i = 0;i < (amplitudes.Length - wavelengths.Length);i++)
            {
                wavelengths.Append<float>(1f);
            }
        } else if (wavelengths.Length > amplitudes.Length)
        {
            for (int i = 0; i < (wavelengths.Length - amplitudes.Length); i++)
            {
                amplitudes.Append<float>(1f);
            }
        }

        //Check for 0s in array
        for (int i = 0;i < amplitudes.Length; i++)
        {
            if (amplitudes[i] == 0f)
            {
                amplitudes[i] = 1f;
            } 
            if (wavelengths[i] == 0f)
            {
                wavelengths[i] = 1f;
            }
        }
    }

    List<Vector3> setVertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        float interval = max / density;
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
    List<int> setTriangles()
    {

        List<int> triangles = new List<int>();

        for (int i = 0; i < density; i++)
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

    void WaterFunction(float time)
    {
        for (int i = 0;i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float y = 0f;
            for (int j = 0;j < amplitudes.Length;j++)
            {
                y += amplitudes[j] * Mathf.Sin((vertex.x * wavelengths[j]) + (time * (2 / wavelengths[j])));
            }
            vertex.y = y;
            vertices[i] = vertex;
        }
    }
}
