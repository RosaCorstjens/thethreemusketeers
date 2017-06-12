using System.Collections;
using System.Collections.Generic;

public class TileGrammarRule
{
    public string Name { get { return ruleName; } }
    public Grid LHS { get; private set; }
    public List<Grid> RHS { get; private set; }
    public List<float> ProbabilitiesRHS { get; private set; }

    private string ruleName;
    private int width;
    private int height;

    private int maxExecutions;

    private bool canRotate;
    private bool canMirrorH;
    private bool canMirrorV;
    private bool executeRule;

    public TileGrammarRule() { }

    public TileGrammarRule(string name, Grid LHS, List<Grid> RHS, List<float> probRHS = null, bool canR = false, bool canMH = false, bool canMV = false, bool execute = false)
    {
        ruleName = name;
        width = LHS.Width;
        height = LHS.Height;

        canRotate = canR;
        canMirrorH = canMH;
        canMirrorV = canMV;
        executeRule = execute;

        this.LHS = LHS;
        this.RHS = RHS;

        if (probRHS == null)
        {
            probRHS = new List<float>();
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



    public TileGrammarRule(string name, Grid LHS, Grid RHS, float probRHS = 0)
    {
        ruleName = name;
        this.LHS = LHS;
        this.RHS = new List<Grid>();
        this.RHS.Add(RHS);

        this.ProbabilitiesRHS = new List<float>(); 
        this.ProbabilitiesRHS.Add(probRHS <= 0.0f ? 1.0f : probRHS);
    }

    Grid GetRandomRHS()
    {
        //TODO: do we have to set a seed?
        return RHS[UnityEngine.Random.Range(0, RHS.Count)];
    }
}
