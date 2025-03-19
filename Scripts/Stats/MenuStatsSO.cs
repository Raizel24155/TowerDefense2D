using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuStatsSO", menuName = "Scriptable Objects/MenuStatsSO")]
public class MenuStatsSO : ScriptableObject
{
    public int level = 0;
    public int experience = 0;
    public float timer = 0;
    public float health = 1000;
    public int gold = 100;
    public List<int> experienceToLevelUp = new List<int>();

    public void GainExperience(int exp)
    {
        experience += exp;
        if(experienceToLevelUp.Count > 0)
        {
            if (experience >= experienceToLevelUp[0])
            {
                experience -= experienceToLevelUp[0];
                experienceToLevelUp.RemoveAt(0);
                level++;
            }
        }
    }

    public void GainGold(int amount)
    {
        gold += amount;
    }

    public void LoseGold(int amount)
    {
        gold -= amount;
    }

    public bool IsGoldEnough(int amount)
    {
        if (gold - amount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
