using UnityEngine;

public class Cow : MonoBehaviour
{
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;
    private int randomSpot;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    void Update()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
                animator.SetBool("isWalking", false); 
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else
        {
            animator.SetBool("isWalking", true); 

            
            Vector2 direction = (moveSpots[randomSpot].position - transform.position).normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
    }
}
