using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    ///<Summary>
    ///This is an interface, you can put it on any object with a collider, and when the player clicks L, 
    ///this function will activate, if you don't know how to use an interface leave now :)
    ///</Summary>
    public void Activate(Transform _player = null, bool _buttonDown = true){}

}
