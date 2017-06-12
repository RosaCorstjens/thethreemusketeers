using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTest : MonoBehaviour
{
    private GrammarGrid grid;
    private TileGrammarHandler handler;
    private int i = 0;

    public Text txt;

	// Use this for initialization
	void Start ()
	{
        handler = new TileGrammarHandler();
	}

    public void Print()
    {
        handler.Print(txt);
    }
}
