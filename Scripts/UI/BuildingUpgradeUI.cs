using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradeUI : MonoBehaviour
{
    [SerializeField] private List<TowerUpgradeScriptableObject> upgradeSOList;
    [SerializeField] private List<TowerUpgradeScriptableObject> randomUpgradeSOList;
    [SerializeField] private BuildingTypeSelectUI buildingTypeSelectUI;
    [SerializeField] private GameObject stopPanel;

    [SerializeField] private List<Transform> cardsTransform = new List<Transform>();
    private bool isActive = false;
    private void Awake()
    {
        Transform upgradeButtonTemplate = transform.Find("UpgradeButtonTemplate");
        upgradeButtonTemplate.gameObject.SetActive(false);


        // Create Cards
        for(int i = 0; i < 3;i++)
        {
            Transform upgradeButtonTransform = Instantiate(upgradeButtonTemplate, transform);
            upgradeButtonTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(i * 500, 0);
            cardsTransform.Add(upgradeButtonTransform);
        }
    }

    // Select random 3 upgrades
    private void SelectUpgradeRandom()
    {
        int lottery = 3;
        if(upgradeSOList.Count < lottery)
        {
            lottery = upgradeSOList.Count;
        }
        while(lottery > 0)
        {
            int randomUpgrade = Random.Range(0, upgradeSOList.Count);
            if(!randomUpgradeSOList.Contains(upgradeSOList[randomUpgrade]))
            {
                randomUpgradeSOList.Add(upgradeSOList[randomUpgrade]);
                lottery--;
            }
        }
    }

    // Show upgrade cards
    public void ShowUpgrades()
    {
        if(isActive == false)
        {
            stopPanel.SetActive(true);
            isActive = true;
            float time = Time.timeScale;
            Time.timeScale = 0;
            randomUpgradeSOList.Clear();
            SelectUpgradeRandom();
            foreach (TowerUpgradeScriptableObject upgradeSO in randomUpgradeSOList)
            {
                int index = randomUpgradeSOList.IndexOf(upgradeSO);
                cardsTransform[index].gameObject.SetActive(true);

                cardsTransform[index].Find("Image").GetComponent<Image>().sprite = upgradeSO.sprite;
                cardsTransform[index].Find("Stats").GetComponent<TextMeshProUGUI>().text = upgradeSO.GetUpgradeText();

                cardsTransform[index].GetComponent<Button>().onClick.RemoveAllListeners();
                cardsTransform[index].GetComponent<Button>().onClick.AddListener(() =>
                {
                    buildingTypeSelectUI.GetTowerUpgrade(upgradeSO.buildingType, upgradeSO);
                    upgradeSOList.Remove(upgradeSO);
                    foreach (Transform card in cardsTransform)
                    {
                        card.gameObject.SetActive(false);
                    }
                    Time.timeScale = time;
                    // Acrivate method that adds the upgrade to tower List of upgrades
                    stopPanel.SetActive(false);
                    isActive = false;
                });
            }
        }
    }
}
