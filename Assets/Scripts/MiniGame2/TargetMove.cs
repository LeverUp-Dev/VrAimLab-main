using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    [SerializeField] private bool Left;
    [SerializeField] private float speed;

    MoveGameManager mgm;

    private void Awake()
    {
        mgm = FindObjectOfType<MoveGameManager>();
    }

    void Start()
    {
        StartCoroutine(MoveTo());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            mgm.score++;
            mgm.scoreTxt.text = "Á¡¼ö: " + mgm.score.ToString();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator MoveTo()
    {
        yield return null;

        if (!Left)
        {
            while (true)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                if (transform.position.z > 13) break;

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
                if (transform.position.z < -13) break;

                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
