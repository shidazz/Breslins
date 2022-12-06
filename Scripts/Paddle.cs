using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Transform player;

    private void Update()
    {
        FollowBall();
    }

    private void FollowBall()
    {
        float x;
        float z;

        x = Mathf.Clamp(ball.position.x - player.position.x, player.position.x - 1, player.position.x + 1);
        if (x == player.position.x - 1 || x == player.position.x + 1)
            z = player.position.z;
        else
        {
            z = (-Mathf.Abs(x) + 0.5f) + player.position.z;
        }

        transform.position = new Vector3(x, 2.5f, z);
    }
}
