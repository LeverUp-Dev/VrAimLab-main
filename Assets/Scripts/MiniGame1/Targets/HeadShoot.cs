using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI.Table;
using VRFPSKit;

public class HeadShoot : MonoBehaviour
{
    public Target target;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !target.rot && !collision.gameObject.GetComponent<Bullet>().isBulletContatct)
        {
            target.rot = true;
            print(collision.gameObject.name);
            target.PlaySound();
            collision.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.score += 3;
            print("¹Ùµð¼¦!" + target.score);
        }
    }
}
