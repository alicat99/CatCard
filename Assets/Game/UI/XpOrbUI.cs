using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrbUI : MonoBehaviour
{
    [SerializeField]
    float width;
    
    private Vector3 target;
    private Vector3 velocity;

    public bool useClip = true;

    public void Initialize(Vector3 target, Vector3 velocity)
    {
        this.target = target;
        this.velocity = velocity;
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        velocity += new Vector3(0, -1000 * Time.deltaTime, 0);

        if (transform.position.y - 100 * transform.localScale.y < target.y)
        {
            transform.position = transform.position.SetY(target.y + 100 * transform.localScale.y);
            velocity.y *= -0.5f;
            velocity.x *= 0.9f;
        }

        if (!useClip)
        {
            if (velocity.sqrMagnitude < 50f)
            {
                Destroy(gameObject);
            }

            return;
        }

        if (transform.position.x < target.x - width && velocity.x < 0)
        {
            transform.position = transform.position.SetX(target.x - width);
            velocity.x *= -0.5f;
        }

        if (transform.position.x > target.x + width)
        {
            transform.position = transform.position.SetX(target.x + width);
            velocity.x *= -0.5f;
        }
    }
}
