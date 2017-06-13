using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileGrammarHandler
{
    private RecipeCreator rCreator;
    private List<RuleSetProxy> roomList;
    private List<TileGrammarRule> rules;
    private List<TileGrammarRule> currentRecipe;
    private Grid grid;

    public TileGrammarHandler()
    {
        rCreator = new RecipeCreator();
        roomList = rCreator.getRoomList();

        SetRecipe();

        grid = new Grid(1, 1);

        while (!ApplyRecipe())
        {
            grid = new Grid(1, 1);
        }
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

    public bool ApplyRecipe()
    {
        // loop through recipe and apply
        for (int i = 0; i < currentRecipe.Count; i++)
        {
            if (currentRecipe[i].ExecuteRule)
            {
                //Debug.Log("Executing...");
                //ApplyRule(currentRecipe[i]);
                while (ApplyRule(currentRecipe[i]))
                {
                }
                Debug.Log(currentRecipe[i].Name + ": Succes");

            }
            else
            {
                if (ApplyRule(currentRecipe[i]))
                {
                    Debug.Log(currentRecipe[i].Name + ": Succes");
                }
                else
                {
                    //TODO: change width and height and recheck
                    if (ApplyRuleSizeLess(currentRecipe[i]))
                    {

                    }
                    else
                    {
                        Debug.LogWarning("Mui problemos, los cameros is ingeslotos!");
                        Debug.LogError(currentRecipe[i].Name + ": Failed, creating new dungeon");
                        return false;
                    }

                    //if still not solved:
                    //  if room is connected - find previous

                    
                    //return grid;
                }
            }
        }

        Debug.LogError("Succeeded creating dungeon");
        return true;
    }

    private bool ApplyRuleSizeLess(TileGrammarRule rule)
    {
        // add all non-rotated options
        List<Coordinate> possCoordinates = new List<Coordinate>();
        int endN, endE, endS = 0;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.North), true));
        endN = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.East), true));
        endE = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.South), true));
        endS = possCoordinates.Count - 1;
        possCoordinates.AddRange(grid.Contains(Grid.RotateGrid(rule.LHS, Orientation.West), true));

        // check for no possible coordinates
        if (possCoordinates.Count == 0)
        {
            Debug.Log("Rescaling grid did not work.");
            return false;
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

        //rescalen
        //TODO:
        Coordinate originTranslation = new Coordinate(0,0);
        Coordinate scalar = new Coordinate(0,0);
        int ruleW = Grid.RotateGrid(rule.RHS[chosenRHS], tempOrientation).Width;
        int ruleH = Grid.RotateGrid(rule.RHS[chosenRHS], tempOrientation).Height;

        if (possCoordinates[chosenCoord].x < 0)
        {
            //x smaller
            originTranslation.x = ruleW;
            scalar.x += ruleW;
        }
        if (possCoordinates[chosenCoord].y < 0)
        {
            //y smaller
            originTranslation.y = ruleH;
            scalar.y += ruleH;
        }
        if (possCoordinates[chosenCoord].x > grid.Width - ruleW)
        {
            //x bigger
            scalar.x += ruleW;
        }
        if (possCoordinates[chosenCoord].y > grid.Height - ruleH)
        {
            //y bigger
            scalar.y += ruleH;
        }
        //1. new grid aanmaken
        Grid newGrid = new Grid(grid.Width + scalar.x, grid.Height + scalar.y);
        //3. oude grid invullen
        newGrid.SetTiles(originTranslation, grid);
        
        //4. oude grid replacen
        grid = new Grid(newGrid.Width, newGrid.Height);
        grid = newGrid;

        Coordinate newpos = possCoordinates[chosenCoord];
        newpos.x += originTranslation.x;
        newpos.y += originTranslation.y;

        Grid henry = Grid.RotateGrid(rule.RHS[chosenRHS], tempOrientation);
        grid.SetTiles(newpos, henry);

        Debug.LogWarning("Resize: " + scalar.x + " - " + scalar.y);
        // Debug.Log("ChosenRHS: " + chosenRHS);

        return true;
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
            Debug.Log("I cannot place this rule anymore.");
            return false;
        }
        else
        {
            //Debug.Log("I found " + possCoordinates.Count + " occurings of the LHS of this rule!");
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

       // Debug.Log("Orientation: " + tempOrientation);
       // Debug.Log("ChosenRHS: " + chosenRHS);

        return true;
    }
}
