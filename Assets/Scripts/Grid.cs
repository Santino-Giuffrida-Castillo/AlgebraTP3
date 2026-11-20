using System;
using UnityEngine;
using System.Collections.Generic;
public class Grid : MonoBehaviour

{
        [SerializeField] private Vector3Int size = new Vector3Int(40, 40, 40);
        [SerializeField] private float spacing = 5f;
        [SerializeField] private float pointSize = 1f;


    public List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        GeneratePoints();
    }

    private void GeneratePoints()
    {
        points.Clear();

    Vector3 half = new Vector3(size.x, size.y, size.z) / 2f;

    for (int x = 0; x < size.x; x++)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                Vector3 position = transform.position + (new Vector3(x, y, z) - half) * spacing;

                points.Add(position);
            }
        }
    }
    }
    
    private void OnDrawGizmos()
        {
            
            if (points == null)
                return;
            
            Gizmos.color = Color.white;

            foreach (Vector3 point in points)
           {
                Gizmos.DrawSphere(point, pointSize);
           }
 
        }
}

