﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrivateData  {

    public ItemPrivateData(bool dropped, Quality quality)
    {
        this.dropped = dropped;
        this.quality = quality;
    }

    private bool dropped;
    public bool Dropped { get { return dropped; } set { dropped = value; } }

    private Quality quality;
    public Quality Quality { get { return quality; } }
}
