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
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < rightLimit.position.x)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        } 
        else if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > leftLimit.position.x)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.PlayerCollide(other.gameObject);
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }
}
