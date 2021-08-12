using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public float xAxis { get; private set; } = 0;
        public float yAxis { get; private set; } = 0;
        private Dictionary<string, ICanJump> _jumpComponents;

        private void Update()
        {
            xAxis = Input.GetAxis("Horizontal");
            yAxis = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Jump")) JumpCallback();
        }

        public PlayerInput RegisterJump(ICanJump obj)
        {
            if (_jumpComponents == null) _jumpComponents = new Dictionary<string, ICanJump>();

            _jumpComponents[obj.ID] = obj;

            return this;
        }

        private void JumpCallback()
        {
            foreach (KeyValuePair<string, ICanJump> cb in _jumpComponents)
            {
                if (_jumpComponents[cb.Key] == null) _jumpComponents.Remove(cb.Key);
                else cb.Value.Jump();
            }
        }
    }
}
