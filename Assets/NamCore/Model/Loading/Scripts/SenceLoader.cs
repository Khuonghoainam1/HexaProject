using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UIElements;
namespace NameCore
{
    public class SenceLoader : Singleton<SenceLoader>
    {
        [Header("UI Loading")]
        public GameObject loadingScreen;
        public UnityEngine.UI.Slider progressBar;

       
        public void LoadSence(SenceID senceID)
        {
            
            StartCoroutine(LoadSceneAsync(senceID));
        }

        private IEnumerator LoadSceneAsync(SenceID sceneName)
        {
            loadingScreen?.SetActive(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName.ToString());
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                progressBar.value = operation.progress;
                yield return null;
            }

            progressBar.value = 1f;
            yield return new WaitForSeconds(0.5f); // Đợi 1 chút cho đẹp
            operation.allowSceneActivation = true;

            loadingScreen?.SetActive(false);
        }


        public void OnDisable()
        {
            StopAllCoroutines();
        }
    }

}
