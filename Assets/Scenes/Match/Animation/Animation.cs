using System.Collections;
using UnityEngine;

    public abstract class Animation
    {
        public float duration;
        public float progressTime;

        public abstract void animate();
    }
