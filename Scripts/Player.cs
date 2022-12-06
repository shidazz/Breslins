using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;

    public  float speed = 5f;
    private AnimationController anim;

    private void Awake()
    {
        anim = GetComponentInChildren<AnimationController>();
    }

    public void Move(float input)
    {
        transform.Translate(input * speed * Time.deltaTime, 0, 0);
    }

    public IEnumerator Swing(float direction, float spin)
    {
        anim.UpdateAnimations("Forehand");
        Debug.Log("Swing");
        yield return new WaitUntil(() => ball.hasHit);
        Debug.Log("Hit");
        if (direction < 0)
            trajectory.MovePoints("left", "player", spin);
        if (direction > 0)
            trajectory.MovePoints("right", "player", spin);
    }

    public void ResetBall()
    {
        ball.coroutineAllowed = true;
        ball.speedModifier = 1;
    }
}
