using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DeffencePlayer : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public int maxHP;
    public float curHp;
    [SerializeField] private GameObject failtext;
    void Start()
    {
        curHp = maxHP;
    }
    void Update() {
        lifeText.text = "현재 체력 : " + curHp.ToString();
        if (curHp <= 0)
            failtext.SetActive(true);
    }
}
