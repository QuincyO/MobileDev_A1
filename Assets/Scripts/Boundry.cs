using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quincy.Structs
{
    [System.Serializable]
    public struct Boundary
    {
        public Axis x;
        public Axis y;
    }

    [System.Serializable]
    public struct Axis
    {
       public float min;
       public float max;
    }
}

