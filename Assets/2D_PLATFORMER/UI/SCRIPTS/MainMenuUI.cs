using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    ///// reference to all the panels and objects we need to toggle on/off
    [Header("Panels & Objects")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject MainMenuButtons;
    [SerializeField] private GameObject MainMenuAnimation;
    [SerializeField] private GameObject MainMenuTittle;
    [SerializeField] private GameObject ControlsPanel;
    [SerializeField] private GameObject HudPanel;
    [SerializeField] private GameObject Player;

    ///// Default buttons to focus when switching panels
    ///// which buttons needs to be first selected when we open the menu or the controls
    [Header("Selection Defaults")]
    [SerializeField] private GameObject MainMenuFirstButton;
    [SerializeField] private GameObject ControlsFirstButton;

    ///// Internal state to track which button should be focused when clicking the background
    private GameObject currentDefaultButton;

    ///// Internal state to manage the click-fix behavior
    ///// I did this becuase when i was clicking on the screen the selection of the button was disappearing
    ///// and i wanted to make sure that when the menu is active, if the user clicks on the background, it will refocus the last button that was selected 
    private bool menuIsActive = true;

    void Awake() 
    {
        ///// Ensure the menu starts in the correct state
        if (ControlsPanel) ControlsPanel.SetActive(false);
        if (MainMenuButtons) MainMenuButtons.SetActive(false);
        
        ///// Set the initial default button to the main menu's first button
        currentDefaultButton = MainMenuFirstButton;
    }

    void Start()
    {
        ///// Focus the initial default button when the menu is first shown
        FocusButton(currentDefaultButton);
    }

    void Update()
    {
        ///// If the menu is active and no button is currently selected, refocus the last default button
        if (menuIsActive && EventSystem.current.currentSelectedGameObject == null)
        {
            ///// This ensures that if the user clicks on the background or somehow deselects the button
            ///// it will automatically refocus the last button that was selected
            FocusButton(currentDefaultButton);
        }
    }

    public void onStartClicked()
    {    
        ///// this will hide all the menu elements and show the HUD and the player when the Start button is clicked
        menuIsActive = false;
        ControlsPanel.SetActive(false);
        MenuPanel.SetActive(false);
        HudPanel.SetActive(true);
        Player.SetActive(true);
        
        ///// Clear any selected button to prevent UI issues when the menu is hidden
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void onControlsClicked()
    {
        ///// Toggle the controls panel visibility
        bool openingControls = !ControlsPanel.activeSelf;
        ///// Show/hide the controls panel
        ControlsPanel.SetActive(openingControls);
        
        ///// Hide/Show all Main Menu elements at once
        MainMenuButtons.SetActive(!openingControls);
        MainMenuAnimation.SetActive(!openingControls);
        MainMenuTittle.SetActive(!openingControls);

        ///// Update which button should be focused if the user clicks the background
        currentDefaultButton = openingControls ? ControlsFirstButton : MainMenuFirstButton;
        FocusButton(currentDefaultButton);
    }

    public void onExitClicked()
    {
        ///// this will quit the application when the Exit button is clicked
        Application.Quit();
    }

    // Helper to ensure the button is properly highlighted
    private void FocusButton(GameObject target)
    {
        ///// This method ensures that the specified button is selected in the EventSystem,
        ///// which is necessary for keyboard/controller navigation to work correctly
        if (target == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }
}