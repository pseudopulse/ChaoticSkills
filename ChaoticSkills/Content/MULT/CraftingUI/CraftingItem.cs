using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChaoticSkills.Content.MULT {
    public class CraftingItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Vector2 startPosition;
        public Transform originalParent;
        public Image image;
        public ItemIndex index;
        public CraftingSlot ourSlot;
        public bool resultSlot = false;
        public CraftingHandler handler;
        public RectTransform rect;
        public void Start() {
            startPosition = transform.position;
            image = GetComponent<Image>();
            originalParent = transform.parent;
            rect = GetComponent<RectTransform>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (resultSlot) {
                handler.Craft();
                return;
            }

            transform.position = eventData.position;
        }

        public void FixedUpdate() {
            // transform.localPosition = Vector3.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (resultSlot) {
                return;
            }

            image.raycastTarget = false;
            
            if (ourSlot) {
                ourSlot.storedItem = null;
                ourSlot = null;
            }

            // transform.position = Vector3.zero;
            transform.SetParent(null);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (resultSlot) {
                return;
            }

            transform.localPosition = Vector3.zero;

            if (!ourSlot) {
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero;
                transform.parent.GetComponent<GridLayoutGroup>().SetDirty();
            }

            image.raycastTarget = true;

            handler.UpdateOutputSlot();
        }
    }
}