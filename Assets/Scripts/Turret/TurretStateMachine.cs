using UnityEngine;

public class TurretStateMachine : StateMachineBehaviour
{
    public string boolName;          // Animator의 bool 변수의 이름을 저장할 변수
    public bool updateOnState;       // 상태 진입 및 종료 시 Animator의 bool 변수를 업데이트할지 여부를 결정하는 플래그
    public bool updateOnStateMachine; // 상태 머신 진입 및 종료 시 Animator의 bool 변수를 업데이트할지 여부를 결정하는 플래그
    public bool valueOnEnter;        // 상태 진입 시 설정할 Animator의 bool 변수 값
    public bool valueOnExit;         // 상태 종료 시 설정할 Animator의 bool 변수 값

    // OnStateEnter는 해당 상태에 진입할 때 호출됩니다.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //turret.anim.SetBool(animBoolName, true);
        if (updateOnState)
        {
            // updateOnState 플래그가 true이면 boolName으로 지정된 Animator의 bool 변수를 valueOnEnter 값으로 설정합니다.
            animator.SetBool(boolName, valueOnEnter);
        }
    }

    // 상태가 업데이트될 때 호출됩니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit는 해당 상태에서 빠져나갈 때 호출됩니다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            // updateOnState 플래그가 true이면 boolName으로 지정된 Animator의 bool 변수를 valueOnExit 값으로 설정합니다.
            animator.SetBool(boolName, valueOnExit);
        }
    }

}