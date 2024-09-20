using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag=="Playerr"||collision.transform.tag=="Enemy")
        {
            collision.transform.GetComponent<UltimateChek>().HaveUltimate = true;
            Destroy(transform.gameObject);
            if (collision.transform.tag=="Playerr")
            {
                GameManager.Instance.PLayer.GetComponent<PlayerMovement>().Circle.localScale *= 1.5f;
                GameManager.Instance.Armature.GetComponent<PlayerAttack>().detectionRadius *= 1.5f;
            }
        }
    }
}
