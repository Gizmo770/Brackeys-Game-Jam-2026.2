using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Laser : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position = transform.position + transform.up * speed * Time.deltaTime;
    }
}
