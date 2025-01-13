using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool _canCameraMove;
    [SerializeField] private SelectedUnitsSettings _selectedUnitsSettings;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private float _boundry;

    private Ray _ray;
    private RaycastHit _hit;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (_canCameraMove)
            MoveCameraOnBoundries();
    }

    private void MoveCameraOnBoundries()
    {
        Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        Vector3 currentCameraPosition = Camera.main.transform.position;

        if (currentMousePosition.x > Screen.width - _boundry)
        {
            currentCameraPosition.x += _cameraSpeed;
        }
        else if (currentMousePosition.x < _boundry)
        {
            currentCameraPosition.x -= _cameraSpeed;
        }

        if (currentMousePosition.y > Screen.height - _boundry)
        {
            currentCameraPosition.z += _cameraSpeed;
        }
        else if (currentMousePosition.y < _boundry)
        {
            currentCameraPosition.z -= _cameraSpeed;
        }

        Camera.main.transform.position = currentCameraPosition;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(_ray, out _hit))
            {
                if(_hit.transform.gameObject.layer == 7)
                {
                    if (_hit.transform.gameObject.CompareTag("Fighter"))
                    {
                        FighterController fighterController = _hit.transform.gameObject.GetComponent<FighterController>();
                        if (fighterController == null)
                        {
                            fighterController = _hit.transform.gameObject.GetComponentInParent<FighterController>();
                        }
                        fighterController.IsClicked();
                        _selectedUnitsSettings.SelectedUnits.Add(fighterController.SquadController);
                    }
                    else if (_hit.transform.gameObject.CompareTag("Squad"))
                    {
                        SquadController squadController = _hit.transform.gameObject.GetComponent<SquadController>();
                        if (squadController == null)
                        {
                            squadController = _hit.transform.gameObject.GetComponentInParent<SquadController>();
                        }
                        squadController.IsClicked();
                        _selectedUnitsSettings.SelectedUnits.Add(squadController);
                    }
                    else if (_hit.transform.gameObject.CompareTag("Fregate"))
                    {
                        FregateController fregateController = _hit.transform.gameObject.GetComponent<FregateController>();
                        if (fregateController == null)
                        {
                            fregateController = _hit.transform.gameObject.GetComponentInParent<FregateController>();
                        }
                        fregateController.IsClicked();
                        _selectedUnitsSettings.SelectedUnits.Add(fregateController);
                    }
                }
                else
                {
                    List<ISelectable> units = new List<ISelectable>();
                    foreach (var unit in _selectedUnitsSettings.SelectedUnits)
                    {
                        units.Add(unit);
                    }

                    foreach (var unit in _selectedUnitsSettings.SelectedUnits)
                    {
                        unit.Deselect();
                        units.Remove(unit);
                    }

                    _selectedUnitsSettings.SelectedUnits = units;
                }
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_selectedUnitsSettings.SelectedUnits.Count > 0)
            {
                _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(_ray, out _hit))
                {
                    if (_hit.transform.CompareTag("Ground"))
                    {
                        Vector3 point = new Vector3(_hit.point.x, 0, _hit.point.z);
                        foreach (ISelectable selectable in _selectedUnitsSettings.SelectedUnits)
                        {
                            selectable.OnRightClick(point);
                        }
                    }
                }
            }
        }
    }

    public void OnSpacePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            transform.position = _startPosition;
        }
    }
}
