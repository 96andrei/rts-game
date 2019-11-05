using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererClear : MonoBehaviour {

    [SerializeField]
    TrailRenderer tRenderer;

    private void OnEnable()
    {
        tRenderer.Clear();
    }
}
