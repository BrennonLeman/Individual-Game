using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrennonsFunctions
{
    public static void LineRendererBezier(LineRenderer line, Vector3 p0, Vector3 p1, Vector3 p2, int seg)
    {
        line.positionCount = seg;
        for (int i = 0; i < seg; i++)
        {
            float t = i * (1f / (seg - 1));
            Vector3 actualPoint = QuadBezier(p0, p1, p2, t);
            line.SetPosition(i, actualPoint);
        }
    }
    private static Vector3 QuadBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }
    public static void ResetTransform(Transform _t)
    {
        if (_t != null)
        {
            _t.localPosition = Vector3.zero;
            _t.localScale = Vector3.one;
            _t.localRotation = Quaternion.identity;
        }
    }
    public static float scale(float oldMin, float oldMax, float newMin, float newMax, float oldValue){
 
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
 
        return(newValue);
    }
}
