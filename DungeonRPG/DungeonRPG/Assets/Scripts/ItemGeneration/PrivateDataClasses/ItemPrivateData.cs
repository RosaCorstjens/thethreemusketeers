using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrivateData  {

    private BaseItem itemInfo;
    public BaseItem ItemInfo { get { return itemInfo; } }

    private bool dropped;
    public bool Dropped { get { return dropped; } set { dropped = value; } }

    private Quality quality;
    public Quality Quality { get { return quality; } }

}
