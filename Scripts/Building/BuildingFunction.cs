using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BuildingFunction : MonoBehaviour
{
    [SerializeField] private TowerScriptableObject towerSO;
    [SerializeField] private MenuStatsSO currentStatsSO;
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;
    private int enemyLayerInt = 7;                                                          // Enemy Layer
    private float shootDelay = 0.1f;                                                        // delay beetween shooting bullets

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch(towerSO.buildingType)
        {
            case TowerScriptableObject.BuildingType.attack:
                GetComponent<CircleCollider2D>().radius = towerSO.attackRange;
                StartCoroutine(Reload());
            break;
            case TowerScriptableObject.BuildingType.defense: 

            break;
            case TowerScriptableObject.BuildingType.coinGenerator:
                 StartCoroutine(GenerateCoins());
            break;
        }
    }

    IEnumerator Reload()
    {
        SortEnemyInRangeList();
        if (enemiesInRange.Count > 0)
        {
            for (int i = towerSO.bulletsAmount; i > 0;)
            {
                if (enemiesInRange.Count == 0)
                {
                    break;
                }
                for (int j = 0; j < enemiesInRange.Count; j++)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    else
                    {
                        i--;
                        GameObject bullet = Instantiate(this.bulletPrefab, this.gameObject.transform);

                        bullet.GetComponent<Bullet>().SetDestination(enemiesInRange[j].transform);
                        bullet.GetComponent<Bullet>().SetRange(towerSO.attackRange);
                        bullet.GetComponent<Bullet>().SetDamageToDeal(towerSO.damage);
                        yield return new WaitForSeconds(shootDelay);
                    }
                }
            }
        }
        yield return new WaitForSeconds(towerSO.attackSpeed);
        StartCoroutine(Reload());
    }

    private IEnumerator GenerateCoins()
    {
        yield return new WaitForSeconds(towerSO.attackSpeed);
        currentStatsSO.GainGold(towerSO.damage);
        StartCoroutine(GenerateCoins());
    }

    private IEnumerator ShowTakenDmg()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void SortEnemyInRangeList()
    {
        enemiesInRange.Sort((enemy1,enemy2) => Vector3.Distance(enemy1.transform.position,transform.position).CompareTo(Vector3.Distance(enemy2.transform.position, transform.position)));
    }
    public void TakeDamage(int damage)
    {
        towerSO.health -= damage;
        StartCoroutine(ShowTakenDmg());
        if (towerSO.health <= 0) 
        {
            Data.Instance.BudynekZniszczony(transform.position);
            Destroy(gameObject);
        }
    }

    public void SetTowerSO(TowerScriptableObject tower)
    {
        towerSO = tower;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == enemyLayerInt)
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == enemyLayerInt)
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }
}