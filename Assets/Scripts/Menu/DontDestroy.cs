using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static bool flag = true;

    /* -------------------------------------------------------------------------------- */

    private void Awake()
    {
        if (flag)
        {
            DontDestroyOnLoad(gameObject);
            flag = false;
        }
        else
            Destroy(gameObject);
    }
}
