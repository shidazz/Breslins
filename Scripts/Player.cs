using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;

    private float speed = 5f;
    private AnimationController anim;

    private void Awake()
    {
        anim = GetComponent<AnimationController>();
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
            trajectory.MovePoints(Random.Range(-2.5f, 0), 0, "left");
        if (direction > 0)
            trajectory.MovePoints(Random.Range(0, 2.5f), 0, "left");
    }

    public void ResetBall()
    {
        ball.coroutineAllowed = true;
        ball.speedModifier = 1;
    }
}
