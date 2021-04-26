using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Replay
{
    public class ReplayManager: MonoBehaviour
    {
        public static List<KeyCode> inputs = new List<KeyCode>();
        public static List<float> timeBetweenInputs = new List<float>();
    }
}