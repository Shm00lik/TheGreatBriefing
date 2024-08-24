using UnityEngine;

public class RamiController : MonoBehaviour
{
    private const float ABSOLUT_MIN_Y = -6f;

    [Min(0)]
    public float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float speed = GetSpeed(GameManager.instance.ramisState.speedMultiplier);

        transform.position -= new Vector3(0, speed * Time.deltaTime);

        if (transform.position.y < ABSOLUT_MIN_Y)
        {
            GameManager.instance.DestroyRami(this.gameObject);
        }
    }

    public float GetSpeed(float speedMultiplier)
    {
        return baseSpeed * speedMultiplier;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerPrefs.GetInt("enableCheats") == 1) return;

        if (other.tag == "Player")
        {
            StartCoroutine(GameManager.instance.PlayerCollideRami(this.gameObject));
        }
    }
}
