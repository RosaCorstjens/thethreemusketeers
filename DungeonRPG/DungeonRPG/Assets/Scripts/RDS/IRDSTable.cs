using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IRDSTable : IRDSObject
{
    int rdsCount { get; set; }              // How many items shall drop from this table?
    List<IRDSObject> rdsContents { get; }   // The contents of the table. 
    List<IRDSObject> rdsResult { get; }     // The result set. 
}
