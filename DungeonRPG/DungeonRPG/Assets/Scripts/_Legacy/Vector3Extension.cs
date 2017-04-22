using UnityEngine;
using System.Collections;

public static class Vector3Extension
{
    private static Vector3 one = new Vector3(1,1,1);

    public static Vector3 One(this Vector3 vector3) { return one; }
}
