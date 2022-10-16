using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPenaltyTrigger : MonoBehaviour
{

    public LevelManager ThisLevelsManager;
    public int Penalty;

    private void OnTriggerEnter(Collider _col)
    {

        if(_col.TryGetComponent<GrabbableItem>(out GrabbableItem _item))
        {

            ThisLevelsManager.UpdateMoney(Penalty);
            Destroy(_item.gameObject);

        }

    }

}
