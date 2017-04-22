using UnityEngine;
using System.Collections;

public class BaseWarriorClass : BaseCharacterClass
{
    public BaseWarriorClass()
    {
        characterClassName = "Warrior";
        characterClassDescription = "I tire of the empty battles I once craved. I wander, outcast, while my tribe curses the gods who abandoded us. Our home is in ruins, yet I stand firm. \n \n Brute force makes a successful return, the warrior devastates foes with mighty power, man and woman alike.";
        primaryStat = StatTypes.Strength;
        characterClassType = CharacterClass.Warrior;
    }
}
