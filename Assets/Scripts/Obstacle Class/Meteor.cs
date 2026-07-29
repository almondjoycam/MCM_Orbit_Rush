using UnityEngine;

public class Meteor : MonoBehaviour
{
    private MeteorShower parentShower;
    private Vector3 direction;
    private float speed;
    private bool reportedInactive;

    private void Awake()
    {
        parentShower = GetComponentInParent<MeteorShower>();
    }

    private void OnEnable()
    {
        reportedInactive = false;

        if (parentShower == null)
        {
            parentShower = GetComponentInParent<MeteorShower>();
        }

        if (parentShower == null)
        {
            Debug.LogError(
                "Meteor could not find a MeteorShower parent.",
                this
            );

            enabled = false;
            return;
        }

        float angleInRadians =
            parentShower.meteorAngle * Mathf.Deg2Rad;

        direction = new Vector3(
            Mathf.Cos(angleInRadians) +
                Random.Range(-0.15f, 0.15f),

            Mathf.Sin(angleInRadians) +
                Random.Range(-0.15f, 0.15f),

            0f
        ).normalized;

        speed = parentShower.MeteorMoveSpeed + Random.Range(-0.5f, 1.0f);

        float randomScale = Random.Range(0.6f, 1.4f);
        transform.localScale = Vector3.one * randomScale;
    }

    private void Update()
    {
        transform.Translate(
            direction * speed * Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerControls player =
            other.GetComponent<PlayerControls>();

        parentShower.Hurt(player);
        DeactivateMeteor();
    }

    private void OnBecomeInvisible()
    {
        DeactivateMeteor();
    }

    private void DeactivateMeteor()
    {
        if (reportedInactive)
            return;

        reportedInactive = true;

        if (parentShower != null)
        {
            parentShower.OnChildInactive();
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (parentShower != null)
        {
            transform.position = parentShower.transform.position;
        }
    }
}