using System.Collections;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private Transform[] trajectory;

    private Trajectory t;
    private int path;
    private float tParam;
    private Vector3 objectPosition;
    private bool coroutineAllowed;
    private bool hasHit = false;
    public float speedModifier = 0.5f;
    public static Vector3 returnStartPosition;

    void Start()
    {
        t = trajectory[0].GetComponentInParent<Trajectory>();
        path = 0;
        tParam = 0f;
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(path));
        }
    }

    private IEnumerator GoByTheRoute(int num)
    {
        coroutineAllowed = false;

        Vector3 p0 = trajectory[num].GetChild(0).position;
        Vector3 p1 = trajectory[num].GetChild(1).position;
        Vector3 p2 = trajectory[num].GetChild(2).position;
        Vector3 p3 = trajectory[num].GetChild(3).position;

        while (tParam < 1)
        {
            if (coroutineAllowed)
                yield break;
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        path += 1;

        if (path > trajectory.Length - 1)
        {
            path = 0;
        }
        coroutineAllowed = true;
        hasHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            if (collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Wall")) 
            {
                hasHit = true;
                returnStartPosition = transform.position;
                t.MovePoints();
                path = 0;
                tParam = 0;
                coroutineAllowed = true;
                Debug.Log("Collision");
            }
        }
    }

    public void ResetBall()
    {
        transform.position = new Vector3(0, 3.5f, 5);
        coroutineAllowed = true;
    }
}
