using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyScriptableObject")]
public class EnemyScriptableObject : ScriptableObject
{
    public string mobName;
    public GameObject prefab;
    public int baseHealth;
    public int baseDamage;
    public float baseAttackSpeed;
    public float baseMovementSpeed;
    public int expDrop;
    [Range(0f,100f)] public float chanceToSpawn;
    public string description;


    public int baseHealthFluctuations;
    public int baseDamageFluctuations;
    public float baseAttackSpeedFluctuations;
    public int baseMovementSpeedFluctuations;

    public EnemyScriptableObject Clone()
    {
        return Instantiate(this);
    }
}
