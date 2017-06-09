using System.Collections;
using System.Collections.Generic;

public class TileGrammarRule
{
    public Grid LHS { get; private set; }
    public List<Grid> RHS { get; private set; }
    public List<int> ProbabilitiesRHS { get; private set; }

    private string ruleName;
    private int width;
    private int height;

    private int maxExecutions;

    private bool canRotate;
    private bool canMirrorH;
    private bool canMirrorV;

    public TileGrammarRule() { }

    public TileGrammarRule(Grid LHS, List<Grid> RHS, List<int> probRHS = null)
    {
        this.LHS = LHS;
        this.RHS = RHS;

        if (probRHS == null)
        {
            probRHS = new List<int>();
            for (int i = 0; i < RHS.Count; i++)
            {
                probRHS.Add(1);
            }
        }
        else
        {
            for (int i = 0; i < probRHS.Count; i++)
            {
                if (probRHS[i] < 1) probRHS[i] = 1;
            }
        }

        this.ProbabilitiesRHS = probRHS;
    }

    public TileGrammarRule(Grid LHS, Grid RHS, int probRHS = 0)
    {
        this.LHS = LHS;
        this.RHS = new List<Grid>();
        this.RHS.Add(RHS);

        this.ProbabilitiesRHS = new List<int>(); 
        this.ProbabilitiesRHS.Add(probRHS < 1 ? 1 : probRHS);
    }

    Grid GetRandomRHS()
    {
        //TODO: do we have to set a seed?
        return RHS[UnityEngine.Random.Range(0, RHS.Count)];
    }
}
