using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    //owner of the state machine
    T owner;

    //state properties
    private State<T> startState;
    private State<T> previous;
    private State<T> current;
    public State<T> Current
    {
        get
        {
            return current;
        }
    }
    public Dictionary<string, State<T>> PossibleStates;

    public StateMachine(T owner, State<T> startState, Dictionary<string, State<T>> possibleStates)
    {
        //Set the owner
        this.owner = owner;

        //intialize the current state to be null
        current = null;

        //fill the dictionary with the possible states
        PossibleStates = possibleStates;

        this.startState = startState;
    }

    public void Init()
    {
        //Set the startState
        SetState(startState);
    }

    public void Update()
    {
        if (Current != null)
        {
            Current.Execute(owner);
        }
    }

    public void SetState(State<T> newState)
    {
        previous = current;
        if (current != null)
        {
            current.Exit(owner);
        }
        current = newState;
        current.Enter(owner);
    }

    public void GoBackState()
    {
        SetState(previous);
    }

}
