using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DeffencePlayer : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public int maxHP;
    public float curHp;

    void Start()
    {
        curHp = maxHP;
    }
    void Update() {
        lifeText.text = "ÇöÀç Ã¼·Â : " + curHp.ToString();
        if (curHp <= 0)
            print("À¸¾ÇÁ×À½");
    }
}
