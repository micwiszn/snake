using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState currentState;


    private void Update()
    {
        // Update current state
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }


    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.DestroyState();
        }
        //switch the state to new one
        currentState = newState;

        //check if we're passing null
        if (currentState != null)
        {
            currentState.owner = this;
            currentState.InitState();
        }
    }
}