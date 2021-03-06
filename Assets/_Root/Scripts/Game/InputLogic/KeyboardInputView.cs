using JoostenProductions;
using Tool;
using UnityEngine;
namespace Game.InputLogic
{
    internal class KeyboardInputView : BaseInputView
    {
        private const string Horizontal = nameof(Horizontal);
        [SerializeField] private float _inputMultiplier = 10;

        public override void Init(SubscriptionProperty<float> leftMove, SubscriptionProperty<float> rightMove, float speed)
        {
            base.Init(leftMove, rightMove, speed);
        }

        private void Start() =>
            UpdateManager.SubscribeToUpdate(Move);

        private void OnDestroy() =>
            UpdateManager.UnsubscribeFromUpdate(Move);


        private void Move()
        {
            float axisOffset = Input.GetAxis(Horizontal);
            float moveValue = _inputMultiplier * Time.deltaTime * axisOffset;

            float abs = Mathf.Abs(moveValue);
            float sign = Mathf.Sign(moveValue);

            if (sign > 0)
                OnRightMove(abs);
            else if (sign < 0)
                OnLeftMove(abs);
        }
    }
}