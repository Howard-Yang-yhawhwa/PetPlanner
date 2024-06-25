// Copyright (C) 2024 ricimi. All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Ricimi
{
    // This class is responsible for creating and opening a popup of the
    // given prefab and adding it to the UI canvas of the current scene.
    public class PopupOpener : MonoBehaviour
    {
        public Popup popupManager;

        Button button;

        private void Awake()
        {
            if (TryGetComponent(out button) == false)
            {
                Debug.LogError($"No button found in PopupOpener ({gameObject.name})");
                return;
            }

            button.onClick.AddListener(OpenPopup);
        }

        public virtual void OpenPopup()
        {
            popupManager.Open();
        }
    }
}
