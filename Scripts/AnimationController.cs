using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() 
    {
        //if (animator.GetFloat("swing") != 0)
        //    animator.SetFloat("swing", 0);
    }

    public void UpdateAnimations(string parameter, float input)
    {
        //animator.SetFloat(parameter, input);
    }
}
