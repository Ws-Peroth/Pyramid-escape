using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    public static float Abs(float x) => x < 0 ? -x : x;
    public static float Avg(int a, int b) => (a + b) / 2f;
}
