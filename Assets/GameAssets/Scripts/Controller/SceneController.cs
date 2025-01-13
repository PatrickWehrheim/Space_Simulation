using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class SceneController : MonoBehaviour
{
    [SerializeField]
    private List<string> _scenesNames;
    private int _maxSceneIndex { get => _scenesNames.Count - 1; }

    private int _currentSceneIndex;

    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        if (_scenesNames == null)
            _scenesNames = new List<string>();
    }

    public void OnNextScene(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (++_currentSceneIndex > _maxSceneIndex)
                _currentSceneIndex = 0;

            SceneManager.LoadScene(_currentSceneIndex);
        }
    }

    public void OnPreviousScene(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (--_currentSceneIndex < 0)
                _currentSceneIndex = _maxSceneIndex;

            SceneManager.LoadScene(_currentSceneIndex);
        }
    }
}
