using UnityEngine;
using System.Collections;

public class WorldUIManager
{
    // The label that appears when the player can interact with something. 
    private UILabel label;
    private Coroutine labelCoroutine;

    private const string PRESS_E_TO_OPEN = "Press 'E' to open.";
    private const string PRESS_E_TO_ENTER = "Press 'E' to go to the next floor.";

    public void Initialize()
    {
        // Get the prefab for the 'press e label', instatiate it. 
        label = Resources.Load<UILabel>("Prefabs/UI/InGameUI/PressEToOpenLabel");
        label = GameObject.Instantiate(label);
        label.transform.SetParent(GameManager.Instance.UIManager.UIRoot.transform);
        label.transform.localScale =  Vector3.one;
        label.gameObject.SetActive(false);
    }

    // Keeps centering the label on the given gameobject. 
    private IEnumerator HandlePositioningLabel(GameObject go)
    {
        Vector3 position;

        while (true)
        {
            position = Camera.main.WorldToNormalizedViewportPoint(go.transform.position);
            label.transform.position = GameManager.Instance.UIManager.UICamera.NormalizedViewportToWorldPoint(position);

            yield return null;
        }

        yield break;
    }

    public void ShowPressToOpen(GameObject go)
    {
        label.text = PRESS_E_TO_OPEN;
        label.gameObject.SetActive(true);
        labelCoroutine = GameManager.Instance.StartCoroutine(HandlePositioningLabel(go));
    }

    public void ShowPressToEnter(GameObject go)
    {
        label.text = PRESS_E_TO_ENTER;
        label.gameObject.SetActive(true);
        labelCoroutine = GameManager.Instance.StartCoroutine(HandlePositioningLabel(go));
    }

    public void HideLabel()
    {
        if (labelCoroutine != null) GameManager.Instance.StopCoroutine(labelCoroutine);

        label.gameObject.SetActive(false);
    }

}
