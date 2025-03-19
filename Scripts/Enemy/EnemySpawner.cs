using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MenuStatsSO currentStatsSO;
    [SerializeField] private List<EnemyScriptableObject> mobList = new List<EnemyScriptableObject>();
    [SerializeField] private float spawnCooldown = 7.0f;
    private Dictionary<EnemyScriptableObject, float> mobWeightList = new Dictionary<EnemyScriptableObject, float>(); 
    private float spawnRange = 12.0f;
    private int everyHundredSec = 1;
    private float accumulativeWeights = 0;
    private System.Random rand = new System.Random();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CalculateWeight();
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());

    }

    IEnumerator SpawnEnemy()
    {
        if(currentStatsSO.timer/100 >= 0)
        {
            everyHundredSec = Mathf.FloorToInt(currentStatsSO.timer/100) + 1;
        }
        float tempCumulative = 0;
        foreach(EnemyScriptableObject enemy in mobList)
        {
            tempCumulative += enemy.chanceToSpawn;
        }
        if(tempCumulative != accumulativeWeights)
        {
            CalculateWeight();
        }

        float randomAngle = Random.Range(0, 360);
        float x = spawnRange * Mathf.Sin(randomAngle);
        float y = spawnRange * Mathf.Cos(randomAngle);
        int mobIndex = GetEnemyIndex();
        GameObject mob = Instantiate(mobList[mobIndex].prefab, new Vector3(transform.position.x + x,transform.position.y + y,0), Quaternion.identity);
        mob.transform.parent = GameObject.Find("Enemies").transform;
        mob.GetComponent<Enemy>().SetEnemySO(mobList[mobIndex].Clone());
        yield return new WaitForSeconds(spawnCooldown/everyHundredSec);
        StartCoroutine(SpawnEnemy());
        
    }

    private int GetEnemyIndex()
    {
        double chance = rand.NextDouble() * accumulativeWeights;
        for(int i = 0; i < mobList.Count; i++)
        {
            if(mobList[i] == null) continue;
            float mobWeight;
            mobWeightList.TryGetValue(mobList[i], out mobWeight);

            if (mobWeight >= chance)
            {
                return i;
            }
        }
        return 0;
    }

    private void CalculateWeight()
    {
        accumulativeWeights = 0;
        mobWeightList.Clear();
        foreach(EnemyScriptableObject enemySO in mobList)
        {
            accumulativeWeights += enemySO.chanceToSpawn;
            mobWeightList.Add(enemySO, accumulativeWeights);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,spawnRange);
    }
}
