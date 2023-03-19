using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerController player = other.transform.GetComponent<PlayerController>();
            player.PlayerTakeDamage(damageAmount);

        }
    }

}
