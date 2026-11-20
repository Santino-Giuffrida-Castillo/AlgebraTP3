using UnityEngine;

public class Vertex : MonoBehaviour
{
    private Vector3[] localVertex;
    private int[] index;
    private Vector3[] worldVertexes;

    // Bounding box
    //vertice menor
    public Vector3 min;
    //vertice mayor
    public Vector3 max;

    // Centro del modelo
    private Vector3 center;

    // Un plano por cada cara: un punto del plano y su normal
    private Vector3[] planePoints;
    private Vector3[] normals;

    private int planeCount;

    private void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        localVertex = mesh.vertices;
        index = mesh.triangles;
        worldVertexes = new Vector3[localVertex.Length];

        planePoints = new Vector3[index.Length / 3];
        normals = new Vector3[index.Length / 3];
    }

    private void Update()
    {
        // Vértices en el mundo, bounding box y centro
        Vector3 sum = Vector3.zero;
        min = transform.TransformPoint(localVertex[0]);
        max = min;

        for (int i = 0; i < localVertex.Length; i++)
        {
            Vector3 worldVertex = transform.TransformPoint(localVertex[i]);
            worldVertexes[i] = worldVertex;
            sum += worldVertex;
            //Calculo bounding box (vertice menor)

            if (worldVertex.x < min.x) 
            {
                min.x = worldVertex.x;
            }
            if (worldVertex.y < min.y) 
            {
                min.y = worldVertex.y;
            }
            if (worldVertex.z < min.z)
            {
                min.z = worldVertex.z;
            } 
            //Y ahora con el vertice mayor
            if (worldVertex.x > max.x)
            {
                max.x = worldVertex.x;
            }
            if (worldVertex.y > max.y)
            {
                max.y = worldVertex.y;
            } 
            if (worldVertex.z > max.z)
            {
                max.z = worldVertex.z;
            } 
        }
        //Caclulo el centro (la suma de todos los vertices dividido la cantidad totatl de vertices)
        center = sum / localVertex.Length;

        // Planos de cada cara
        planeCount = 0;
        for (int i = 0; i < index.Length; i += 3)
        {
            Vector3 a = worldVertexes[index[i]];
            Vector3 b = worldVertexes[index[i + 1]];
            Vector3 c = worldVertexes[index[i + 2]];

            //Aca saco la normal(Producto punto entre el vector que va desde (A hasta B) y (A hasta C) )
            Vector3 normal = Normalize(CrossProduct(b - a, c - a));

            //Caso que toque un triangulo degenerado lo tenemos que saltear porque no forma un plano        
            if (normal == Vector3.zero)
            {
                continue;
            }
            // La normal tiene que mirar hacia el centro del modelo, por eso hago producto punto, y uso el centro
            if (Dot(normal, center - a) < 0)
            {
                normal = new Vector3(-normal.x, -normal.y, -normal.z);
            }
            
            bool repeated = false;

            for (int j = 0; j < planeCount; j++)
            {
                //Comparo el triangulo actual(normal) con uno ya guardado (normals[j])
                bool isInSameDireccion = Dot(normals[j], normal) > 0.999f;
                //Pregunto si estan a la misma altura, haciendo producto punto entre la normal del truangulo actual y un vector (que arranca desde el punto del plano que estoy comparando ahora con j hasta a)
                float distanceToPlane = Dot(normal, a - planePoints[j]);

                //Si es negativo lo paso a positivo porque solo me interesa saber que tan lejos esta no el lado
                if (distanceToPlane < 0)
                {
                    distanceToPlane = -distanceToPlane;
                }

                //Si apuntan a la misma direccion y si estan a la misma altura es porque la normal es repetida entonces no me guardo nada
                if (isInSameDireccion && distanceToPlane < 0.001f)
                {
                    repeated = true;
                    break;
                }
            }
            if (!repeated)
            {
                planePoints[planeCount] = a;
                normals[planeCount] = normal;
                planeCount++;
            }

        }
    }
         // El punto está dentro si está del lado de la normal en TODOS los planos
    public bool isInside(Vector3 point)
    {
        // Chequeo con las bounding box
        if (point.x < min.x || point.x > max.x ||
            point.y < min.y || point.y > max.y ||
            point.z < min.z || point.z > max.z)
        {
            return false;
        }
        // Chequeo si un punto esta adentro del modelo
        for (int i = 0; i < planeCount; i++)
        {
            if (Dot(normals[i], point - planePoints[i]) < 0)
            {
                return false;
            }
        }
        return true;
    }
    //Producto punto (me devuelve un escalar)
    private float Dot(Vector3 p1, Vector3 p2)
    {
        return p1.x * p2.x + p1.y * p2.y + p1.z * p2.z;
    }
    //Producto cruz (Me retorna un vector que esta a 90 grados de los 2 vectores (NO ES CONMUTATIVO))
    private Vector3 CrossProduct(Vector3 p1, Vector3 p2)
    {
        return new Vector3(p1.y * p2.z - p1.z * p2.y, p1.z * p2.x - p1.x * p2.z, p1.x * p2.y - p1.y * p2.x);
    }
    //Te devuelve un vector con la misma direccion y magnitud 1
    private Vector3 Normalize(Vector3 p)
    {
        float distance = Mathf.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z);
        if (distance == 0) 
        {
            return Vector3.zero;
        } 
        // triángulo degenerado

        return new Vector3(p.x / distance, p.y / distance, p.z / distance);
    }
    //Dibujado de cada cara (el contorno y de color azul)
    private void OnDrawGizmos()
    {
        if (normals == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((min + max) / 2f, max - min);

        Gizmos.color = Color.blue;
        for (int i = 0; i < planeCount; i++)
        {
            Gizmos.DrawRay(center, normals[i] * 0.5f);
        }
    }

}
