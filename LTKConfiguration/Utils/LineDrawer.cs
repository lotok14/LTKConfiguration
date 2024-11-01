using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Rendering;

namespace LTKConfiguration.Utils
{
    public class LineDrawer : MonoBehaviour
    {
        private static Mesh lineMesh;
        private static Material lineMaterial;
        public static void LoadLineDrawer()
        {
            lineMesh = CreateLineMesh();
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        private static Mesh CreateLineMesh()
        {
            Mesh mesh = new Mesh();

            // 1x1 quad with center at origin
            mesh.vertices = new Vector3[]
            {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0)
            };

            mesh.triangles = new int[]
            {
            0, 1, 2,
            1, 3, 2
            };

            mesh.RecalculateNormals();

            return mesh;
        }

        public static void DrawRedLine(Vector3 start, Vector3 end, float progress)
        {
            // Calculate the line's midpoint, length, and angle
            Vector3 lineCenter = (start + end) / 2;
            float lineLength = Vector3.Distance(start, end);
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

            // Create transformation matrix for line
            Matrix4x4 matrix = Matrix4x4.TRS(lineCenter, Quaternion.Euler(0, 0, angle), new Vector3(lineLength, 0.15f * (1 + progress), 1));

            // Set material color
            lineMaterial.color = new Color(1, 0, 0, 0.25f + progress * 0.75f);

            // Draw the mesh instance
            Graphics.DrawMesh(lineMesh, matrix, lineMaterial, 0);
        }
    }
}
