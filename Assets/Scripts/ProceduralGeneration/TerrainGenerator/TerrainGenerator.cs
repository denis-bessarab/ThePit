using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("LocationBanks")]
    [SerializeField] private LocationBank startLocation;
    [SerializeField] private LocationBank tunnelN;
    [SerializeField] private LocationBank tunnelE;
    [SerializeField] private LocationBank tunnelS;
    [SerializeField] private LocationBank tunnelW;
    [SerializeField] private LocationBank tunnelNE;
    [SerializeField] private LocationBank tunnelSE;
    [SerializeField] private LocationBank tunnelSW;
    [SerializeField] private LocationBank tunnelNW;

    [Header("Components")]
    [SerializeField] private Grid grid;


    private void Awake()
    {
        grid = FindGrid();

        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        if (grid == null) return;

        GenerateStartLocation(startLocation);
    }

    private void GenerateStartLocation(LocationBank startLocationBank)
    {
        LocationDescription location = GetRandomLocationFromBank(startLocationBank);

        GameObject go = Instantiate(location.locationPrefab, grid.gameObject.transform);

        Vector3 previousConnetionsPositionsSum = Vector3.zero;

        go.transform.position = previousConnetionsPositionsSum;

        for (int i = 0; i < location.connections.Count; i++)
        {
            LocationConnection lc = location.connections[i];
            GenerateConnection(lc, previousConnetionsPositionsSum);
        }
    }

    private void GenerateConnection(LocationConnection connection, Vector3 previousConnetionsPositionsSum)
    {
        ConnectionType type = GetRandomConnectionType(connection);

        LocationBank bank = ChooseBankFromType(type);

        if (bank == null) return;

        LocationDescription location = GetRandomLocationFromBank(bank);

        GameObject go = Instantiate(location.locationPrefab, grid.gameObject.transform);
        previousConnetionsPositionsSum += connection.connectionPosition;

        go.transform.position = previousConnetionsPositionsSum;

        for (int i = 0; i < location.connections.Count; i++)
        {
            LocationConnection lc = location.connections[i];
            GenerateConnection(lc, previousConnetionsPositionsSum);
        }
    }

    private LocationDescription GetRandomLocationFromBank(LocationBank bank)
    {
        return bank.locations[Random.Range(0, bank.locations.Count)];
    }

    private ConnectionType GetRandomConnectionType(LocationConnection connection)
    {
        return connection.possibleConnectionTypes[Random.Range(0, connection.possibleConnectionTypes.Count)];
    }

    private Grid FindGrid()
    {
        return FindAnyObjectByType<Grid>();
    }

    private LocationBank ChooseBankFromType(ConnectionType type)
    {
        return type switch
        {
            ConnectionType.TunnelN => tunnelN,
            ConnectionType.TunnelE => tunnelE,
            ConnectionType.TunnelS => tunnelS,
            ConnectionType.TunnelW => tunnelW,
            ConnectionType.TunnelNE => tunnelNE,
            ConnectionType.TunnelSE => tunnelSE,
            ConnectionType.TunnelSW => tunnelSW,
            ConnectionType.TunnelNW => tunnelNW,
            _ => null,
        };
    }
}
