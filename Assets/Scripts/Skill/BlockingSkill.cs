using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingSkill : MonoBehaviour
{

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Attack")
        {
            Destroy(other);
        }
    }
}
