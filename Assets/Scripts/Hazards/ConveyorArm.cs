using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorArm : MonoBehaviour
{

    public float MovePower;

    private void OnCollisionStay(Collision _col)
    {

        if(_col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            Vector3 _tempV3 = _col.transform.position - transform.position;
            _tempV3.y = 0;
            _col.transform.position += _tempV3.normalized * MovePower * Time.deltaTime;

        }

    }

}
