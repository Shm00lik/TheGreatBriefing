using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Input.GetKey(KeyCode.A) ? Vector2.left : Input.GetKey(KeyCode.D) ? Vector2.right : Vector2.zero;

        transform.Translate(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.Eat(other.gameObject);
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }
}
