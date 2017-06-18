using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

//parses all excisting tilegrammar rules from a file and places them in a list.
public class TileRuleParser
{
    private string filePath = "Assets/Resources/LudoScope/TileRules/TileRules.grm"; //file location
    private TileGrammarRule startRule;
    private List<TileGrammarRule> rules;

    //list of abbreviations per rulename
    public static Dictionary<string, char> abbreviation = new Dictionary<string, char>
    {
        { "undefined", 'u'},
        { "door", 'd'},
        { "room", 'r'},
        { "wall", 'w'},
        { "treasure", 't'},
        { "key", 'k'},
        { "keyfinal", 'K'},
        { "keymulti", '0'},
        { "lock", 'l'},
        { "lockfinal", 'L'},
        { "lockmulti", '1'},
        { "floor", 'f'},
        { "Hook", 'h'},
        { "DirectedHook", 'H'},
        { "monster", 'm'},
        { "trap", 'p'},
        { "entrance", 'e'},
        { "portal", 'P'},
        { "hubTile", '2'},
        { "hubDecoration", '3'}
    };

    //constructs a tile parser (Wil also read the file already)
    public TileRuleParser(string filePath = "")
    {
        if (filePath != "") this.filePath = filePath;
        Debug.Log("Parsing Tile rules, using file:" + filePath);
        rules = new List<TileGrammarRule>();
        ReadFile();
    }

    //returns a list of all tile-grammar rules
    public List<TileGrammarRule> GetTileRules()
    {
        List<TileGrammarRule> ruleList = new List<TileGrammarRule>(rules.Count + 1);
        ruleList.Add(startRule);
        for (int i = 0; i < rules.Count; i++)
        {
         ruleList.Add(rules[i]);   
        }
        return ruleList;
    }

    //returns the tile-grammar rule with the given name
    public TileGrammarRule GetRule(string name)
    {
        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].Name == name)
            {
                return rules[i];
            }
        }
        Debug.LogError("No such rule found: " + name);
        return null;
    }

    //returns the tile-grammar rule with the given abbreviation
    public TileGrammarRule GetRule(char abbreviationChar)
    {
        for (int i = 0; i < abbreviation.Count; i++)
        {
            if (abbreviation.ElementAt(i).Value == abbreviationChar)
            {
                return GetRule(abbreviation.ElementAt(i).Key);
            }
        }
        Debug.LogError("No such abbreviation found");
        return null;
    }

    //reads the file saving the data as rules
    public void ReadFile()
    {
        List<string> content = new List<string>();
        StreamReader reader = new StreamReader(filePath, Encoding.Default);
        string raw = reader.ReadToEnd();
        reader = new StreamReader(filePath, Encoding.Default);

        string line = reader.ReadLine();
        if (line.Contains("version"))
        {
            //skip version line
            line = reader.ReadLine();
        }

        while (!reader.EndOfStream || !line.IsNullOrEmpty())
        {
            int i = 0;
            if (line != "")
            {
                string type;

                type = line.Substring(0, line.IndexOf(' ') + 1);
                line = line.Substring(type.Length);

                if (type.Contains("start"))
                {
                    CreateStartRule(line);
                }
                else if (type.Contains("rule"))
                {
                    CreateRule(line);
                }
                line = reader.ReadLine();
            }
            else
            {
                line = reader.ReadLine();
            }
            
        }
        
        reader.Close();
    }

    //creates a start rule - a start rule is a rule in which no rhs exists. The lhs will define hw big the grid is and how it will be filled
    public void CreateStartRule(string input)
    {
        char[,] grid = GetTileMap(input);
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        TileGrammarRuleProxy proxy = new TileGrammarRuleProxy("start");
        proxy.width = width;
        proxy.height = height;
        proxy.SetupLHS();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                proxy.SetLHSTile(x,y, grid[x,y]);
            }
        }
        startRule = proxy.getRule();
    }

    // creates a rule with both lhs and rhs components. Saves this rule in the rule list
    public void CreateRule(string input)
    {
        string lhs = input.Substring(0, input.IndexOf('>'));
        string rhs = input.Substring(input.IndexOf('>'), input.Length - input.IndexOf('>'));
        string name;
        int width = 1;
        int height = 1;

        TileGrammarRuleProxy proxy = new TileGrammarRuleProxy("unnamed-rule");
        //has parameters
        if (lhs.Contains("("))
        {
            string[] parameterList;
            name = lhs.Substring(0, lhs.IndexOf('('));
            proxy.ruleName = name;
            parameterList = lhs.Substring(lhs.IndexOf('(') + 1, lhs.IndexOf(')') - (lhs.IndexOf('(') + 1)).Split(',');

            for (int i = 0; i < parameterList.Length; i++)
            {
                if (parameterList[i][0] == ' ')
                {
                    parameterList[i] = parameterList[i].Substring(1, parameterList[i].Length - 1);
                }
                string[] parameter = parameterList[i].Split('=');

                switch (parameter[0])
                {
                    case "step": //not used
                        break;
                    case "max":
                        proxy.maxExecutions = int.Parse(parameter[1]);
                        break;
                    case "width":
                        width = int.Parse(parameter[1]);
                        break;
                    case "height":
                        height = int.Parse(parameter[1]);
                        break;
                    case "gt":
                        int gt = int.Parse(parameter[1]);
                        if (gt - 4 >= 0) {
                            gt -= 4;
                            proxy.canMirrorV = true;
                        }
                        if (gt - 2 >= 0) {
                            gt -= 2;
                            proxy.canMirrorH = true;
                        }
                        if (gt - 1 >= 0) {
                            gt -= 1;
                            proxy.canRotate = true;
                        }
                        break;
                    default:
                        Debug.Log("new type of parameter:" + parameter[0]);
                        break;
                }
            }
        }
        else
        { // no parameters
            proxy.ruleName = lhs.Substring(0, lhs.IndexOf('=') - 1);
        }
        proxy.width = width;
        proxy.height = height;
        proxy.SetupLHS();

        lhs = lhs.Substring(lhs.IndexOf("TILEMAP"));
        char[,] grid = GetTileMap(lhs);
        //read all tiles and create grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                proxy.SetLHSTile(x, y, grid[x, y]);
            }
        }

        //B part:
        int ruleCounter = 0;
        while (rhs != "" || rhs.Length > 0)
        {
            //extract everything between '{' and '}'
            string rule = rhs.Substring(rhs.IndexOf('{'), rhs.IndexOf('}') - rhs.IndexOf('{'));
            rhs = rhs.Substring(rhs.IndexOf('}') + 1);
            int nameLength = rule.IndexOf('=') - 1;
            float prob = 1.0f;

            if (rule.Contains('>'))
            {
                proxy.executeRule = true;
                rule = rule.Split('>')[0];
            }

            if (rule.Contains('('))
            {
                nameLength = rule.IndexOf('(') - 1;
                string[] parameterlist =
                    rule.Substring(rule.IndexOf('(') + 1, rule.IndexOf(')') - (rule.IndexOf('(') + 1)).Split(',');
                for (int i = 0; i < parameterlist.Length; i++)
                {
                    string[] parameter = parameterlist[i].Split('=');

                    switch (parameter[0])
                    {
                        case "prob": //not used
                            if (parameter[1].Contains('f'))
                                parameter[1] = parameter[1].Substring(0, parameter[1].Length - 1);
                            prob = float.Parse(parameter[1]);
                            break;
                        default:
                            Debug.Log("new type of parameter:" + parameter[0]);
                            break;
                    }
                }
                rule = rule.Substring(rule.IndexOf(')') + 1);
                if (rule.Length > 3)
                {
                    rule = rule.Substring(3);
                }
            }
            else
            {
                //skip = and space characters after name
                if (rule.Length > 3)
                {
                    rule = rule.Substring(nameLength + 3);
                }
            }
            proxy.ProbabilitiesRHS.Add(prob);
            
            char[,] rhsGrid = GetTileMap(rule);
            proxy.SetupRHS();
            proxy.SetRHS(ruleCounter, rhsGrid);
            ruleCounter++;
        }
        rules.Add(proxy.getRule());
    }

    //returns the grid described in an input.
    public char[,] GetTileMap(string input)
    {
        int width;
        int height;

        if (input.Contains("TILEMAP"))
        {
            input = input.Substring(input.IndexOf(' ') + 1);
        }

        //get width and heigth
        width = int.Parse(input.Substring(0, input.IndexOf(' ')));
        input = input.Substring(input.IndexOf(' ') + 1);
        height = int.Parse(input.Substring(0, input.IndexOf(' ')));
        input = input.Substring(input.IndexOf(' ') + 1);
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("wrong width/ height input");
        }

        char[,] gridArray = new char[width, height];

        int x = 0, y = 0;
        string[] tile;
        //add right values
        while (input != "" || input.Length > 0)
        {
            int length = input.IndexOf(' ');
            if (length == -1)
            {
                length = input.Length;
                tile = input.Split(':');
                input = "";

            }
            else
            {
                tile = input.Substring(0, length).Split(':');
                input = input.Substring(input.IndexOf(' ') + 1);
            }

            int id = int.Parse(tile[0]);
            x = id % width;
            y = (int)id / width;

            if (x < width && y < height)
            {
                gridArray[x, y] = abbreviation.Get(tile[1]);
            }
            else
            {
                Debug.LogError("Too many tiles for this grid");
                break;
            }
        }
        return gridArray;
    }
}