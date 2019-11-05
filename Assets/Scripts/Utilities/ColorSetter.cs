using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour {

    [SerializeField]
    Renderer[] renderers;

    [SerializeField]
    Entity entity;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null)
                continue;
            if(!renderers[i].material.HasProperty("_OccludedColor"))
                renderers[i].material.color = entity.Team.Color;
            else
                renderers[i].material.SetColor("_OccludedColor", entity.Team.Color);
            
        }
	}
	

}
