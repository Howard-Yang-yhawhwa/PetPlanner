// Copyright (C) 2024 ricimi. All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // This class is responsible for popup management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.
    public class Popup : MonoBehaviour
    {
        [SerializeField] Image background;
        [SerializeField] Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);
        [SerializeField] float destroyTime = 0.5f;


        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            background.color = backgroundColor;
        }

        public void Open()
        {
            AddBackground();

            if (animator == null) animator = GetComponent<Animator>();

            animator.SetBool("Open", true);
        }

        public void Close()
        {
            if (animator == null) animator = GetComponent<Animator>();

            animator.SetBool("Open", false);

            RemoveBackground();
        }

        // We destroy the popup automatically 0.5 seconds after closing it.
        // The destruction is performed asynchronously via a coroutine. If you
        // want to destroy the popup at the exact time its closing animation is
        // finished, you can use an animation event instead.
        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(destroyTime);
        }

        private void AddBackground()
        {
            background.CrossFadeAlpha(1.0f, 0.4f, false);
            background.gameObject.SetActive(true);
        }

        private void RemoveBackground()
        {
            if (background != null)
            {
                background.CrossFadeAlpha(0.0f, 0.2f, false);
            }

            background.gameObject.SetActive(false);
        }
    }
}
