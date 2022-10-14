using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItemOnTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item))
        {

            Destroy(_item.gameObject);

        }

    }

}
