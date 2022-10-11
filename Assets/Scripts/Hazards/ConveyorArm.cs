using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ConveyorArm : MonoBehaviour
{

    public float MovePower;
    public EventReference ConveyorHitSound;

    private void OnCollisionEnter(Collision _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            /*Vector3 _tempV3 = _col.transform.position - transform.position;
            _tempV3.y = 0;
            _col.transform.position += _tempV3.normalized * MovePower * Time.deltaTime;*/
            SoundManager.Instance.PlayOneShot(ConveyorHitSound, gameObject);

        }

    }

}
