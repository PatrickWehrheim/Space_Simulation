using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Skybox_rotator : MonoBehaviour
{
    public TextMeshProUGUI skyboxNameText;
    public List<Material> skyboxes;
    private int currentSkybox = 0;

    private bool _isLeftMouseClicked = false;

    void Start()
    {
        SetSkybox();
    }

	void Update ()
    {
        if (_isLeftMouseClicked)
        {
            _isLeftMouseClicked = false;

            // increment the skybox
            currentSkybox++;
            if (currentSkybox >= skyboxes.Count)
            {
                // loop round to the first skybox if we have reached the last skybox
                currentSkybox = 0;
            }

            SetSkybox();
        }
	}

    void SetSkybox()
    {
        // set the skybox
        RenderSettings.skybox = skyboxes[currentSkybox];
        skyboxNameText.text = skyboxes[currentSkybox].name;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isLeftMouseClicked = true;
        }
    }
}
