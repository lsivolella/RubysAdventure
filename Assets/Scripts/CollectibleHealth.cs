using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    [SerializeField] AudioClip collectedClip;
    [SerializeField] int healthAmount = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller)
        {
            if (controller.GetCurrentHealth < controller.GetMaxHealth)
            {
                controller.ChangeHealth(healthAmount);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
        }
    }
}
