using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private TowerScriptableObject currentTower;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap grass;
    [SerializeField] private Tile grassTile;
    [SerializeField] private MenuStatsSO currentStats;
    [SerializeField] private TowerUpgradeScriptableObject tU;

    //private Dictionary<Vector2Int, bool> canPutBuilding = new Dictionary<Vector2Int, bool>();
    private int xMax = 10, yMax = 10;
    private void Start()
    {
        MakeGrid();
        GridPlayerIsOn();
        tU.GetUpgradeText();
    }

    void Update()
    {
        Vector3 mousePosition = inputManager.GetMousePositionVector2();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject.Find("BuildingUpgradeUI").GetComponent<BuildingUpgradeUI>().ShowUpgrades();
        //}

        if (Physics2D.Raycast(mousePosition, Vector3.forward, 100, gridLayer) && !EventSystem.current.IsPointerOverGameObject())
        {
            cellIndicator.SetActive(true);
            cellIndicator.transform.position = gridPosition + new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0);
        }
        else
        {
            cellIndicator.SetActive(false);
        }
        Vector2Int gridPosition2 = new Vector2Int(gridPosition.x,gridPosition.y);
        if (currentTower != null && Input.GetMouseButton(0) && Data.Instance.canPutBuilding.TryGetValue(gridPosition2, out bool point) && !EventSystem.current.IsPointerOverGameObject() && currentStats.IsGoldEnough(currentTower.cost))
        {
            // Sprawdza czy mozna miejsce jest wolne w danym punkcie
            // Check if the place is occupied 
            if (point == false)
            {
                // Postawic budynek i zmienic miejsce na zajete
                // Build a building and set this place as occupied
                Data.Instance.canPutBuilding[gridPosition2] = true;
                GameObject building = Instantiate(currentTower.prefab, gridPosition + new Vector3(grid.cellSize.x, grid.cellSize.y, 0) * 0.5f, Quaternion.identity);
                building.transform.parent = GameObject.Find("Buildings").transform;
                building.GetComponent<BuildingFunction>().SetTowerSO(currentTower.CloneWithList());
                currentStats.LoseGold(currentTower.cost);
            }
        }
    }

    // Makes Grid in which you can place Buildings
    void MakeGrid()
    {
        for (int i = 0; i < xMax; i++)
        {
            for (int j = 0; j < yMax; j++)
            {
                grass.SetTile(new Vector3Int(i, j, 0), grassTile);
                Data.Instance.canPutBuilding.Add((Vector2Int)new Vector3Int(i, j, 0), false);
            }
        }
    }

    // Find player and change places its on to occupied
    void GridPlayerIsOn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < player.transform.lossyScale.x; i++)
        {
            for (int j = 0; j < player.transform.lossyScale.y; j++)
            {
                Data.Instance.canPutBuilding[new Vector2Int((int)player.transform.position.x - i, (int)player.transform.position.y - j)] = true;
            }
        }
    }

    public void SetActiveBuildingType(TowerScriptableObject towerScriptableObject)
    {
        currentTower = towerScriptableObject;
    }

    public TowerScriptableObject GetActiveBuildingType()
    {
        return currentTower;
    }
}

