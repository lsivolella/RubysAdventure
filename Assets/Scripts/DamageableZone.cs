using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableZone : MonoBehaviour
{
    [SerializeField] int healthAmount = -1;
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller)
        {
            controller.ChangeHealth(healthAmount);
            Debug.Log("Trigger event");
        }
    }
}
