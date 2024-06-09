using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float rotSpeed;
    public float returnSpeed;
    public int score;
    public bool rot;
    [SerializeField] private float rotX;
    [SerializeField] private AudioSource sound;

    public bool isFall = false;

    private void Awake() {
        print(transform.eulerAngles.x);
        
    }
    private void Update() {
        if (transform.eulerAngles.x < 90 && rot)
        {
            isFall = true;
            rotX = rotSpeed * Time.deltaTime;
            transform.eulerAngles += new Vector3(rotX, 0, 0);
            if(transform.eulerAngles.x >= 80)
            {
                rot = false;
                transform.eulerAngles = new Vector3(90f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }

    public void PlaySound()
    {
        sound.Play();
    }

    public void TryReset()
    {
        StartCoroutine(nameof(Reset));
    }

    IEnumerator Reset()
    {
        if (isFall == false)
            yield break;

        isFall = false;
        yield return new WaitForEndOfFrame();

        while (true)
        {
            Debug.Log("ÀÛµ¿Áß");
            //rotX = returnSpeed * Time.deltaTime;
            //transform.eulerAngles += new Vector3(-rotX, 0, 0);
            transform.Rotate(10, 0, 0);
            yield return null;

            if (transform.eulerAngles.x >= 0 && transform.eulerAngles.x <= 15)
                break;
        }

        transform.eulerAngles = Vector3.zero;

        yield break;
    }
}
