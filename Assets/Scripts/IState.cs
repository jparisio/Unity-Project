using UnityEngine;

public interface IState
{
    void Enter();       // Called when entering the state
    void Update();     // Called every frame the state is active
    void Exit();        // Called when exiting the state
}

