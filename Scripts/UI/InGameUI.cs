using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private MenuStatsSO baseStats;
    [SerializeField] private MenuStatsSO currentStats;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gameOver;
    [SerializeField] private Button speedButton;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private List<float> timeSpeed = new List<float>();
    

    private int currentGold;
    private int currentLevel;
    private int currentTimeSpeed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CopyStats();

        healthSlider.maxValue = currentStats.health;


        speedButton.onClick.AddListener(() =>
        {
            ChangeSpeed();
        });


        StartCoroutine(Tick());
    }

    // Update is called once per frame
    void Update()
    {
        if(currentGold != currentStats.gold)
        {
            UpdateGoldAmountUI();
        }

        if (healthSlider.value != currentStats.health)
        {
            HealthChangeUI();
        }

        if(currentLevel != currentStats.level)
        {
            currentLevel = currentStats.level;
            GameObject.Find("BuildingUpgradeUI").GetComponent<BuildingUpgradeUI>().ShowUpgrades();
        }

        if(currentStats.health <= 0)
        {
            gameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void UpdateTimerUI()
    {
        timerText.text = currentStats.timer.ToString();
    }

    private void UpdateGoldAmountUI()
    {
        goldText.text = "Gold: " +  currentStats.gold.ToString();
        currentGold = currentStats.gold;
    }

    private void ChangeSpeed()
    {
        currentTimeSpeed++;
        if(currentTimeSpeed == timeSpeed.Count)
        {
            currentTimeSpeed = 0;
        }
        speedButton.GetComponentInChildren<TextMeshProUGUI>().text = timeSpeed.ElementAt(currentTimeSpeed).ToString() + "x";
        Time.timeScale = timeSpeed.ElementAt(currentTimeSpeed);
    }

    private void HealthChangeUI()
    {
        healthSlider.value = currentStats.health;
    }

    private void CopyStats()
    {
        currentStats.timer = baseStats.timer;
        currentStats.health = baseStats.health;
        currentStats.gold = baseStats.gold;
        currentStats.level = baseStats.level;
        currentStats.experience = baseStats.experience;
        currentStats.experienceToLevelUp = new List<int>(baseStats.experienceToLevelUp);

        currentLevel = currentStats.level;
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1f);
        currentStats.timer += 1;
        UpdateTimerUI();
        StartCoroutine(Tick());
    }
}
