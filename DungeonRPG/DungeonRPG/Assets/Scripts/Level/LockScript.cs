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

        // handles multiple keys if there are multiple lock anims
        foreach (var child in children)
        {
            if (child.name.Contains("lock"))
            {
                MeshRenderer[] meshes = child.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshes)
                {
                    mesh.material.color = doorColor;
                    /*mesh.material.EnableKeyword("_EMISSION");
                    mesh.material.SetColor("_EMISSION", new Color(0.125f, 0.125f, 0.125f));*/
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
