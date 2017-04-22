using UnityEngine;
using System.Collections;

public class GoldInstance : ItemInstance
{
    [SerializeField]
    private BaseGold baseGold;
    public BaseGold BaseGold { get { return baseGold; } }

    [SerializeField]
    private int value;
    public int Value { get { return value; } set { this.value = value; } }

    public void Initialize(BaseGold itemInfo, Quality quality, int value)
    {
        base.Initialize(itemInfo, quality);

        gameObject.SetActive(false);

        this.itemInfo = itemInfo;
        baseGold = itemInfo;

        this.value = value;

        gameObject.GetComponent<Renderer>().material = GameManager.Instance.ItemManager.QualityMaterials[0];
    }
}
