using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileGrammarHandler
{
    private RecipeCreator rCreator;
    private List<RoomRuleProxy> roomList;
    private List<TileGrammarRule> rules;
    private List<TileGrammarRule> currentRecipe;
    private GrammarGrid grid;

    public TileGrammarHandler()
    {
        rCreator = new RecipeCreator();
        roomList = rCreator.getRoomList();

        SetRecipe();
        grid = new GrammarGrid(40, 40);
        ApplyRecipe();
    }

    public void Print(Text output)
    {
        grid.PrintGrid(output);
    }

    private void SetRecipe()
    {
        List<TileGrammarRule> rules = new List<TileGrammarRule>();
        for (int i = 0; i < roomList.Count; i++)
        {
            for (int j = 0; j < roomList[i].AmountOfRules; j++)
            {
                rules.Add(roomList[i].GetRule(j));
            }
        }

        currentRecipe = rules;
    }

    public GrammarGrid ApplyRecipe()
    {
        //TODO: loop through recipe and apply
        for (int i = 0; i < currentRecipe.Count; i++)
        {
            if (currentRecipe[i].ExecuteRule)
            {
                Debug.Log("Executing...");
                ApplyRule(currentRecipe[i]);
                while (ApplyRule(currentRecipe[i]))
                {
                    
                }
            }
            else
            {
                if (ApplyRule(currentRecipe[i]))
                {
                    Debug.Log(currentRecipe[i].Name + ": Succes");
                }
                else
                {
                    Debug.Log(currentRecipe[i].Name + ": Succes");
                }
            }
            
        }

        return grid;
    }

    private bool ApplyRule(TileGrammarRule rule)
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
            return false;
        }
        else
        {
            Debug.Log("I found " + possCoordinates.Count + "options!");
        }

        int chosenCoord = UnityEngine.Random.Range(0, possCoordinates.Count);

        // random roll for RHS probabilities
        float summedProb = 0.0f;
        rule.ProbabilitiesRHS.HandleAction(r => summedProb += r);

        float chosenProb = UnityEngine.Random.Range(0, summedProb);
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

        return true;
    }
}
