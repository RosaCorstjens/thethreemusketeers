using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrammarHandler
{
    private List<TileGrammarRule> rules;
    private List<TileGrammarRule> currentRecipe;

    public TileGrammarHandler(string ruleFile)
    {
        ReadRules(ruleFile);

        SetRecipe(CreateTestRules());

        int endN = 10;
        int endE = 20;
        int endS = 30; 
        int endW = 40;
        int chosenCoord = 29;

        Orientation tempOrientation = chosenCoord > endN ?
            (chosenCoord > endE ?
                (chosenCoord > endS ?
                Orientation.West : Orientation.South)
            : Orientation.East) : Orientation.North;

        Debug.Log(tempOrientation);
    }

    private List<TileGrammarRule> CreateTestRules()
    {
        List<TileGrammarRule> returnList = new List<TileGrammarRule>();

        //temporary: test recipe
        char[,] lhsGridOne = 
        {
            {'u', 'u', 'u', 'u', 'u'},
            {'u', 'u', 'u', 'u', 'u'},
            {'u', 'u', 'u', 'u', 'u'},
            {'u', 'u', 'u', 'u', 'u'},
            {'u', 'u', 'u', 'u', 'u'}
        };

        char[,] rhsGridOne =
        {
            {'u', 'u', 'h', 'u', 'u'},
            {'u', 'r', 'r', 'r', 'u'},
            {'h', 'r', 'r', 'r', 'h'},
            {'u', 'r', 'r', 'r', 'u'},
            {'u', 'u', 'e', 'u', 'u'}
        };

        char[,] rhsGridTwo =
        {
            {'u', 'u', 'h', 'u', 'u'},
            {'u', 'r', 'r', 'r', 'u'},
            {'h', 'r', 'r', 'r', 'h'},
            {'u', 'r', 'r', 'r', 'u'},
            {'u', 'u', 'e', 'u', 'u'}
        };

        char[,] rhsGridThree =
        {
            {'u', 'u', 'h', 'u', 'u'},
            {'u', 'r', 'r', 'r', 'u'},
            {'h', 'r', 'r', 'r', 'h'},
            {'u', 'r', 'r', 'r', 'u'},
            {'u', 'u', 'e', 'u', 'u'}
        };

        Grid lhsOne = Grid.CreateGrid(lhsGridOne);
        Grid rhsOne = Grid.CreateGrid(rhsGridOne);
        Grid rhsTwo = Grid.CreateGrid(rhsGridTwo);
        Grid rhsThree = Grid.CreateGrid(rhsGridThree);
        List<Grid> rhs = new List<Grid>();
        rhs.Add(rhsOne);
        rhs.Add(rhsTwo);
        rhs.Add(rhsThree);

        List<int> probs = new List<int>();
        probs.Add(101);
        probs.Add(100);
        probs.Add(150);

        TileGrammarRule ruleOne = new TileGrammarRule(lhsOne, rhs, probs);

        returnList.Add(ruleOne);

        return returnList;
    }

    private void ReadRules(string ruleFile)
    {
        
    }

    public void SetRecipe(List<TileGrammarRule> newRecipe)
    {
        // maybe tests are needed

        currentRecipe = newRecipe;
    }

    public GrammarGrid ApplyRecipe(GrammarGrid grid)
    {
        //TODO: loop through recipe and apply
        for (int i = 0; i < currentRecipe.Count; i++)
        {
            grid = ApplyRule(currentRecipe[i], grid);
        }

        return grid;
    }

    private GrammarGrid ApplyRule(TileGrammarRule rule, GrammarGrid grid)
    {
        // add all non-rotated options
        List<Coordinate> possCoordinates = new List<Coordinate>();
        int endN, endE, endS, endW = 0;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.North)));
        endN = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.East)));
        endE = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.South)));
        endS = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.West)));
        endW = possCoordinates.Count - 1;

        // check for no possible coordinates
        if (possCoordinates.Count == 0)
        {
            Debug.LogError("This rule isn't contained within specified grid!");
            return grid;
        }
        else
        {
            Debug.Log("I found " + possCoordinates.Count + "options!");
        }

        int chosenCoord = UnityEngine.Random.Range(0, possCoordinates.Count);

        // random roll for RHS probabilities
        int summedProb = 0;
        rule.ProbabilitiesRHS.HandleAction(r => summedProb += r);

        int chosenProb = UnityEngine.Random.Range(0, summedProb);
        int chosenRHS = 0;
            
        summedProb = 0;
        for (int i = 0; i < rule.ProbabilitiesRHS.Count; i++)
        {
            summedProb += rule.ProbabilitiesRHS[i];

            // found the chosen one
            if (chosenProb < summedProb)
            {
                chosenRHS = i;
                break;
            }
        }

        Orientation tempOrientation = chosenCoord > endN ?
            (chosenCoord > endE ?
            (chosenCoord > endS ?
            Orientation.West : Orientation.South)
            : Orientation.East) : Orientation.North;

        grid.SetTiles(possCoordinates[chosenCoord], Grid.RotateGrid(rule.RHS[chosenRHS], 
            tempOrientation));

        Debug.Log("Orientation: " + tempOrientation);
        Debug.Log("ChosenRHS: " + chosenRHS);

        return grid;
    }
}
