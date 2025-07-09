using UnityEngine;

public class IdleState : GuideState
{
    public IdleState(GuideController controller)
    {
        Controller = controller;
    }

    public override void Enter()
    {
        // Можно добавить анимацию простоя
    }

    public override void Update()
    {
        // Ничего не делаем
    }

    public override void Exit()
    {
        // Завершение состояния простоя
    }
}