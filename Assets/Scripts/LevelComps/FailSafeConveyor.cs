using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailSafeConveyor : MonoBehaviour
{

    [SerializeField, Tooltip("negative values go in reverse, positive values go forward")]
    public Vector3 TransportDirection;
    public float TransportSpeed;
    private List<Rigidbody> rigidbodiesOnConveyor;
    private Transform player;

    private void Start()
    {

        rigidbodiesOnConveyor = new List<Rigidbody>();

    }

    private void OnCollisionEnter(Collision _col)
    {

        if(!_col.rigidbody)
            return;
        if(_col.gameObject.tag.Equals("Player"))
            player = _col.transform;
        else
            rigidbodiesOnConveyor.Add(_col.rigidbody);
        
    }

    private void OnCollisionExit(Collision _col)
    {

        if(!_col.rigidbody)
            return;
        if(_col.gameObject.tag.Equals("Player"))
            player = null;
        else if(rigidbodiesOnConveyor.Remove(_col.rigidbody))
        {

            _col.rigidbody.velocity = TransportDirection * TransportSpeed;

        }

    }

    private void FixedUpdate()
    {

        for(int i = 0; i < rigidbodiesOnConveyor.Count; i++)
        {

            if(!rigidbodiesOnConveyor[i])
            {

                rigidbodiesOnConveyor.RemoveAt(i);
                i--;

                continue;

            }
            if(!rigidbodiesOnConveyor[i].GetComponent<Collider>().enabled)
            {

                rigidbodiesOnConveyor.RemoveAt(i);
                i--;

                continue;

            }

            rigidbodiesOnConveyor[i].transform.position += TransportDirection * (TransportSpeed / 60);

        }

        if (player)
            player.position += TransportDirection * (TransportSpeed / 60);

    }

}
