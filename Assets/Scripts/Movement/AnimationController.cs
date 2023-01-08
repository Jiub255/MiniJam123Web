using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("XSpeed", rb.velocity.x);
        animator.SetFloat("YSpeed", rb .velocity.y);
    }
}