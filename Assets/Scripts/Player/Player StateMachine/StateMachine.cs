using UnityEngine;
using System;
public class StateMachine
{
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }


    // reference to the state objects
    public RunState runState;
    public JumpState jumpState;
    public IdleState idleState;


    // event to notify other objects of the state change
    public event Action<IState> stateChanged;


    // pass in necessary parameters into constructor 
    public StateMachine(PlayerController player)
    {
        // create an instance for each state and pass in PlayerController
        this.runState = new RunState(player);
        this.jumpState = new JumpState(player);
        this.idleState = new IdleState(player);
    }


    // set the starting state
    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();


        // notify other objects that state has changed
        stateChanged?.Invoke(state);
    }


    // exit this state and enter another
    public void ChangeState(IState nextState)
    {
        CurrentState.Exit();
        PreviousState = CurrentState;
        CurrentState = nextState;
        nextState.Enter();


        // notify other objects that state has changed
        stateChanged?.Invoke(nextState);
    }


    // allow the StateMachine to update this state
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}