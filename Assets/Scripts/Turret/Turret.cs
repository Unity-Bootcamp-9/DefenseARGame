using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] public float turretHP;

    #region States
    public TurretStateMachine stateMachine { get; private set; }
    public TurretDetectionState detectionState { get; private set; }
    #endregion

    #region Animator
    public Animator anim { get; private set; }
    #endregion

    public void Awake()
    {
        stateMachine = new TurretStateMachine();
        detectionState = new TurretDetectionState(stateMachine, this, "isIdle");
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        stateMachine.Init(detectionState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}