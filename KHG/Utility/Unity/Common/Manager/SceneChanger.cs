using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility.Unity.Common.Manager
{
    public class SceneChanger : MonoBehaviour
    {
        static string nextSceneName;

        [SerializeField] private TextMeshProUGUI loadingText;

        /// <summary>
        /// LoadingScene을 추가해야 합니다.
        /// </summary>
        /// <param name="sceneName">넘어갈 목표 씬의 이름 string</param>
        public static void LoadScene(string sceneName)
        {
            nextSceneName = sceneName;
            SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Single); //Scene추가 필요
        }


        private void OnEnable()
        {
            if (string.IsNullOrEmpty(nextSceneName))
            {
                Debug.LogError("nextSceneName is null or empty. Did you forget to call LoadScene()?");
                return;
            }

            IEnumerator loader = loadingText ? LoadSceneProcess() : LoadingSceneDefault();
            StartCoroutine(loader);
        }


        private IEnumerator LoadSceneProcess()
        {
            loadingText.text = $"Loading... {0}:%";
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
            operation.allowSceneActivation = false;

            float time = 0f;

            float loadingPercent = 0f;
            while (true)
            {
                yield return null;
                loadingText.text = $"Loading... {loadingPercent:F0}%";

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

        private IEnumerator LoadingSceneDefault()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
            yield return null;
        }
    }
}