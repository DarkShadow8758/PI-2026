using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_SpeedUp : Collectable
{
    public override void ApllyEffect (PlayerController target)
    {
        target.spd*=2;
    }
}
