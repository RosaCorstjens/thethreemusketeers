using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    TriggerArea trigger;

    private Transform targetTransform;
    private PlayerController targetScript;

    Coroutine checkForKey;
    Coroutine unlock;

    int lockId;

    public void Initialize(int id)
    {
        lockId = id;

        trigger = GetComponent<TriggerArea>();
        
        Animator[] children = transform.GetComponentsInChildren<Animator>();

        Color doorColor = GetColor(lockId);


        foreach (var child in children)
        {
            if (child.name.Contains("lock"))
            {
                MeshRenderer[] meshes = child.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshes)
                {
                    mesh.material.color = doorColor;
                }
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
            case 0:
                return Color.blue;
                break;
            case 1:
                return Color.cyan;
                break;
            case 2:
                return Color.gray;
                break;
            case 3:
                return Color.green;
                break;
            case 4:
                return Color.magenta;
                break;
            case 5:
                return Color.red;
                break;
            case 6:
                return Color.white;
                break;
            case 7:
                return Color.yellow;
                break;
            default:
                return Color.black;
                break;
        }
    }

    public void PlayerInRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.ShowPressToOpen(this.gameObject);
        checkForKey = StartCoroutine(CheckKeyPressed());
    }

    public void PlayerOutOfRange()
    {
        GameManager.Instance.UIManager.WorldUIManager.HideLabel();
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
            if (targetScript.HasKey(lockId))
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
        if (anim.Length == 1)
        {
            anim[0].SetTrigger("Unlock");
            yield return new WaitForSeconds(4f);
            GameManager.Instance.UIManager.WorldUIManager.HideLabel();
            trigger.gameObject.SetActive(false);
        }

        unlock = null;

        yield return null;
    }
}
