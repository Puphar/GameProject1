using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructTime;
    private float _timer;

    void Start()
    {
        _timer = destructTime;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
