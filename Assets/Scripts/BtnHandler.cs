using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnHandler : MonoBehaviour
{
    [SerializeField] ForestBehaviour _forestBehaviour;
    [SerializeField] PlayerController _playerController;
    [SerializeField] CameraController _cameraController;
   public void HandleButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "Button_L":
                _forestBehaviour.LookAtPreviousTree();
                break;
            case "Button_R":
                _forestBehaviour.LookAtNextTree();
                break;
            case "Button_Restart":
                SceneManager.LoadScene(0);
                break;
            case "Button_Chop":
                _playerController.ShootTheBullet();
                break;
        }
    }
}
