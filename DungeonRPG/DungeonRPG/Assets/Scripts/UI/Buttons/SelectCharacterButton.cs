using UnityEngine;
using System.Collections;

public class SelectCharacterButton : ButtonBase
{
    public int id;

    public void SelectCharacter()
    {
        GameManager.Instance.MainMenu.OnSelectCharacterButton(id);
    }
}
