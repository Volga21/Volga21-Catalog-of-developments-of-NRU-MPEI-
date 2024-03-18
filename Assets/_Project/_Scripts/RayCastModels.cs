using UnityEngine;
using System.Collections.Generic;

public class RayCastModels : MonoBehaviour
{
    public Interactable _currentInteractable;
    public Interactable _lastInteractable;


    private void Update()
    {
        // Курсор на выбранном объекте
        if (_currentInteractable != null)
        {
            AppManager.Instanse.SetCursorTransform(_currentInteractable.transform.position);
        }

        if (AppManager.Instanse.CanTrace)
        {
            if (AppManager.Instanse.Debug)
            {
                if ((Input.GetMouseButtonDown(0)))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit raycastHit;

                    if (Physics.Raycast(raycast, out raycastHit))
                    {
                        GameObject hittedGameObject = raycastHit.collider.gameObject;

                        if (hittedGameObject.CompareTag("Interactable"))
                        {
                            Interactable hittedInteractableObject = hittedGameObject.GetComponent<Interactable>();
                            _currentInteractable = hittedInteractableObject;
                            AppManager.Instanse.SetCursorActive(true);
                            AppManager.Instanse.UpdateTextPanel(_currentInteractable);

                            if (_lastInteractable == null)
                            {
                                _lastInteractable = _currentInteractable;
                                AppManager.Instanse.InteractWithObject(_currentInteractable);
                            }
                            else if (_currentInteractable != _lastInteractable)
                            {
                                AppManager.Instanse.LooseInteractableObject();
                                _lastInteractable = _currentInteractable;
                                AppManager.Instanse.InteractWithObject(_currentInteractable);
                            }
                            else if (_currentInteractable == _lastInteractable)
                            {
                                AppManager.Instanse.SetCursorActive(false);
                                _lastInteractable = null;
                                LoseInteractable();
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    //и тут с мышки на тач поменять
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit raycastHit;

                    if (Physics.Raycast(raycast, out raycastHit))
                    {
                        GameObject hittedGameObject = raycastHit.collider.gameObject;

                        if (hittedGameObject.CompareTag("Interactable"))
                        {
                            Interactable hittedInteractableObject = hittedGameObject.GetComponent<Interactable>();
                            _currentInteractable = hittedInteractableObject;
                            AppManager.Instanse.SetCursorActive(true);
                            AppManager.Instanse.UpdateTextPanel(_currentInteractable);

                            if (_lastInteractable == null)
                            {
                                _lastInteractable = _currentInteractable;
                                AppManager.Instanse.InteractWithObject(_currentInteractable);
                            }
                            else if (_currentInteractable != _lastInteractable)
                            {
                                AppManager.Instanse.LooseInteractableObject();
                                _lastInteractable = _currentInteractable;
                                AppManager.Instanse.InteractWithObject(_currentInteractable);
                            }
                            else if (_currentInteractable == _lastInteractable)
                            {
                                AppManager.Instanse.SetCursorActive(false);
                                _lastInteractable = null;
                                LoseInteractable();
                            }
                        }
                    }

                }
            }
        }
    }


    public void LoseInteractable()
    {
        AppManager.Instanse.LooseInteractableObject();
        _currentInteractable = null;
    }


}

