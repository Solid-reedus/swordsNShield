using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock : IdirectionalInput
{
    //public Transform BlockTarget { get; }
    public Collider shieldTrigger { get; }
    public bool isBlocking { get; set; }
}
