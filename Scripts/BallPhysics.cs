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

    public float speedModifier = 0.5f;

    void Start()
    {
        t = trajectory[0].gameObject.GetComponent<Trajectory>();
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

            //if statement for when collision with paddle and ball occurs here
            t.MovePoints();
        }

        coroutineAllowed = true;
        //test
    }
}
