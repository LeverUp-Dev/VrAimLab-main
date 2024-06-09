using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRFPSKit;

public class HeadShootXY : MonoBehaviour
{
    public EnemyBaseXY target;

    GameTimerXY gtXY;

    private void Start()
    {
        gtXY = FindObjectOfType<GameTimerXY>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !target.isDie && !collision.gameObject.GetComponent<Bullet>().isBulletContatct)
        {
            collision.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.stats.curHP -= 3;

            gtXY.scoreCount += 3;
            gtXY.score.text = "Á¡¼ö: " + gtXY.scoreCount;

            Destroy(collision.gameObject);
            Destroy(target.gameObject);
        }
    }
}
