public abstract class BaseState
{
    // Reference to our state machine.
    public StateMachine owner;

    public virtual void InitState() { }

    public virtual void UpdateState() { }

    public virtual void DestroyState() { }
}