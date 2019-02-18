using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMesh : MonoBehaviour
{
    //FOV script
    FOV fov;

    //Desenho
    Mesh mesh;

    //acerto
    RaycastHit2D hit;

    //Resolucao
    [SerializeField] float meshRes = 2;

    //vertices
    [HideInInspector] public Vector3[] vertices;

    //triangulacao de pontos
    [HideInInspector] public int[] triangles;

    //contador de passos
    [HideInInspector] public int stepCount;

    public float E = 1, F = 1, G = 1;
    private bool rotational = false;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        fov = GetComponentInParent<FOV>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!rotational)
        {
            if ((E == 1 && F == -1 & G == -1) || (E == 3 && F == -3.66f & G == -1))
            {
                GetComponent<Transform>().localRotation = Quaternion.Euler(-180, -1, 0);
            }
            else
            {
                GetComponent<Transform>().localRotation = Quaternion.Euler(1, 1, 0);
            }
        }
        else
        {
            if (E >= 9.38f)
            {
                E = 1.38f;
                GetComponent<Transform>().localRotation = Quaternion.Euler(1, 1, 0);
            }
            else if((E >= 1f && E < 1.38f) || (E > 2.61f && E < 4) || (E > 5.37f && E < 6.62f) || (E > 7.99f && E < 9.38f))
            {
                GetComponent<Transform>().localRotation = Quaternion.Euler(-1, -1, 0);
            }
            else
            {
                GetComponent<Transform>().localRotation = Quaternion.Euler(1, 1, 0);
            }
        }

        MakeMesh();
    }

    public void setRotational(bool toggle)
    {
        rotational = toggle;
    }
    private void MakeMesh()
    {
        stepCount = Mathf.RoundToInt(fov.viewAngle * meshRes);
        float stepAngle = fov.viewAngle / stepCount;

        List<Vector3> viewVertex = new List<Vector3>();

        hit = new RaycastHit2D();

        for (int i = 0; i < stepCount; i++)
        {
            //E,F = rotaciono o FOV: + sentido horario, - sentido anti-horario
            //B,G,C,H = aumento o raio do FOV: + sentido ah, - sentido h
            float angle = fov.transform.eulerAngles.y - fov.viewAngle * E / 2 * F + stepAngle * G * i;
            Vector3 dir = fov.DirFromAngle(angle, false);

            //armazena o obstaculo
            hit = Physics2D.Raycast(fov.transform.position, dir, fov.viewRadius, fov.obstacleMask);

            //se o collider do obstaculo for nulo
            if (hit.collider == null)
            {
                viewVertex.Add(transform.position + dir.normalized * fov.viewRadius);
            }
            else
            {
                viewVertex.Add(transform.position + dir.normalized * hit.distance);
            }
        }

        int vertexCount = viewVertex.Count + 1;

        vertices = new Vector3[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3 + 2] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3] = i + 2;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
