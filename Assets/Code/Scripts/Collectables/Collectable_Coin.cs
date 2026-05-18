using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Coin : Collectable
{
    public override void ApllyEffect (PlayerController target)
    {
        target.GainLife(10);
    }
}
