using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFCWorldGenerator
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject prototypesCollections;

        void OnDrawGizmos()
        {
            for (int i = 0; i < prototypesCollections.transform.childCount; i++)
            {
                GameObject prototype = prototypesCollections.transform.GetChild(i).gameObject;
                Vector3[][] bounds = PrototypeParser.GetHexBoundingVertices(prototype);
                BoundsVisualizer.DrawGizmoHexBounds(prototype, bounds, Color.red);
            }
        }
    }
}
