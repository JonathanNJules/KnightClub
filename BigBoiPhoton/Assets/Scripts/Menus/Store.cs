using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private Dictionary<string, int> prices;
    public StoreTrigger st;

    void Awake()
    {
        prices = new Dictionary<string, int>
        {
            {"Mask", 15 },
            {"Headband", 20 },
            {"Bowtie", 35 },
            {"Tophat", 55 },
            {"Glasses", 100 },
            {"Headphones", 250 },
            {"GraduationCap", 300 },
            {"BucketHat", 500 }
        };
    }

    public void BuyItem(string itemname)
    {
        if (prices[itemname] <= GameManager.user.currency)
        {
            st.HideStore();
            GameManager.SetHeadwear(itemname, prices[itemname]);
        }
    }
}
