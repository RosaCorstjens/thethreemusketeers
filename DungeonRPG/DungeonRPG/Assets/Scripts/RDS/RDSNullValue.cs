using UnityEngine;
using System.Collections;
using System;

public class RDSNullValue : IRDSValue<object>
{
    public RDSNullValue(double probability)
  //: base(null, probability, false, false, true)
    { }

    public bool rdsAlways
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool rdsEnabled
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public double rdsProbability
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool rdsUnique
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public object rdsValue
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public event EventHandler rdsHit;
    public event EventHandler rdsPostResultEvaluation;
    public event EventHandler rdsPreResultEvaluation;

    public void OnRDSHit(EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnRDSPostResultEvaluation(EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnRDSPreResultEvaluation(EventArgs e)
    {
        throw new NotImplementedException();
    }
}
