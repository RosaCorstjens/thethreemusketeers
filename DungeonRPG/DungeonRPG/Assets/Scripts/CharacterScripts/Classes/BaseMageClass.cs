using UnityEngine;
using System.Collections;

public class BaseMageClass : BaseCharacterClass
{
    public BaseMageClass()
    {
        characterClassName = "Mage";
        characterClassDescription = "The powers I yield are not from this world. My loved ones abonded me in fear for my magic. I learned to walk my path alone, away from simple people who like to judge. I found peace. In peace I found power. \n \n Manipulating the primal forces of the world, the mage is not afraid to destory all in the path of victory.";
        primaryStat = StatTypes.Intelligence;
        characterClassType = CharacterClass.Mage;
    }
}
