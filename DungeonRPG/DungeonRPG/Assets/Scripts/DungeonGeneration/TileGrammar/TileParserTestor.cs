using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileParserTestor : MonoBehaviour
{
    private TileRuleParser parser;
    public void Fire()
    {
        parser = new TileRuleParser();
        parser.ReadFile();
    }
}
