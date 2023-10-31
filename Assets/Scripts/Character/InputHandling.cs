using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandling : MonoBehaviour
{

    private DragonflyController dragonflyController;
    private AbilityManager abilityManager;
    private InventoryManager inventoryManager; // Lisää tämä viittaus

    private void Awake()
    {
        dragonflyController = GetComponent<DragonflyController>();
        abilityManager = GetComponent<AbilityManager>();
        if (abilityManager == null)
        {
            Debug.LogError("AbilityManager not found on the same GameObject as InputHandling.");
        }
        inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found or not properly initialized.");
        }
    }
    public void HandleInput()
    {
        // Tarkista, onko hiiri tai kosketus UI-elementin päällä
        if (EventSystem.current.IsPointerOverGameObject() || IsTouchOverUI())
        {
            return; // Jos on, älä suorita seuraavaa koodia
        }

        //Tarkista hiiren napsautus tai kosketussyöte
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            dragonflyController.Jump();
        }
    }

    public bool IsTouchOverUI()
    {
        if (Input.touchCount > 0)
        {
            // Tarkista ensimmäinen kosketus (voit myös käydä läpi kaikki kosketukset tarvittaessa)
            Touch touch = Input.GetTouch(0);
            int touchID = touch.fingerId;

            // Tarkista, onko kosketus UI-elementin päällä
            return EventSystem.current.IsPointerOverGameObject(touchID);
        }

        return false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Voit käyttää sopivaa näppäintä tai syötettä
        {
            AbilitySO currentAbility = abilityManager.currentAbility;
            abilityManager.UseCurrentAbility();
        }
    }
}
