using UnityEngine;

public class Paddle : MonoBehaviour
{
    private BallPhysics ball;
    private Animator animator;
    private BoxCollider hitbox;
    private bool collided = false;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        hitbox = GetComponent<BoxCollider>();
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallPhysics>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !collided)
        {
            hitbox.enabled = true;
            if (ball.hasHit)
                collided = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hitbox.enabled = false;
            collided = false;
        }
        else
            hitbox.enabled = false;
    }
}
