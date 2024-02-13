using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings
{
    public class ResetAllBindings : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputAction;
        public void ResetBindings()
        {
            foreach (InputActionMap map in inputAction.actionMaps)
            {
                map.RemoveAllBindingOverrides();
            }
            PlayerPrefs.DeleteKey("rebinds");
        }
    }
}
