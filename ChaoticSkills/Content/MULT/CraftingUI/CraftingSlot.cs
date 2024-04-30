using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChaoticSkills.Content.MULT {
    public class CraftingSlot : MonoBehaviour, IDropHandler
    {
        public CraftingItem storedItem;
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.transform) {
                CraftingItem item = eventData.pointerDrag.GetComponent<CraftingItem>();
                if (item) {
                    item.ourSlot = this;
                    storedItem = item;
                    item.transform.SetParent(this.transform);
                }
            }
        }
    }
}