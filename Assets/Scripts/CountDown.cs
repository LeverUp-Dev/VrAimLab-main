using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public TMP_Text tmp;

    public void TextSet(string Text)
    {
        tmp.text = Text;
    } 
}
