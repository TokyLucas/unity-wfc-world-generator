using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFCWorldGenerator
{
    public class BoundsVisualizer
    {
        public static void DrawGizmoHexBounds(GameObject prototype, Vector3[][] bounds, Color color = default)
        {
            Gizmos.color    = color;
            Transform t     = prototype.transform;

            for (int i = 0; i < bounds.Length; i++)
            {
                for (int j = 0; j < bounds[i].Length; j++)
                {
                    Vector3 pos = t.TransformPoint(bounds[i][j]);
                    Gizmos.DrawSphere(pos, 0.025f);

                    // Draw line to next vert on same side
                    if (j < bounds[i].Length - 1)
                    {
                        Vector3 next = t.TransformPoint(bounds[i][j + 1]);
                        Gizmos.DrawLine(pos, next);
                    }
                }
            }
        }
    }
}