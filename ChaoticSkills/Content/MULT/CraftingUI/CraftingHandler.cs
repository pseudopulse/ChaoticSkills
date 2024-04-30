using System;
using RoR2.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ChaoticSkills.Content.MULT.RecipeManager;

namespace ChaoticSkills.Content.MULT {
    public class CraftingHandler : MonoBehaviour {
        public CraftingItem OutputSlot;
        public CraftingSlot[] CraftingSlots;
        private Recipe ourResult;
        public CharacterBody whoUsedUs;
        public Button.ButtonClickedEvent onExit = new();
        private bool hasCraftedAlready = false;
        public void Start() {
            // OutputSlot.resultSlot = true;

            Transform MainPanel = this.transform.Find("MainPanel");
            Transform Juice = MainPanel.Find("Juice");
            Transform Icons = Juice.Find("IconContainer");
            Transform Template = Icons.Find("PickupButtonTemplate");

            CraftingSlots = new CraftingSlot[9];

            // Crafting Grid
            GameObject crafter = GameObject.Instantiate(Icons.gameObject, Juice);
            crafter.GetComponent<GridLayoutGroup>().constraintCount = 3;
            crafter.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;

            Icons.GetComponent<RectTransform>().localPosition = new(0, -160, 0);
            crafter.GetComponent<RectTransform>().localPosition = new(0, 70, 0);

            for (int i = 0; i < 9; i++) {
                GameObject slot = GameObject.Instantiate(Template.gameObject, crafter.transform);
                slot.transform.Find("Icon").gameObject.SetActive(false);
                slot.gameObject.SetActive(true);
                CraftingSlot cSlot = slot.AddComponent<CraftingSlot>();
                CraftingSlots[i] = cSlot;
            }

            GameObject resultSlot = GameObject.Instantiate(Template.gameObject, Juice);
            CraftingItem item = resultSlot.AddComponent<CraftingItem>();
            item.index = ItemIndex.None;
            item.image = resultSlot.transform.Find("Icon").GetComponent<Image>();
            item.image.enabled = false;
            item.resultSlot = true;
            item.GetComponent<RectTransform>().localPosition = new(270, 69, 0);
            resultSlot.gameObject.SetActive(true);
            OutputSlot = resultSlot.GetComponent<CraftingItem>();

            Setup(whoUsedUs);
        }

        public void Setup(CharacterBody body) {
            Transform MainPanel = this.transform.Find("MainPanel");
            Transform Juice = MainPanel.Find("Juice");
            Transform Icons = Juice.Find("IconContainer");
            Transform Template = Icons.Find("PickupButtonTemplate");

            GameObject cancel = Juice.Find("CancelButton").gameObject;

            whoUsedUs = body;
            foreach (ItemIndex item in whoUsedUs.inventory.itemAcquisitionOrder) {
                for (int i = 0; i < whoUsedUs.inventory.GetItemCount(item); i++) {
                    GameObject itemButton = GameObject.Instantiate(Template.gameObject, Icons);
                    itemButton.gameObject.SetActive(true);
                    ItemDef def = ItemCatalog.GetItemDef(item);
                    itemButton.transform.Find("Icon").GetComponent<Image>().sprite = def.pickupIconSprite;
                    
                    CraftingItem crafting = itemButton.AddComponent<CraftingItem>();
                    crafting.index = item;
                    crafting.image = itemButton.transform.Find("Icon").GetComponent<Image>();
                    crafting.handler = this;
                }
            }
        }

        public void Craft() {
            if (ourResult == null || hasCraftedAlready) {
                return;
            }

            for (int i = 0; i < 9; i++) {
                if (CraftingSlots[i].storedItem == null) {
                    continue;
                } 
                ItemIndex item = CraftingSlots[i].storedItem.index;
                whoUsedUs.inventory.RemoveItem(item);
            }

            hasCraftedAlready = true;

            whoUsedUs.inventory.GiveItem(OutputSlot.index, ourResult.ResultCount);

            Destroy(this.gameObject);
        }

        public void UpdateOutputSlot() {
            ourResult = RecipeManager.CheckRecipe(CraftingSlots);
            
            if (ourResult == null) {
                OutputSlot.image.enabled = false;
                return;
            }

            OutputSlot.image.enabled = true;

            OutputSlot.index = ourResult.Result.itemIndex;
            OutputSlot.image.sprite = ourResult.Result.pickupIconSprite;
            OutputSlot.handler = this;
        }
    }
}