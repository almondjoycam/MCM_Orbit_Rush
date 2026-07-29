using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 40f;

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}