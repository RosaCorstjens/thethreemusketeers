using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        //skip version line
        string line = reader.ReadLine();
        string type;

        if (line.Contains("version"))
        {
            line = reader.ReadLine();
        }
        type = line.Substring(0, raw.IndexOf(' ') - 1);
        line = line.Substring(type.Length);

        if (type.Contains("start"))
        {
            CreateStartRule(line);
        }
        else if (type.Contains("rule"))
        {
            //rule
            //split on < sign in part A & B
            //A:
            //save name
            //between '(' and ')' sign
            //split on ',' or space and gather all elements
            //save data in correct var
            //find TILEMAP
            //read width
            //read depth
            //read all tiles and create grid

            //B:
            //extract everything between '{' and '}'
            //for each element
            //save id
            //save probablility
            //sace width
            //save height
            //extract and save tile positions
        }

        reader.Close();
        Debug.Log("Reading done, parsing content...");

        return content;
    }

    public void CreateStartRule(string input)
    {
        //start
        //remove TILEMAP
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
        if (width == 0 || height == 0)
        {
            Debug.Log("wrong width/ height input");
        }
        RuleProxy proxy = new RuleProxy(width, height, "start");
        //save grid with appropriate tiles
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
            y = (int) id / width;
            proxy.SetTile(x, y, tile[1]);
        }
        //make rule

    }
}

public struct RuleProxy
{
    public string name;
    public int width;
    private int height;
    private string[,] grid;

    public RuleProxy(int width, int height, string name)
    {
        this.name = name;
        this.width = width;
        this.height = height;

        grid = new string[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = "undefined";
            }
        }
    }

    public string GetTile(int x, int y)
    {
        if (x > 0 && y > 0 && x < width && y < height)
        {
            return grid[x, y];
        }
        else
        {
            return "Error!";
        }
    }

    public void SetTile(int x, int y, string value)
    {
        grid[x, y] = value;
    }
}