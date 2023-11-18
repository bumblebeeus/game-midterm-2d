using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemSO;
    public GameObject[] shopPanelsGO;
    public Shoptemplate[] shopPanels;
    public Button[] purchaseButtons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemSO.Length; i++){
            shopPanelsGO[i].SetActive(true);
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

    public void CheckPurchaseable(){
        for (int i = 0; i < shopItemSO.Length; i++){
            if (coins >= shopItemSO[i].baseCost){
                purchaseButtons[i].interactable = true;
            } else {
                purchaseButtons[i].interactable = false;
            }
        }
    }

    public void PurchaseItem(int index){
        if (coins >= shopItemSO[index].baseCost){
            coins -= shopItemSO[index].baseCost;
            coinUI.text = "Your coins: " + coins.ToString();
            CheckPurchaseable();
        }
    }

    public void loadPanels(){
        for (int i = 0; i < shopItemSO.Length; i++){
            shopPanels[i].title.text = shopItemSO[i].title;
            shopPanels[i].itemImage.sprite = shopItemSO[i].image;
            shopPanels[i].purchasePrice.text = "Coins: " + shopItemSO[i].baseCost.ToString();
        }
    }

}
