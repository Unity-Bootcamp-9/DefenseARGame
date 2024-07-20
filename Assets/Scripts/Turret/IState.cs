public interface IState
{
    public void Enter()
    {
        // 상태 진입할 때
    }

    public void Update()
    {
        // 프레임 당 로직
    }

    public void Exit() 
    {
        // 상태 벗어날 때
    }
}