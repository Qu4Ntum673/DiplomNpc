using UnityEngine;

public class PC : MonoBehaviour

{
    [SerializeField]private AudioClip m_Clip;
    private AudioSource m_Source;

    public int health = 100;
    public float speed;
    private Vector2 direction;
    public Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_Source = rb.GetComponent<AudioSource>();
    }


    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }
    private void PlaySoundStep()
    {
        m_Source.PlayOneShot(m_Clip, 1f);
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took damage: " + damage + ". Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player died!");
        
    }
}
