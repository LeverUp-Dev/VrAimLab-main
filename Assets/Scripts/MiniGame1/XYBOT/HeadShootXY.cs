using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRFPSKit;

public class HeadShootXY : MonoBehaviour
{
    public EnemyBaseXY target;
    public Text text;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")&& !target.isDie && !other.gameObject.GetComponent<Bullet>().isBulletContatct)
        {
            other.gameObject.GetComponent<Bullet>().isBulletContatct = true;
            target.stats.curHP -= 3;
            text.text += 3;
        }
    }
}
