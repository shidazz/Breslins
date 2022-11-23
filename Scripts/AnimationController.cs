using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() 
    {
        if (animator.GetBool("inSwing"))
            animator.SetBool("inSwing", false);
    }

    public void UpdateAnimations(string input)
    {
        if (input == "Forehand")
        {
            animator.SetBool("inSwing", true);
        }
    }
}
