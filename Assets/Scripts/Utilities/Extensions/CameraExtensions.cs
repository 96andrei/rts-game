using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils
{

    private static Camera main;

    public static Camera Main {
        get {
            if (main == null)
            {
                main = Camera.main;
            }

            return main;
        }
    }

}
