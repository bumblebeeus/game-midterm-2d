using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

// TODO: adapt coin in this file

public class ShopManager : MonoBehaviour
{
    public int coins; // user's remaining coins
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemSO;
    public GameObject[] shopPanelsGO;
    public Shoptemplate[] shopPanels;
    public Button[] purchaseButtons;

    // TODO: Create the shopItemSO items here and add it to the shopPanelsGO array
    void Start()
    {
        // initialize the shopItemSO array with size of 2 (we expect only 2 items)
        shopItemSO = new ShopItemSO[2];
        for (int i = 0; i < shopItemSO.Length; i++){
            // TODO: create new scriptableObect here (I done it for you, you can change it if you want)
            // Create a new instance of ShopItemSO
            ShopItemSO newShopItem = ScriptableObject.CreateInstance<ShopItemSO>();
            shopItemSO[i] = newShopItem;
            // TODO: fetch the data and set it to the new Instance of ShopItemSO, below is my example:
            /*
            shopItemSO[i].baseCost = ...;
            shopItemSO[i].title = ...;
            shopItemSO[i].image = ...;
            */
            shopItemSO[i].baseCost = 150;
            shopItemSO[i].title = "NewTitle";
            shopItemSO[i].image = Resources.Load<Sprite>("skin2");
            // about the Resources.Load, please read the docs here:
            // https://docs.unity3d.com/ScriptReference/Resources.Load.html
            // Hint: save the images of sprites in the Resources folder

            shopPanelsGO[i].SetActive(true); // you better not change this line or you will be fucked up
        }
        coinUI.text = "Your coins: " + coins.ToString();
        loadPanels();
        CheckPurchaseable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins(int amount){
        coins += amount;
        coinUI.text = coins.ToString();
        CheckPurchaseable();
    }

    // TODO: Change text of shop button to "Equip" or "Equipped" based on each context
    public void CheckPurchaseable(){
        for (int i = 0; i < shopItemSO.Length; i++){
            if (coins >= shopItemSO[i].baseCost){
                purchaseButtons[i].interactable = true;
                
            } else {
                purchaseButtons[i].interactable = false;
                // example of how to change the text on the button:
                // I think the text on the button will be "Purchase" -> "Equip" -> "Equipped" (disable: interactable = false)
                purchaseButtons[i].GetComponentInChildren<TMP_Text>().text = "Not enough coins";
            }
        }
    }


    // TODO: check logic here!
    public void PurchaseItem(int index){
        if (coins >= shopItemSO[index].baseCost){
            coins -= shopItemSO[index].baseCost;
            coinUI.text = "Your coins: " + coins.ToString();
            CheckPurchaseable();
        }
    }

    // this function is to set up the panels of scroll view in shop to be rendered as the shopItemSO array
    public void loadPanels(){
        for (int i = 0; i < shopItemSO.Length; i++){
            shopPanels[i].title.text = shopItemSO[i].title;
            shopPanels[i].itemImage.sprite = shopItemSO[i].image;
            shopPanels[i].purchasePrice.text = "Coins: " + shopItemSO[i].baseCost.ToString();
        }
    }

}
