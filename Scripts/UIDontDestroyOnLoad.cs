using UnityEngine;

public class UIDontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectOfType<UIDontDestroyOnLoad>() != this)
            Destroy(gameObject);
    }
}
