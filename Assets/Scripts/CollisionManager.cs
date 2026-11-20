using UnityEngine;
using System.Collections.Generic;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private List<Vertex> models;
    [SerializeField] private Grid grid;

    //Aca van los textos que estan en la izquierda
    private string state = "";

    // LateUpdate para que los modelos ya hayan actualizado su bounding box
    private void LateUpdate()
    {
        state = "";

        for (int i = 0; i < models.Count; i++)
        {
            for (int j = i + 1; j < models.Count; j++)
            {
                Vertex ver1 = models[i];
                Vertex ver2 = models[j];
                string names = ver1.name + " y " + ver2.name + ": ";

                // Verifico las bounding
                if (!BoundingBoxesCollide(ver1, ver2))
                {
                    state += names + "NO colisionan (bounding box)\n";
                }
                // Sino con el punto (colision)
                else if (PointInBoth(ver1, ver2))
                {
                    state += names + "COLISIONAN\n";
                }
                else
                {
                    state += names + "NO colisionan (las bounding box se tocan pero los modelos no)\n";
                }
            }
        }
    }

    // Colision de las bounding (AABB)
    private bool BoundingBoxesCollide(Vertex ver1, Vertex ver2)
    {
        return ver1.min.x <= ver2.max.x && ver1.max.x >= ver2.min.x &&
               ver1.min.y <= ver2.max.y && ver1.max.y >= ver2.min.y &&
               ver1.min.z <= ver2.max.z && ver1.max.z >= ver2.min.z;
    }

    // Hay colisión si al menos un punto de la grilla está dentro de los dos modelos
    private bool PointInBoth(Vertex ver1, Vertex ver2)
    {
        foreach (Vector3 point in grid.points)
        {
            if (ver1.isInside(point) && ver2.isInside(point))
            {
                return true;
            }
        }
        return false;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 800, 200), state);
    }
}