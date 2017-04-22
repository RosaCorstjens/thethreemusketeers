using UnityEngine;
using System.Collections;

public class PlayerMovement : Movement
{
    private void ToggleBattle(bool inBattle)
    {
        anim.SetBool("InBattle", inBattle);
    }

    private void ToggleSword(bool equipped)
    {
        anim.SetBool("Sword", equipped);
    }

    private void ToggleStaff(bool equipped)
    {
        anim.SetBool("Staff", equipped);
    }

    private void ToggleBow(bool equipped)
    {
        anim.SetBool("Bow", equipped);
    }
}
