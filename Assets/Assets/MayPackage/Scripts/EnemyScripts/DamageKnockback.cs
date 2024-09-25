using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DamageKnockback : MonoBehaviour
{
    public PlayerDeath playerDeath;
    private CinemachineBrain vcam;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDeath.knockFromRight = (collision.transform.position.x > transform.position.x) ? false : true;

            playerDeath.Die();
        }
    }
}
