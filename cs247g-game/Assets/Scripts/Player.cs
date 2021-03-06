using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public SceneController sceneController;
    public Scene2 scene2;
    public GameObject canvas; // 2D interactions (phone screen, menus)
    private bool inRange;
    private Collider collision;
    private Item item;
    private Outline script;
    private bool isTV;
    private TextMeshProUGUI bottomText;


    void Start()
    {
        inRange = false;
        isTV = true;
        collision = null;
        item = null;
        bottomText = canvas.transform.Find("Bottom Text 2").GetComponent<TextMeshProUGUI>();
        bottomText.gameObject.SetActive(false);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        collision = other;
        inRange = true;

        // Highlight item
        script = other.GetComponent<Outline>();
        if (script)
        {
            sceneController.HighlightObject(other.gameObject, 8);
        }
        
        if (other.tag == "TV")
        {
            isTV = true;
            ShowBottomText("Press 'E' to interact");
            // Debug.Log("TV near");
        }

    }

    public void OnTriggerExit(Collider other)
    {
        // Dehighlight item
        script = other.GetComponent<Outline>();
        if (script)
        {
            sceneController.DehighlightObject(other.gameObject);
        }

        if (other.tag == "TV")
        {
            HideBottomText();
        }

        isTV = false;
        collision = null;
        inRange = false;
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

    void Update()
    {
        if (collision != null) {
            item = collision.GetComponent<Item>();
            inRange = true;
        }

        // Interact with items
        if (inRange && isTV && Input.GetKeyDown(KeyCode.E))
        {
            HideBottomText();
            StartCoroutine(scene2.OpenAd());
            Debug.Log("Button pressed!");
        }

        // Collect item
        //if (inRange && item && Input.GetKeyDown(KeyCode.E))
        //{
            //inventory.AddItem(item.item, 1);
            //Destroy(collision.gameObject);
            //collision = null;
            //inRange = false;
        //}
    }

    // Show str as bottom text
    private void ShowBottomText(string str)
    {
        bottomText.text = str;
        bottomText.gameObject.SetActive(true);
    }

    // Hide bottom text
    private void HideBottomText()
    {
        bottomText.gameObject.SetActive(false);
    }
}
