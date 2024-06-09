using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public bool overlap;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Target"))
        {
            overlap = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {


        if (collision.collider.gameObject.CompareTag("Target"))
        {
            overlap = false;
        }
    }
}
