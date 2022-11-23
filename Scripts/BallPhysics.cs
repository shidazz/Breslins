using System.Collections;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Trajectory trajectory;

    private float tParam = 0f;
    private Vector3 objectPosition;
    private int counter = 0;
    public bool coroutineAllowed = false;
    public bool hasHit = false;
    public float speedModifier = 0.5f;
    public static Vector3 returnStartPosition;

    void Update()
    {
        if (coroutineAllowed && counter == 0)
        {
            StartCoroutine(GoByTheRoute());
        }
    }

    private IEnumerator GoByTheRoute()
    {
        counter++;
        coroutineAllowed = false;

        Vector3[] p = new Vector3[8];

        for (int i = 0; i < trajectory.controlPoints.Length; i++)
            p[i] = trajectory.controlPoints[i].position;

        for (int i = 0; i <= 4; i += 4)
        {
            while (tParam < 1)
            {
                tParam += Time.deltaTime * speedModifier;

                objectPosition = Mathf.Pow(1 - tParam, 3) * p[i] + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p[i+1] + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p[i+2] + Mathf.Pow(tParam, 3) * p[i+3];

                transform.position = objectPosition;

                yield return new WaitForEndOfFrame();

                if (hasHit)
                {
                    hasHit = false;
                    break;
                }
            }
            tParam = 0;
        }
        counter = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            if (collision.gameObject.CompareTag("Paddle")) 
            {
                hasHit = true;
                returnStartPosition = transform.position;
                tParam = 0;
                coroutineAllowed = true;
                speedModifier += 0.05f;
            }
        }
    }
}
