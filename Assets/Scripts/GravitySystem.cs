using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    [SerializeField]
    float gravityConstant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics2D.gravity = new Vector2(0, -gravityConstant);
    }
}
