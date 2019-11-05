using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMetadata : MonoBehaviour {

    [SerializeField]
    EntityType type;

    [SerializeField]
    EntityUiData uiData;

    [SerializeField]
    SpawnableConfig spawnableConfig;

    HashSet<string> flags = new HashSet<string>();

    public EntityType Type { get { return type; } }
	public EntityUiData UiData { get { return uiData; } }
    public SpawnableConfig SpawnableConfig { get { return spawnableConfig; } }

    public void WriteFlag(string flag)
    {
        flags.Add(flag);
    }

    public void RemoveFlag(string flag)
    {
        flags.Remove(flag);
    }

    public void ClearFlags()
    {
        flags.Clear();
    }

    public bool HasFlag(string flag)
    {
        return flags.Contains(flag);
    }

}
