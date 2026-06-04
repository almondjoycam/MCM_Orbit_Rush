using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    float lifespan = 12;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        lifespan -= Time.deltaTime;
    }
}
