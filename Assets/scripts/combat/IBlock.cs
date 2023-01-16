using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock : IdirectionalInput
{
    public Collider shieldTrigger { get; }
    public bool isBlocking { get; set; }
}
