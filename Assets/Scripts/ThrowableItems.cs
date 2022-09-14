using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItems : MonoBehaviour
{
    [SerializeField] float staminaToRemove;
    [HideInInspector] public GameObject shadow;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (!playerController)
            return;

        playerController.HitOnStamina(staminaToRemove);

        Destroy(gameObject);
        Destroy(shadow);
    }
}
