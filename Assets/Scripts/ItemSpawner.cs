using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class ItemSpawner : MonoBehaviour
{
    [Serializable]
    public class Menus
    {
        public GameObject Parents;
        public GameObject[] Ui;
        public GameObject[] Objects;
    }

    public GameObject SpawnPoint;

    public Menus[] Spawnables;
    public int selectedElement = 0;
    public int selectedMenu = 0;
    void Start()
    {
        Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.yellow;
    }
    public void Next()
    {
        Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.white;
        if (selectedElement < Spawnables[selectedMenu].Ui.Length-1)
        {
            selectedElement++;
        }
        else
        {
            selectedElement = 0;
        }
        Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.yellow;
    }
    public void Prev()
    {
        Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.white;
        if (selectedElement > 0)
        {
            selectedElement--;
        }
        else if (selectedElement == 0) 
        {
            selectedElement = Spawnables[selectedMenu].Ui.Length - 1;
        }
        Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.yellow;
    }
    public void Select()
    {
        if(selectedMenu == 0)
        {
            Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.white;
            Spawnables[selectedMenu].Parents.SetActive(false);
            selectedMenu = selectedElement + 1;
            Spawnables[selectedMenu].Parents.SetActive(true);
            selectedElement = 0;
            Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.yellow;
        }
        else
        {
            GameObject SpawnedObject = Instantiate(Spawnables[selectedMenu].Objects[selectedElement], SpawnPoint.transform.position, Quaternion.identity);
        }
    }
    public void GoBack()
    {
        if(selectedMenu != 0)
        {
            Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.white;
            Spawnables[selectedMenu].Parents.SetActive(false);
            selectedMenu = 0;
            Spawnables[selectedMenu].Parents.SetActive(true);
            selectedElement = 0;
            Spawnables[selectedMenu].Ui[selectedElement].gameObject.GetComponent<TextMeshPro>().color = Color.yellow;
        }
    }
}
