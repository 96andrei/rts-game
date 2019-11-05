using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiControllerHelper : MonoBehaviour {

    [SerializeField]
    GameObject selectBoxObject;
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    GameObject moveMarker;
    [SerializeField]
    GameObject attackMarker;
    [SerializeField]
    RectTransform selectableArea;

    public GameObject SelectBoxObject { get { return selectBoxObject; } }
    public GameObject Canvas { get { return canvas; } }
    public RectTransform RectTransform { get { return rectTransform; } }
    public GameObject MoveMarker { get { return moveMarker; } }
    public GameObject AttackMarker { get { return attackMarker; } }
    public RectTransform SelectableArea { get { return selectableArea; } }


}
