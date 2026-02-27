using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFCWorldGenerator
{
    public class PrototypeParser
    {
        /// <summary>
        /// Returns the bounding vertices of a hex mesh grouped by side.
        /// </summary>
        /// <param name="hexObject">The hex GameObject with a MeshFilter.</param>
        /// <param name="hexRadius">The radius of the hexagon in local space.</param>
        /// <returns>6 arrays of Vector3, one per hex side, in local space.</returns>
        public static Vector3[][] GetHexBoundingVertices(GameObject hexObject, float hexRadius = 1f)
        {
            Mesh mesh = hexObject.GetComponent<MeshFilter>().sharedMesh;

            if(mesh == null)
            {
                Debug.LogError("PrototypeParser: GameObject " + hexObject.name + " does not have a MeshFilter with a mesh.");
                return new Vector3[0][];
            }

            Vector3[] verts = mesh.vertices;
            Vector3 center = Vector3.zero;

            HashSet<Vector3> hexCornerVerticesSet = new HashSet<Vector3>();
            List<Vector3> innerVertices = new List<Vector3>();

            // Get vertices that are outside the hexagon defined by the center and hexRadius
            foreach (Vector3 v in verts)
            {
                // Distance from center to vertex in the xz plane (supposed to be v - center but center is Vector3.zero since local space)
                float xzDist = new Vector2(v.x, v.z).magnitude;
                if (Mathf.Abs(xzDist - hexRadius) <= 0.01f)
                    hexCornerVerticesSet.Add(v);
                else
                    innerVertices.Add(v);
            }

            List<Vector3> hexCornerVertices = new List<Vector3>(hexCornerVerticesSet);
            hexCornerVertices.Sort((a, b) =>
                Mathf.Atan2(a.z, a.x).CompareTo(Mathf.Atan2(b.z, b.x))
            );

            List<Vector3[]> boundingVertices = new List<Vector3[]>();
            for (int i = 0; i < hexCornerVertices.Count; i++)
            {
                Vector3 start = hexCornerVertices[i];
                Vector3 end = hexCornerVertices[(i + 1) % hexCornerVertices.Count];
                List<Vector3> boundingVerticesList = new List<Vector3> { start };

                foreach (Vector3 r in innerVertices)
                {
                    // r cross the vector pv if (px-rx)*vz - (pz-rz)*vx == 0
                    // simplifed by ignoring the y axis
                    // https://stackoverflow.com/questions/48488101/check-if-a-point-lies-on-a-line-vector
                    Vector3 edge = end - start;
                    float cross = (r.x - start.x) * edge.z - (r.z - start.z) * edge.x;
                    if (Mathf.Abs(cross) < 0.01f)
                        boundingVerticesList.Add(r);
                }

                boundingVerticesList.Add(end);
                boundingVertices.Add(boundingVerticesList.ToArray());
            }

            return boundingVertices.ToArray();
        }

        public static Material GetEdgeMaterial(GameObject prototype)
        {
            return prototype.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }
}
