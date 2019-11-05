using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceMap : MonoBehaviour
{

    [SerializeField]
    private int width;
    public int Width { get { return width; } }
    [SerializeField]
    private int height;
    public int Height { get { return height; } }
    [SerializeField]
    private float spacing;
    public float Spacing { get { return spacing; } }
    [SerializeField]
    private Vector3 mapCenter;
    [SerializeField]
    private float updateInterval;
    [SerializeField]
    private int maxStepsPerFrame = 750;
    [SerializeField]
    InfluenceScanner scanner;

    public bool UpdatedThisFrame { get; private set; }

    private HashSet<InfluenceObject> influencers = new HashSet<InfluenceObject>();
    private List<InfluenceObject> influencerList = new List<InfluenceObject>();

    public float UpdateInterval { get { return updateInterval; } }

    [Header("Debug")]
    [SerializeField]
    bool drawMap = false;
    [SerializeField]
    private InfluenceType drawType;
    public List<InfluenceObject> dummyInfluencers;
    public float drawSize = 1;

    InfluenceTile[,] grid;

    float offsetX;
    float offsetZ;
    float currentUpdate;

    private int updatesInProgress = 0;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        UpdatedThisFrame = false;

        currentUpdate -= Time.deltaTime;
        if (currentUpdate > 0)
            return;

        currentUpdate = updateInterval;

        UpdateMap();
        UpdatedThisFrame = true;
    }


    public InfluenceTile GetTile(int x, int y)
    {
        return grid[y,x];
    }

    public void GenerateMap()
    {
        grid = new InfluenceTile[height, width];

        //offsetX = 
        //offsetZ =

        //generate grid
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                var t = new InfluenceTile();
                grid[i, j] = t;

                t.X = j;
                t.Y = i;

                t.WorldPosition = GridToWorldPosition(j, i);
            }

        //make neighbour connections
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                PrepareNeighbours(i, j);

    }

    void PrepareNeighbours(int i, int j)
    {
        var t = grid[i, j];
        if (IsInGrid(i - 1, j - 1))
            t.AddNeighbour(grid[i - 1, j - 1]);
        if (IsInGrid(i - 1, j))
            t.AddNeighbour(grid[i - 1, j]);
        if (IsInGrid(i - 1, j + 1))
            t.AddNeighbour(grid[i - 1, j + 1]);

        if (IsInGrid(i, j - 1))
            t.AddNeighbour(grid[i, j - 1]);
        if (IsInGrid(i, j))
            t.AddNeighbour(grid[i, j]);
        if (IsInGrid(i, j + 1))
            t.AddNeighbour(grid[i, j + 1]);

        if (IsInGrid(i + 1, j - 1))
            t.AddNeighbour(grid[i + 1, j - 1]);
        if (IsInGrid(i + 1, j))
            t.AddNeighbour(grid[i + 1, j]);
        if (IsInGrid(i + 1, j + 1))
            t.AddNeighbour(grid[i + 1, j + 1]);
    }

    public bool IsInGrid(int y, int x)
    {
        return y >= 0 && x >= 0 && y < height && x < width;
    }

    public void UpdateMap()
    {
        StartCoroutine(UpdateRoutine());
    }

    private IEnumerator UpdateRoutine()
    {

        updatesInProgress++;
        if (updatesInProgress > 2)
        {
            updatesInProgress--;
            yield break;
        }

        int steps = 0;

        List<InfluenceObject> influencers = RequestInfluencers();

        //reset affected tiles
        for (int k = 0; k < influencers.Count; k++)
        {
            if (influencers[k] == null)
                continue;

            var inf = influencers[k];
            var wPos = inf.transform.position;
            var lPos = WorldToGridPosition(wPos);

            for (int i = -inf.Radius; i <= inf.Radius; i++)
                for (int j = -inf.Radius; j <= inf.Radius; j++)
                {
                    int gridX = j + lPos.x;
                    int gridY = i + lPos.y;
                    if (IsInGrid(gridY, gridX))
                    {
                        grid[gridY, gridX].SetInfluece(inf.Type, 0);
                        steps++;
                        if (steps > maxStepsPerFrame)
                        {
                            yield return null;
                            steps = 0;
                        }
                    }
                }

        }

        //update affected tiles
        for (int k = 0; k < influencers.Count; k++)
        {
            if (influencers[k] == null)
                continue;

            var inf = influencers[k];
            var wPos = inf.transform.position;
            var lPos = WorldToGridPosition(wPos);


            for (int i = -inf.Radius; i <= inf.Radius; i++)
                for (int j = -inf.Radius; j <= inf.Radius; j++)
                {
                    int gridX = j + lPos.x;
                    int gridY = i + lPos.y;
                    if (IsInGrid(gridY, gridX))
                    {
                        float dist = Vector2Int.Distance(lPos, new Vector2Int(gridX, gridY));
                        float dec = inf.Decrease;
                        //var infEnt = inf.gameObject.GetCachedEntity();
                        float strength = (1 - (dist * dec - 1) / (1 + inf.Radius)) * inf.Influence;
                        grid[gridY, gridX].AddInfluence(inf.Type, strength);
                        steps++;
                        if (steps > maxStepsPerFrame)
                        {
                            yield return null;
                            steps = 0;
                        }
                        
                    }
                }
        }

        ClearInfluencers();
        updatesInProgress--;
    }

    private List<InfluenceObject> RequestInfluencers()
    {
        scanner.Scan(this);
        influencerList.Clear();
        foreach (var i in influencers)
            influencerList.Add(i);
        return influencerList;
    }



    private void ClearInfluencers()
    {
        influencers.Clear();
    }

    public void AddInfluencer(InfluenceObject influencer)
    {
        influencers.Add(influencer);
    }

    public Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x * spacing + spacing / 2, 0, y * spacing + spacing / 2);
    }

    public Vector2Int WorldToGridPosition(Vector3 position)
    {

        int x = Mathf.RoundToInt((position.x - spacing / 2) / spacing);
        int y = Mathf.RoundToInt((position.z - spacing / 2) / spacing);

        return new Vector2Int(x, y);
    }

    private void OnDrawGizmos()
    {
        if (grid == null || !drawMap)
            return;

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                float t = grid[j, i].GetInfluence(InfluenceType.Utility) / 5;
                Color startColor = Color.black, endColor = Color.yellow;

                switch (drawType)
                {
                    case InfluenceType.Utility:
                        t = grid[j, i].GetInfluence(InfluenceType.Utility) / 5;
                        startColor = Color.black;
                        endColor = Color.yellow;
                        break;
                    case InfluenceType.ZoneControl:
                        t = (10 + grid[j, i].GetInfluence(InfluenceType.ZoneControl)) / 20;
                        startColor = Color.green;
                        endColor = Color.red;
                        break;
                    case InfluenceType.MapFeature:
                        t = grid[j, i].GetInfluence(InfluenceType.MapFeature) / 10;
                        startColor = Color.black;
                        endColor = Color.white;
                        break;
                }

                Gizmos.color = Color.Lerp(startColor, endColor, t);
                Gizmos.DrawCube(grid[j, i].WorldPosition, Vector3.one * drawSize);
            }
    }

}
