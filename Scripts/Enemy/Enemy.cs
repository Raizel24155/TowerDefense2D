using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private MenuStatsSO currentStatsSO;
    [SerializeField] private List<Collider2D> buildingsInRange = new List<Collider2D>();
    [SerializeField] private EnemyScriptableObject enemySO;
    private Rigidbody2D rb;
    private ContactFilter2D filter2D = new ContactFilter2D();
    private string playerTag = "Player";
    private string buildingLayerName = "Building";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySO.baseHealth += Random.Range(-enemySO.baseHealthFluctuations, enemySO.baseHealthFluctuations);
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Attack());
        filter2D.layerMask = LayerMask.NameToLayer(buildingLayerName);
    }

    private void FixedUpdate()
    {
        Vector3 direction = (GameObject.FindGameObjectWithTag(playerTag).transform.position - transform.position).normalized;

        // Apply force towards target
        rb.AddForce(direction * enemySO.baseMovementSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    IEnumerator Attack()
    {
        Physics2D.GetContacts(gameObject.GetComponent<BoxCollider2D>(), filter2D, buildingsInRange);
        foreach (Collider2D building in buildingsInRange)
        {
            if (building is BoxCollider2D)
            {
                if (building.tag == playerTag)
                {
                    currentStatsSO.health -= enemySO.baseDamage;
                }
                else
                {
                    building.GetComponent<BuildingFunction>().TakeDamage(enemySO.baseDamage);
                }
            }
        }
        yield return new WaitForSeconds(enemySO.baseAttackSpeed);
        StartCoroutine(Attack());
    }

    public void TakeDamage(int damageTaken )
    {
        enemySO.baseHealth -= damageTaken;
        if (enemySO.baseHealth <= 0)
        {
            currentStatsSO.GainExperience(enemySO.expDrop);
            Destroy(gameObject);
        }
    }

    public void SetEnemySO(EnemyScriptableObject enemy)
    {
        enemySO = enemy;
    }
}
