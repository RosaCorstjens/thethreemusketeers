using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootChest : MonoBehaviour
{
    private Loottable loottable;

    TriggerArea trigger;

    bool open;

    Animator anim;
    List<Material> materials;

    Coroutine checkForOpen;

    // TO DO: make this into an initialize method, called by something like LevelManager.
    public void Initialize()
    {
        trigger = GetComponentInChildren<TriggerArea>();

        trigger.onTriggerAction = PlayerInRange;
        trigger.offTriggerAction = PlayerOutOfRange;

        anim = GetComponentInChildren<Animator>();

        Renderer[] tempArrayRender = GetComponentsInChildren<Renderer>();
        List<Renderer> tempListRender = new List<Renderer>(tempArrayRender);
        materials = new List<Material>();

        tempListRender.HandleAction(r => materials.AddRange(r.materials));

        loottable = new Loottable();
        loottable.Initialize(10, 15);
    }

    public void PlayerInRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.ShowPressToOpen(this.gameObject);

        materials.HandleAction(m => m.shader = Shader.Find("Legacy Shaders/Self-Illumin/Bumped Diffuse"));

        checkForOpen = StartCoroutine(CheckForOpening());
    }

    public void PlayerOutOfRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.HideLabel();

        materials.HandleAction(m => m.shader = Shader.Find("Standard"));

        if (checkForOpen != null) StopCoroutine(checkForOpen);
    }

    private IEnumerator CheckForOpening()
    {
        while (!Input.GetKey(KeyCode.E))
        {
            yield return null;
        }

        checkForOpen = null;

        open = true;

        anim.SetTrigger("action");

        trigger.gameObject.SetActive(false);

        GameManager.Instance.UIManager.WorldUIManager.HideLabel();

        materials.HandleAction(m => m.shader = Shader.Find("Legacy Shaders/Bumped Diffuse"));

        loottable.DropItems(transform.position + (2.5f * transform.forward));

        yield break;
    }
}
