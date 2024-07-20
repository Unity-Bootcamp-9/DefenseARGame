public class TurretStateMachine
{
    public TurretState currentState { get; private set; }
    public TurretDetectionState detectionState;

    public void Init(TurretState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }
    public void ChangeState(TurretState nextState)
    {
        currentState.Exit();
        currentState = nextState;
        nextState.Enter();
    }
/*    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }*/
}