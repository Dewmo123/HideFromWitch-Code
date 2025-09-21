using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KHG.Managers
{
    public class SceneLoadController : MonoBehaviour
    {
        static string nextSceneName;

        [SerializeField] private TextMeshProUGUI loadingText;

        public static async Task LoadScene(string sceneName)
        {
            nextSceneName = sceneName;
            await SceneManager.LoadSceneAsync("SceneChange", LoadSceneMode.Single);
            //��Ŷ �ޱ�
            
        }


        private void OnEnable()
        {
            if (string.IsNullOrEmpty(nextSceneName)) return;

            StartCoroutine(LoadSceneProcess());
        }


        private IEnumerator LoadSceneProcess()
        {
            loadingText.text = $"{0}:%";
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
            operation.allowSceneActivation = false;

            float time = 0f;

            float loadingPercent = 0f;
            while (true)
            {
                yield return null;
                loadingText.text = $"{loadingPercent:F0}%";

                if (operation.progress < 0.9f)
                {
                    loadingPercent = operation.progress * 100f;
                }
                else
                {
                    time += Time.unscaledDeltaTime;
                    loadingPercent = 98f;
                    if (time >= 1f)
                    {
                        operation.allowSceneActivation = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
            loadingText.text = $"Loading... {100}%";
        }
    }
}