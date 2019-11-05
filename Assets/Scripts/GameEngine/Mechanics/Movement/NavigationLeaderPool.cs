using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationLeaderPool : MonoBehaviour {

    public static NavigationLeaderPool Instance { get; private set; }

    [SerializeField]
    GameObject agentPrefab;

    [SerializeField]
    List<VirtualLeader> agents;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        agentPrefab.transform.SetParent(transform, true);
    }

    public static void AddLeader(VirtualLeader agent)
    {
        Instance.agents.Add(agent);
    }

    public static VirtualLeader GetLeader()
    {
        if(Instance.agents.Count == 0)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(Vector3.zero, out hit, 100, 1);
            var go = Instantiate(Instance.agentPrefab, hit.position, Quaternion.identity).GetComponent<VirtualLeader>();
            go.transform.SetParent(Instance.transform, true);
            go.gameObject.SetActive(true);
            return go;
        }

        VirtualLeader agent = Instance.agents[Instance.agents.Count -1];
        Instance.agents.Remove(agent);
        agent.gameObject.SetActive(true);
        return agent;
    }
	
}
