using UnityEngine;
using System.Collections;

public class HUDManager
{
    private GameObject hud;
    public GameObject HUD { get { return hud; } }

    private HUDBar healthBar;
    public HUDBar HealthBar { get { return healthBar; } }

    private HUDBar manaBar;
    public HUDBar ManaBar { get { return manaBar; } }

    public void Initialize()
    {
        hud = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InGameUI/HUD"));
        hud.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        hud.transform.localPosition = Vector3.zero;
        hud.transform.localScale = Vector3.one;

        healthBar = hud.transform.FindChild("HealthBarContainer").GetComponent<HUDBar>();
        manaBar = hud.transform.FindChild("ManaBarContainer").GetComponent<HUDBar>();

        healthBar.Initialize();
        manaBar.Initialize();
    }
}