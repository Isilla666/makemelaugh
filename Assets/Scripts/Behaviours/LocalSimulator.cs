using System.Linq;
using Backend.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviours
{
    public class LocalSimulator : MonoBehaviour
    {
        public ActionEvent.ActionData[] actions;
        public KeyCode keyCode;
        private int count;

        public UnityEvent<ActionEvent.ActionData> onActionData;


        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
                IncrementCount();
        }

        private void IncrementCount()
        {
            count += Random.Range(1, 15);

            foreach (var act in actions)
            {
                if (count >= act.damage && count % act.damage == 0)
                    onActionData?.Invoke(act);
            }


            if (count >= actions.Last().damage)
                count = 0;
        }
    }
}