using UnityEngine;
using System.Collections;

public class BaseHunterClass : BaseCharacterClass
{
    public BaseHunterClass()
    {
        characterClassName = "Hunter";
        characterClassDescription = "My family was butchered. To witness such a thing is enought to leave your mind in ruins. The madness left me frightened and alone. Alone, until I was resued by great masters that forged me into a weapon. Now, I am ready. \n \n A stealthy warrior with a focus mainly on ranged combat. ";
        primaryStat = StatTypes.Dexterity;
        characterClassType = CharacterClass.Hunter;
    }
}
