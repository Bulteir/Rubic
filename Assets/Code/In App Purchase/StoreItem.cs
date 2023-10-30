using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    public string Item_Id;
    public GameObject StoreController;
    public Button PurchaseButton;
    public TMP_Text PriceText;
    public TMP_Text ContentText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        StoreController.GetComponent<StoreController>().Purchase(Item_Id, PurchaseButton);
        Debug.Log("Satýn alma iþlemi baþladý");
    }

    public void SetItemMetaData(string Title, string Description, string Price)
    {
        //ideal olan IAP servisten gelen title ve description'ý göstermek ancak bunun localizationu nasýl idere edilir bilmediðimiz için kullanmýyoruz.
        PriceText.text = Price;

        if (Item_Id.Contains("no_ads"))
        {
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "IAP_no_ads");
        }
        else if (Item_Id.Contains("joker_easy1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "IAP_joker_easy", new object[] { arguments });
        }
        else if (Item_Id.Contains("joker_very_easy1"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", Description } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "IAP_joker_very_easy", new object[] { arguments });
        }
        else if (Item_Id.Contains("increase_hint"))
        {
            Dictionary<string, string> arguments = new Dictionary<string, string> { { "val", "15" } };
            ContentText.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "IAP_increase_hint", new object[] { arguments });
        }
    }
}
