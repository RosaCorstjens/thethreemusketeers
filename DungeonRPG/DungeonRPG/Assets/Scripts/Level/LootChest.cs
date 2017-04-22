using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootChest : MonoBehaviour 
{
    TriggerArea trigger;

    int minItems = 50;
    int maxItems = 60;
    int amount;

    List<GameObject> items;

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

        amount = Random.Range(minItems, maxItems + 1);

        items = new List<GameObject>();

        GameManager.Instance.ItemManager.ItemGenerator.GenerateRandomItem(amount).HandleAction(i => items.Add(i.gameObject));

        anim = GetComponentInChildren<Animator>();

        Renderer[] tempArrayRender = GetComponentsInChildren<Renderer>();
        List<Renderer> tempListRender = new List<Renderer>(tempArrayRender);
        materials = new List<Material>();

        tempListRender.HandleAction(r => materials.AddRange(r.materials));
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

        materials.HandleAction(m => m.shader = Shader.Find("Legacy Shaders/Bumped Diffuse"));

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

        Open();

        yield break;
    }

    private void Open()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(true);
            items[i].gameObject.transform.position = RandomDropPosition();
            items[i].GetComponent<ItemInstance>().Drop();
        }

        items.HandleAction(i => i.gameObject.SetActive(true));

        items.HandleAction(i => i.gameObject.transform.position = RandomDropPosition());
    }

    private Vector3 RandomDropPosition()
    {
        return new Vector3(Random.Range(transform.position.x - 2, transform.position.x + 2), 0.3f, Random.Range(transform.position.z - 2, transform.position.z + 2));
    }
}
