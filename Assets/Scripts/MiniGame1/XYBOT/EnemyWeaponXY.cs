using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponXY : MonoBehaviour
{
    public GameObject enemyObj;
    public EnemyBaseXY enemySc;
    public GameObject targetObj;
    public DeffencePlayer targetSc;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent(out DeffencePlayer player) && enemySc.isAttack && !enemySc.AttackTypeSC)
        {
            targetSc.curHp -= enemySc.stats.attackDamage;
            print("µ¥¹ÌÁö!" + enemySc.stats.attackDamage);
        }
    }
}