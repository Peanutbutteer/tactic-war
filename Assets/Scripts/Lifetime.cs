using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour {

    public float objectTime = 2;
    void Start()
    { 
        Destroy(gameObject, objectTime);
    }
}
