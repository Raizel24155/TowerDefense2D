using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeScriptableObject", menuName = "Scriptable Objects/TowerUpgradeScriptableObject")]
public class TowerUpgradeScriptableObject : ScriptableObject
{
    public string upgradeName;
    public GameObject prefab;
    public Sprite sprite;
    public int health;
    public int damage;
    public int bulletsAmount;
    public float attackRange;
    public float attackSpeed;
    public int upgradeCost;
    public TowerScriptableObject.BuildingType buildingType;

    public string GetUpgradeText()
    {
        string s = "";
        string formattedValue = "";
        if (health != 0)
        {
            formattedValue = (health >= 0 ? "+" : "") + health.ToString();
            s += "Health: " + formattedValue + "\n";
        }
        if(damage != 0)
        {
            formattedValue = (damage >= 0 ? "+" : "") + damage.ToString();
            s += "Damage: " + formattedValue + "\n";
        }
        if(bulletsAmount != 0)
        {
            formattedValue = (bulletsAmount >= 0 ? "+" : "") + bulletsAmount.ToString();
            s += "Bullets: " + formattedValue + "\n";
        }
        if(attackRange != 0)
        {
            formattedValue = (attackRange >= 0 ? "+" : "") + attackRange.ToString();
            s += "Attack Range: " + formattedValue + "\n";
        }
        if(attackSpeed != 0)
        {
            formattedValue = (attackSpeed >= 0 ? "+" : "") + attackSpeed.ToString();
            s += "Attack Speed: "+ formattedValue+ "\n";
        }
        if(upgradeCost != 0)
        {
            s += "Upgrade Cost: " + "<color=red>" +upgradeCost+"</color>" + "\n";
        }

        //Debug.Log(s);


        return s;
    }
}
