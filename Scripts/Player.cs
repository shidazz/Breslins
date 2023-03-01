using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;
    
    private AnimationController anim;

    public  float speed = 5f;

    void Awake()
    {
        anim = GetComponent<AnimationController>();
    }

    public void Move(float input)
    {
        transform.Translate(input * speed * Time.deltaTime, 0, 0);
        //anim.UpdateAnimations("direction", input);
    }

    public float Spin(float input)
    {
        return input;
    }

    public IEnumerator Swing(float direction, float spin)
    {
        anim.UpdateAnimations("swing", direction);
        Debug.Log("swing");
        yield return new WaitUntil(() => ball.hasHit);
        Debug.Log("hit");
        trajectory.MovePoints(direction, "near", spin);
        Debug.Log("spin: " + spin);
    }

    public void ResetBall()
    {
        ball.coroutineAllowed = true;
        ball.speedModifier = 1;
    }
}
