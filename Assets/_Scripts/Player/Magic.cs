using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public float damage = 25f;
    public float speed = 20f;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // If not using rigidbody, manually move
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if it hits enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth eHealth = other.GetComponent<EnemyHealth>();
            if (eHealth != null)
                eHealth.TakeDamage(damage);
        }
        // Always destroy the projectile on collision
        Destroy(gameObject);
    }
}
