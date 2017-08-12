using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ConsoleLog
{
    public class ConsoleController : MonoBehaviour
    {
        public static ConsoleController Instance;

        [SerializeField] public Text text;
        [SerializeField] public InputField inputField;

        private bool isShow;

        private bool IsShow
        {
            get { return isShow; }
            set
            {
                isShow = value;
                GetComponent<Animator>().SetBool("show", isShow);
                if (isShow)
                {
                    inputField.ActivateInputField();
                }
                else
                {
                    inputField.DeactivateInputField();
                }
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            text.text = "";
            inputField.onEndEdit.AddListener(OnEndEditCommand);
            DontDestroyOnLoad(gameObject);
        }

        public void LogMessage(string message)
        {
            text.text += message + "\n";
        }

        private void Update()
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
                Input.GetKeyDown(KeyCode.C))
            {
                IsShow = !IsShow;
            }
            else if (IsShow && Input.GetKeyDown(KeyCode.Escape))
            {
                IsShow = false;
            }
        }

        void OnEndEditCommand(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Log.ConsoleMessage("Run command: " + text);
                inputField.text = "";
                inputField.ActivateInputField();
            }
        }
    }
}