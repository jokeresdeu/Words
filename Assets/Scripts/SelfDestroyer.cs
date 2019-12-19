using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
