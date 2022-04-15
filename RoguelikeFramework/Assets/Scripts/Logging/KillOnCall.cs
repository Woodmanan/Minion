using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCall : MonoBehaviour
{
    public bool kill;

    private void Update()
    {
        if (kill) Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
