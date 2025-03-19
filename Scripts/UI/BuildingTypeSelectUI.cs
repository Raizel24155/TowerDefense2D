using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private List<TowerScriptableObject> baseTowerSOList;
    [SerializeField] private List<TowerScriptableObject> currentTowerSOList;
    [SerializeField] private GridSystem gridSystem;

    private Dictionary<TowerScriptableObject, Transform> towerDictionary;
    private void Awake()
    {
        foreach(TowerScriptableObject buildingType in baseTowerSOList)
        {
            currentTowerSOList.Add(buildingType.CloneEmpty());
        }


        Transform buildingButtonTemplate = transform.Find("BuildingButtonTemplate");
        buildingButtonTemplate.gameObject.SetActive(false);

        towerDictionary = new Dictionary<TowerScriptableObject, Transform>();

        int index = 0;
        foreach (TowerScriptableObject buildingType in currentTowerSOList)
        {
            Transform buildingButtonTransform = Instantiate(buildingButtonTemplate, transform);
            buildingButtonTransform.gameObject.SetActive(true);

            buildingButtonTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(index * 175, 0);
            buildingButtonTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;

            buildingButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                gridSystem.SetActiveBuildingType(buildingType);
                UpdateVisuals();
            });

            towerDictionary[buildingType] = buildingButtonTransform;

            index++;
        }
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public List<TowerScriptableObject> GetBuildingList()
    {
        return currentTowerSOList;
    }

    public void GetTowerUpgrade(TowerScriptableObject.BuildingType buildingType, TowerUpgradeScriptableObject card)
    {
        foreach(TowerScriptableObject towerScriptableObject in currentTowerSOList)
        {
            if(towerScriptableObject.buildingType == buildingType)
            {
                towerScriptableObject.towerUpgradeList.Add(card);
            }
        }
    }

    private void UpdateVisuals()
    {
        foreach(TowerScriptableObject building in towerDictionary.Keys)
        {
            towerDictionary[building].Find("selected").gameObject.SetActive(false);
        }

        TowerScriptableObject activeBuilding = gridSystem.GetActiveBuildingType();
        if(activeBuilding != null)
        {
            towerDictionary[activeBuilding].Find("selected").gameObject.SetActive(true);
        }
    }
}
