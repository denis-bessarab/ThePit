using UnityEngine;
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainMesh2D : MonoBehaviour
{
    [SerializeField] private float depth = 5f;

#if UNITY_EDITOR
    private void Start()
    {
        GenerateMesh();
    }

    private void OnValidate()
    {
        GenerateMesh();
    }
#endif

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector2[] surface =
        {
            new Vector2(0, 0),
            new Vector2(.1f, 0.1f),
            new Vector2(.2f, 0.1f),
            new Vector2(.3f, 0.15f),
            new Vector2(.4f, 0.2f),
            new Vector2(.5f, 0.25f),
            //new Vector2(4, 0.5f),
            //new Vector2(6, 2),
            //new Vector2(8, 1)
        };

        int count = surface.Length;

        Vector3[] vertices = new Vector3[count * 2];

        for (int i = 0; i < count; i++)
        {
            vertices[i] = surface[i];
        }

        for (int i = 0; i < count; i++)
        {
            vertices[i + count] =
                new Vector3(surface[i].x, surface[i].y - depth, 0);
        }

        int[] triangles = new int[(count - 1) * 6];

        int t = 0;

        for (int i = 0; i < count - 1; i++)
        {
            int topLeft = i;
            int topRight = i + 1;
            int bottomLeft = i + count;
            int bottomRight = i + count + 1;

            triangles[t++] = topLeft;
            triangles[t++] = topRight;
            triangles[t++] = bottomLeft;

            triangles[t++] = topRight;
            triangles[t++] = bottomRight;
            triangles[t++] = bottomLeft;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}