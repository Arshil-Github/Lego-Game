using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameModes
    {
        builder,
        camera
    }
    public GameModes gameMode;

    public CameraSystem cameraSystem;
    public TextMeshProUGUI gamemodeText;
    public GameObject orientationPanel;

    [Header("Blocks")]
    public string destinationTag = "DropArea";
    public string negativeTag = "RedZone";
    public float BlockRotateSpeed = 20f;
    public float BlockZSpeed = 5f;

    private void Start()
    {
    }
    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            GameModes toSwitchTo = (gameMode == GameModes.camera) ? GameModes.builder : GameModes.camera;
            SwitchModes(toSwitchTo);
        }*/
    }
    public void SwitchModes(GameModes targetMode)
    {
        gameMode = targetMode;

        bool blockBuilder = true;
        string textToShow = "Builder Mode";

        switch (targetMode)
        {
            case GameModes.builder:
                cameraSystem.enabled = false;
                textToShow = "Builder Mode";
                blockBuilder = true;
                break;
            case GameModes.camera:
                cameraSystem.enabled = true;
                Debug.Log("Camera mode");
                textToShow = "Camera Mode";
                blockBuilder = false;
                break;
            default:
                break;
        }

        foreach (DraggableObject obj in FindObjectsOfType<DraggableObject>())
        {
            obj.enabled = blockBuilder;
        }
        foreach (BlockSpawner obj in FindObjectsOfType<BlockSpawner>())
        {
            obj.enabled = blockBuilder;
        }
        gamemodeText.text = textToShow;

    }

    //Function to set panel active / Deactive
    public void SwitchPanel(and_dragObj block, bool activeStatus)
    {
        //orientationPanel.SetActive(activeStatus);

        if(activeStatus)
        {
            orientationPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { block.RotateButtons(-1); });
            orientationPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { block.RotateButtons(1); });
            orientationPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { block.ForwardButtons(1); });
            orientationPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { block.ForwardButtons(-1); });
        }
        else
        {
            orientationPanel.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            orientationPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            orientationPanel.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            orientationPanel.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
    public void TogglePanel(bool toggleTo) {
        orientationPanel.SetActive(toggleTo);
    }
}
