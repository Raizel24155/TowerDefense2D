using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 destination;
    private float bulletSpeed = 5.0f;
    private Rigidbody2D rb;
    private int damgeToDeal;
    private Vector3 direction;
    private float rangeOfDestruction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(direction * bulletSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        float distanceFromBuilding = Vector2.Distance(transform.position, transform.parent.transform.position);
        if (distanceFromBuilding > rangeOfDestruction)
        {
            Destroy(gameObject);
        }
    }

    public void SetDestination(Transform position)
    {
        destination = position.transform.position;
        direction = (destination - transform.position).normalized;
    }

    public void SetDamageToDeal(int dmg)
    {
        damgeToDeal = dmg;
    }

    public void SetRange(float range)
    {
        rangeOfDestruction = range;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemyComponent;
        collision.gameObject.TryGetComponent<Enemy>(out enemyComponent);
        enemyComponent.TakeDamage(damgeToDeal);
        Destroy(gameObject);
    }
}
