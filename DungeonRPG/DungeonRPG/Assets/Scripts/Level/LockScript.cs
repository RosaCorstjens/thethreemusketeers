using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    [SerializeField] private KeyType type;

    TriggerArea trigger;

    private Transform targetTransform;
    private PlayerController targetScript;

    Coroutine checkForKey;
    Coroutine unlock;

    List<Material> materials;

    int lockId;

    public void Initialize(int id)
    {
        lockId = id;

        trigger = GetComponent<TriggerArea>();
        
        Animator[] children = transform.GetComponentsInChildren<Animator>();
        materials = new List<Material>();

        Color doorColor = GetColor(lockId);
        type = GetType(id);

        // handles multiple keys if there are multiple lock anims
        foreach (var child in children)
        {
            if (child.name.Contains("lock"))
            {
                MeshRenderer[] meshes = child.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshes)
                {
                    mesh.material.color = doorColor;
                }

                Renderer[] tempArrayRender = child.GetComponentsInChildren<Renderer>();
                List<Renderer> tempListRender = new List<Renderer>(tempArrayRender);
                tempListRender.HandleAction(r => materials.AddRange(r.materials));
            }
        }

        trigger.onTriggerAction = PlayerInRange;
        trigger.offTriggerAction = PlayerOutOfRange;

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();
    }

    public static Color GetColor(int id)
    {
        switch (id)
        {
            // normal keys/locks
            case 0:
                return Color.green;
                break;

            // multi keys/locks
            case 1:
                return Color.yellow;
                break;

            // final keys/locks
            case 2:
                return Color.red;
                break;

            default:
                return Color.black;
                break;
        }
    }

    public static KeyType GetType(int id)
    {
        switch (id)
        {
            // normal keys/locks
            case 0:
                return KeyType.Normal;
                break;

            // multi keys/locks
            case 1:
                return KeyType.Multi;
                break;

            // final keys/locks
            case 2:
                return KeyType.Final;
                break;

            default:
                return KeyType.Normal;
                break;
        }
    }

    public void PlayerInRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.ShowPressToOpen(this.gameObject);
        materials.HandleAction(m => m.shader = Shader.Find("Legacy Shaders/Self-Illumin/Bumped Diffuse"));

        checkForKey = StartCoroutine(CheckKeyPressed());
    }

    public void PlayerOutOfRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.HideLabel();
        materials.HandleAction(m => m.shader = Shader.Find("Legacy Shaders/Bumped Diffuse"));

        if (checkForKey != null) StopCoroutine(checkForKey);
    }

    public IEnumerator CheckKeyPressed()
    {
        while (true)
        {
            while (!Input.GetKey(KeyCode.E))
            {
                yield return null;
            }
            if (targetScript.HasKey(lockId, type))
            {
                GameManager.Instance.UIManager.WorldUIManager.HideLabel();
                if (unlock == null)
                {
                    unlock = StartCoroutine(Unlock());
                }
                yield return new WaitForSeconds(2f);
            }
            else
            {
                GameManager.Instance.UIManager.YouDoNotHaveAKeyWarning();
            }
            yield return null;
        }
    }

    public IEnumerator Unlock()
    {
        Animator[] anim = transform.GetComponentsInChildren<Animator>();
        List<Animator> activeAnimations = new List<Animator>();
        foreach (var animator in anim)
        {
            if (animator.name.Contains("lock") && activeAnimations.Count < 2)
            {
                animator.SetTrigger("Unlock");
                activeAnimations.Add(animator);
            }
        }
        yield return new WaitForSeconds(2f);

        foreach (var animator in activeAnimations)
        {
            animator.gameObject.SetActive(false);
        }

        anim = transform.GetComponentsInChildren<Animator>();
        Animator myAnim = transform.GetComponent<Animator>();
        if (myAnim != null && anim.Length == 1){
            myAnim.SetTrigger("Unlock");
            Debug.Log("Unlocking");
            yield return new WaitForSeconds(4f);
            GameManager.Instance.UIManager.WorldUIManager.HideLabel();
            trigger.gameObject.SetActive(false);
        }

        unlock = null;

        yield return null;
    }
}
