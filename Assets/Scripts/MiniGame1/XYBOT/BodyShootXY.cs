using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRFPSKit;

public class BodyShootXY : MonoBehaviour
{
    public EnemyBaseXY target;

    GameTimerXY gtXY;
    int count = 0;

    private void Start()
    {
        gtXY = FindObjectOfType<GameTimerXY>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !target.isDie && !collision.gameObject.GetComponent<Bullet>().isBulletContatct)
        {
            count++;
            if (count == 3)
            {
                Destroy(target.gameObject);
            }

            collision.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.stats.curHP -= 1;

            gtXY.scoreCount++;
            gtXY.score.text = "Á¡¼ö: " + gtXY.scoreCount;

            Destroy(collision.gameObject);
        }
    }
}
