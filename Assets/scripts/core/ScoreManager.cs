using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : GenericSingletonClass<ScoreManager>
{
    public Outpost[] outposts;

    public override void Initialize()
    {
        outposts = GameObject.FindObjectsOfType<Outpost>();
    }
}
