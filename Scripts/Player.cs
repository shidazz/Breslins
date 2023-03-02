using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private BallPhysics ball;
    
    public  float speed = 5f;

    private AnimationController anim;

    void Awake()
    {
        anim = GetComponent<AnimationController>();
    }

    void Update()
    {
        if (transform.position.z <= -6)
            transform.Translate(0, 0, 1f * Time.deltaTime);
    }

    public void Move(Vector2 input)
    {
        transform.Translate(input.x * speed * Time.deltaTime, 0, input.y * speed * Time.deltaTime);
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
