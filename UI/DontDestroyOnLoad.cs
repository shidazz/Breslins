using UnityEngine;
using UnityEngine.UIElements;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectOfType<DontDestroyOnLoad>() != this)
            Destroy(gameObject);
    }
}
