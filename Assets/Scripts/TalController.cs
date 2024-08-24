using UnityEngine;

public class TalController : MonoBehaviour
{
    private const float ABSOLUT_MIN_Y = -7f;

    [Min(0)]
    public float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float speed = GetSpeed(GameManager.instance.talsState.speedMultiplier);

        transform.position -= new Vector3(0, speed * Time.deltaTime);

        if (transform.position.y < ABSOLUT_MIN_Y)
        {
            GameManager.instance.DestroyTal(this.gameObject);
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
            StartCoroutine(GameManager.instance.PlayerCollideTal(this.gameObject));
        }
    }
}
