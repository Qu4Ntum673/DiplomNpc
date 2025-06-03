using UnityEngine;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    public float speed = 2.0f;
    public float stopDistance = 1.5f;
    public float attackDistance = 2.0f;
    private Transform player;
    private Transform enemy;
    private bool followingPlayer = false;
    public Text interactionMessage;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy")?.transform;

        if (player == null)
        {
            Debug.LogError("Игрок с тегом 'Player' не найден!");
        }

        if (interactionMessage != null)
        {
            interactionMessage.gameObject.SetActive(false);
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (interactionMessage != null)
        {
            Vector3 textPosition = transform.position + new Vector3(0, 1.5f, 0);
            interactionMessage.transform.position = Camera.main.WorldToScreenPoint(textPosition);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToEnemy = enemy != null ? Vector3.Distance(transform.position, enemy.position) : float.MaxValue;

        if (distanceToPlayer <= stopDistance)
        {
            ShowInteractionMessage();
            if (Input.GetKeyDown(KeyCode.E))
            {
                followingPlayer = !followingPlayer;
            }
        }
        else
        {
            HideInteractionMessage();
        }

        if (followingPlayer && player != null)
        {
            FollowPlayer();
        }

        if (distanceToEnemy <= attackDistance)
        {
            AttackEnemyAnimation();
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    void ShowInteractionMessage()
    {
        if (interactionMessage != null)
        {
            interactionMessage.gameObject.SetActive(true);
            interactionMessage.text = "Нажмите 'E' для взаимодействия с питомцем.";
        }
    }

    void HideInteractionMessage()
    {
        if (interactionMessage != null)
        {
            interactionMessage.gameObject.SetActive(false);
        }
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (animator != null)
            {
                animator.SetFloat("speed", speed);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetFloat("speed", 0);
            }
        }
    }

    void AttackEnemyAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
        }
    }
}
