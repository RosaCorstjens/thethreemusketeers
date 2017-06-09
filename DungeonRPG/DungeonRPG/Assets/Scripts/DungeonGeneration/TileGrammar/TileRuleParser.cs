using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class TileRuleParser
{
    private string filePath = "Assets/Resources/LudoScope/TileRules/TileRules.grm";

    public TileRuleParser(string filePath = "")
    {
        if (filePath != "") this.filePath = filePath;
        Debug.Log("Parsing Tile rules, using file:" + filePath);
    }

    public List<string> ReadFile()
    {
        Debug.Log("Reading file...");
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

        while (!reader.EndOfStream)
        {
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
                    createRule(line);
                }
                line = reader.ReadLine();
            }
            else
            {
                line = reader.ReadLine();
            }
            
        }
        
        reader.Close();
        Debug.Log("Reading done, parsing content...");

        return content;
    }
    //gt1 = rotate
    //gt2 = hori
    //gt4 = verti
    
    public void CreateStartRule(string input)
    {
        string[,] grid = getTileMap(input);
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        TileGrammarRuleProxy proxy = new TileGrammarRuleProxy("start");
        proxy.width = width;
        proxy.height = height;
        proxy.Setup();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                proxy.SetTile(x,y, grid[x,y]);
            }
        }
    }

    public void createRule(string input)
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
        proxy.Setup();

        lhs = lhs.Substring(lhs.IndexOf("TILEMAP"));
        string[,] grid = getTileMap(lhs);
        //read all tiles and create grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                proxy.SetTile(x, y, grid[x, y]);
            }
        }

        List<TileGrammarRuleProxy> rhsProxies = new List<TileGrammarRuleProxy>();
        //B part:
        int ruleCounter = 0;
        while (rhs != "" || rhs.Length > 0)
        {
            rhsProxies.Add(new TileGrammarRuleProxy("unnamed-rhs-rule"));
            //extract everything between '{' and '}'
            string rule = rhs.Substring(rhs.IndexOf('{'), rhs.IndexOf('}') - rhs.IndexOf('{'));
            rhs = rhs.Substring(rhs.IndexOf('}') + 1);
            int nameLength = rule.IndexOf('=') - 1;
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
                            rhsProxies[ruleCounter].probability = float.Parse(parameter[1]);
                            break;
                        default:
                            Debug.Log("new type of parameter:" + parameter[0]);
                            break;
                    }
                }
                rhsProxies[ruleCounter].ruleName = rule.Substring(rule.IndexOf('{') + 1, nameLength);
                rule = rule.Substring(rule.IndexOf(')') + 1);
                if (rule.Length > 3)
                {
                    rule = rule.Substring(3);
                }
            }
            else
            {
                rhsProxies[ruleCounter].ruleName = rule.Substring(rule.IndexOf('{') + 1, nameLength - 1);
                //skip = and space characters after name
                if (rule.Length > 3)
                {
                    rule = rule.Substring(nameLength + 3);
                }
            }

            if (rule.Contains('>'))
            {
                proxy.executeRule = true;
                rule = rule.Split('>')[0];
            }
            string[,] rhsGrid = getTileMap(rule);
            rhsProxies[ruleCounter].width = rhsGrid.GetLength(0);
            rhsProxies[ruleCounter].height = rhsGrid.GetLength(1);
            rhsProxies[ruleCounter].Setup();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    rhsProxies[ruleCounter].SetTile(x, y, rhsGrid[x,y]);
                }
            }
            ruleCounter++;
        }
    }

    public string[,] getTileMap(string input)
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

        string[,] gridArray = new string[width, height];

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
                gridArray[x, y] = tile[1];
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