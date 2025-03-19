using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "Scriptable Objects/TowerScriptableObject")]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName;
    public GameObject prefab;
    public Sprite sprite;
    public int health;
    public int lvl = 0;
    public int damage;
    public int bulletsAmount;
    public float attackRange;
    public float attackSpeed;
    public int cost;
    public string description;

    public List<TowerUpgradeScriptableObject> towerUpgradeList = new List<TowerUpgradeScriptableObject>();

    public BuildingType buildingType;
    public enum BuildingType
    {
        attack,
        defense,
        coinGenerator
    }

    public void LevelUp()
    {
        if(lvl < towerUpgradeList.Count)
        {
            health += towerUpgradeList[lvl].health;
            damage += towerUpgradeList[lvl].damage;
            bulletsAmount += towerUpgradeList[lvl].bulletsAmount;
            attackRange += towerUpgradeList[lvl].attackRange;
            attackSpeed += towerUpgradeList[lvl].attackSpeed;
            lvl++;
        }
    }

    public int UpgradeCost()
    {
        return towerUpgradeList[lvl+1].upgradeCost;
    }

    public TowerScriptableObject CloneEmpty()
    {
        TowerScriptableObject tower = Instantiate(this);
        return tower;
    }

    public TowerScriptableObject CloneWithList()
    {
        TowerScriptableObject tower = Instantiate(this);
        tower.towerUpgradeList = this.towerUpgradeList;
        return tower;
    }

    public void AddTowerUpgrades(TowerUpgradeScriptableObject upgrade)
    {
        towerUpgradeList.Add(upgrade);
    }
}
