using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItems : MonoBehaviour
{
    [SerializeField] float staminaToRemove, explosionRadius;
    [SerializeField] LayerMask playerLayer;
    [HideInInspector] public GameObject shadow;

    [SerializeField] ParticleSystem explosion;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.HitOnStamina(staminaToRemove);
        }
        else
        {
            Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);

            if (col.Length <= 0)
                return;

            for (int i = 0; i < col.Length; i++)
            {
                col[i].GetComponent<PlayerController>().HitOnStamina(staminaToRemove);
            }
        }



        Destroy(gameObject);
        Destroy(shadow);
    }
}
