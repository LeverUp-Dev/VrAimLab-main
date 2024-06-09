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
            if (hitCount == 2)
            {
                target.rot = true;
                hitCount = 0;
            }

            hitCount++;
            print(collision.gameObject.name);
            target.PlaySound();
            collision.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.score += 1;
            print("¹Ùµð¼¦!" + target.score);
        }
    }
}
