using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFPSKit;

public class BodyShoot : MonoBehaviour
{
    public Target target;

    public int hitCount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !target.rot && !collision.gameObject.GetComponent<Bullet>().isBulletContatct)
        {
            hitCount++;

            if (hitCount == 3)
            {
                target.rot = true;
                hitCount = 0;
            }

            target.PlaySound();
            collision.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.score += 1;
        }
    }
}
