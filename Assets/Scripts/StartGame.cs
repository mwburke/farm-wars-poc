using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void LoadBattleScene() {
        SceneManager.LoadSceneAsync("Scenes/BattleScene");
    }
}
