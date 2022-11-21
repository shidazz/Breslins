using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;

    public void Move(float input)
    {
        transform.Translate(input * speed * Time.deltaTime, 0, 0);
    }
}
