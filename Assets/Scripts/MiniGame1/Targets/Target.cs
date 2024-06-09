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
        yield return new WaitForEndOfFrame();

        //if (!isFall)
        //    return;

        while (transform.eulerAngles.x > 0)
        {
            rotX = returnSpeed * Time.deltaTime;
            transform.eulerAngles -= new Vector3(rotX, 0, 0);
            //if (transform.eulerAngles.x >= 10)
            //{
            //    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
            //}
        }

        yield return new WaitUntil(() => transform.eulerAngles.x >= 10);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);

        isFall = true;

        yield break;
    }
}
