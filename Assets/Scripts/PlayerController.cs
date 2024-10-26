using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform rightLimit;
    public Transform leftLimit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (touchPos.x > 0)
            {
                dir = Vector2.right;
            }
            else
            {
                dir = Vector2.left;
            }   

            if (transform.position.x < rightLimit.position.x && dir == Vector2.right)
            {
                transform.Translate(dir * speed * Time.deltaTime);
            }
            else if (transform.position.x > leftLimit.position.x && dir == Vector2.left)
            {
                transform.Translate(dir * speed * Time.deltaTime);
            }

            return;
        }

        dir = new Vector2(Input.GetAxis("Horizontal"), 0);

        if (dir.x > 0.2f)
        {
            dir = Vector2.right;
        }
        else if (dir.x < -0.2f)
        {
            dir = Vector2.left;
        } else
        {
            dir = Vector2.zero;
        }

        if (transform.position.x < rightLimit.position.x && dir.x > 0)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
        if (transform.position.x > leftLimit.position.x && dir.x < 0)
        {
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }
}
