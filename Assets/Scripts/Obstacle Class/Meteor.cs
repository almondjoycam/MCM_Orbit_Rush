using UnityEngine;

public class Meteor : MonoBehaviour
{
    MeteorShower parentShower;
    Vector3 direction;
    float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentShower = transform.parent.GetComponent<MeteorShower>();
        direction = new Vector3(
            Mathf.Cos(parentShower.meteorAngle) + Random.value,
            Mathf.Sin(parentShower.meteorAngle) + Random.value
        );
        speed = parentShower.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentShower.Hurt(other.GetComponent<PlayerControls>());
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        transform.position = parentShower.transform.position;
    }

    void OnBecomeInvisible()
    {
        gameObject.SetActive(false);
        parentShower.OnChildInactive();
    }
}
