using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Vector3 cellSize = Vector3.one;
    [Min(1)]public int cellRange = 1;

    public Transform playerTransform;

    SpawnPartitions m_spawnPartitions;

    private void Awake()
    {
        m_spawnPartitions = new SpawnPartitions(cellSize);

        var spawnerArray = FindObjectsOfType<EnemySpawner>();

        foreach(EnemySpawner spawner in spawnerArray)
        {
            spawner.spawnManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HashSet<SpawnLocation> GetPlayerAdjacentCellSpawnLocations()
    {
        HashSet<SpawnLocation> result = new HashSet<SpawnLocation>();
        foreach (IVec3 cellArray in GetPlayerAdjacentCells())
        {
            HashSet<SpawnLocation> locations = m_spawnPartitions.GetEnemySpawners(cellArray);
            if(locations != null)
            {
                result.UnionWith(locations);
            }
        }

        return result;
    }

    public IVec3 GetPlayerCell()
    {
        return m_spawnPartitions.GetCell(playerTransform.position);
    }

    public IVec3[,,] GetPlayerAdjacentCells()
    {
        IVec3 playerCell = GetPlayerCell();
        int arraySize = 1 + (cellRange * 2);
        IVec3[,,] playerCellArray = new IVec3[arraySize, arraySize, arraySize];

        for(int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                for (int z = 0; z < arraySize; z++)
                {
                    IVec3 adjacentCell = IVec3.zero;
                    adjacentCell.x = playerCell.x + x - cellRange;
                    adjacentCell.y = playerCell.y + y - cellRange;
                    adjacentCell.z = playerCell.z + z - cellRange;

                    playerCellArray[x, y, z] = adjacentCell;
                }
            }
        }

        return playerCellArray;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        SpawnPartitions spawnPartitions = m_spawnPartitions;
        if (spawnPartitions == null)
        {
            spawnPartitions = new SpawnPartitions(cellSize);
        }

        IVec3 playerCell = spawnPartitions.GetCell(playerTransform.position);

        int arraySize = 1 + (cellRange * 2);
        IVec3[,,] playerCellArray = new IVec3[arraySize, arraySize, arraySize];

        for (int x = 0; x < arraySize; x++)
        {
            for (int y = 0; y < arraySize; y++)
            {
                for (int z = 0; z < arraySize; z++)
                {
                    IVec3 adjacentCell = IVec3.zero;
                    adjacentCell.x = playerCell.x + x - cellRange;
                    adjacentCell.y = playerCell.y + y - cellRange;
                    adjacentCell.z = playerCell.z + z - cellRange;

                    playerCellArray[x, y, z] = adjacentCell;
                }
            }
        }

        foreach (var cell in playerCellArray)
        {
            Vector3 pos = Vector3.zero;
            pos.x += cell.x * cellSize.x;
            pos.y += cell.y * cellSize.y;
            pos.z += cell.z * cellSize.z;
            Gizmos.DrawWireCube(pos, cellSize);
        }
    }
}
