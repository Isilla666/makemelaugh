using System.Linq;
using Backend.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviours
{
    public class LocalSimulator : MonoBehaviour
    {
        public ActionEvent.ActionData[] actions;

        public int multiply = 10;
        public KeyCode keyCode1;
        public KeyCode keyCode2;

        private int count;
        private bool left;


        public UnityEvent<ActionEvent.ActionData> onActionData;


        private void Update()
        {
            if (!IsAnyDown())
                return;

            var first = left ? keyCode1 : keyCode2;
            var second = left ? keyCode2 : keyCode1;

            left = !left;
            
            if (IsKeyUp(first, second))
                IncrementCount();
        }

        private void IncrementCount()
        {
            count += 1 * multiply;

            foreach (var act in actions)
            {
                if (count >= act.damage && count % act.damage == 0)
                    onActionData?.Invoke(act);
            }


            if (count >= actions.Last().damage)
                count = 0;
        }


        private bool IsAnyDown() => Input.GetKeyDown(keyCode1) || Input.GetKeyDown(keyCode2);

        private static bool IsKeyUp(KeyCode first, KeyCode second) => Input.GetKeyDown(first) && !Input.GetKeyDown(second);
    }
}