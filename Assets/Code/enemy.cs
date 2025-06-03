using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float detectionRange = 5.0f;
    public float stopDistance = 5.0f;
    public int damage = 10;
    public int maxHealth = 100;
    private int currentHealth;
    private Vector3 initialPosition;
    private Transform player;
    private Animator animator;

    void Start()
    {
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            if (distanceToPlayer > stopDistance)
            {
                FollowPlayer();
            }
            else
            {
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            animator.SetBool("isAttacking", false);
            ReturnToInitialPosition();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void ReturnToInitialPosition()
    {
        if (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            Vector3 direction = (initialPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        gameObject.SetActive(false);
    }
}
