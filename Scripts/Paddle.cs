using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Animator animator;
    private BoxCollider hitbox;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        hitbox = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            hitbox.enabled = true;
        else
            hitbox.enabled = false;
    }
}
