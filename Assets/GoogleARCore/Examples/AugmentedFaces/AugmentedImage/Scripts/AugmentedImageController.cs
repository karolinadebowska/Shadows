//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google LLC">
//
// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCore.Examples.ObjectManipulation;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for AugmentedImage example.
    /// </summary>
    /// <remarks>
    /// In this sample, we assume all images are static or moving slowly with
    /// a large occupation of the screen. If the target is actively moving,
    /// we recommend to check <see cref="AugmentedImage.TrackingMethod"/> and
    /// render only when the tracking method equals to
    /// <see cref="AugmentedImageTrackingMethod"/>.<c>FullTracking</c>.
    /// See details in <a href="https://developers.google.com/ar/develop/c/augmented-images/">
    /// Recognize and Augment Images</a>
    /// </remarks>
    public class AugmentedImageController : MonoBehaviour
    {
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
        public Text amPm;

        public bool _demo2;
        public bool Demo2
        {
            get
            {
                // Reads are usually simple
                return _demo2;
            }
            set
            {
                // You can add logic here for race conditions,
                // or other measurements
                _demo2 = value;
            }
        }
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject LoadingOverlay;
        public GameObject LoadingDemo1;
        public GameObject LoadingDemo2;

        public GameObject mainSlider;
        public PawnManipulator pawn;
        public static Slider secondarySlider;
        
        public CanvasGroup canvasGroup;

        private Dictionary<int, AugmentedImageVisualizer> _visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();
        public List<AugmentedImage> _tempAugmentedImages = new List<AugmentedImage>();
        public int counter = 0;

        void HideUI()
        {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        }
        void ShowUI()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            if (GameObject.FindGameObjectWithTag("amPm"))
            {
                amPm = (Text)FindObjectOfType(typeof(Text));
                amPm.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            LoadingDemo1.SetActive(false);
            LoadingDemo2.SetActive(false);
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
            HideUI();
            if (GameObject.FindGameObjectWithTag("secondarySlider"))
            {
                secondarySlider = (Slider)FindObjectOfType(typeof(Slider));
                secondarySlider.gameObject.SetActive(false);
            }            
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(_tempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do
            // not previously have a visualizer. Remove visualizers for stopped images.
            foreach (var image in _tempAugmentedImages)
            {
               // Debug.Log("image : "+image.Name+" tracking method "+image.TrackingMethod);
                AugmentedImageVisualizer visualizer = null;
                _visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && visualizer == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (AugmentedImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    _visualizers.Add(image.DatabaseIndex, visualizer);
                }
                //remove not current visualizer                
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && visualizer != null)
                {
                    if (Demo2 && image.Name.Equals("demo1"))
                    {
                        //remove the object
                        _visualizers.Remove(image.DatabaseIndex);
                        GameObject.Destroy(visualizer.gameObject);
                    }
                    else if (!Demo2 && image.Name.Equals("demo2"))
                    {
                        Debug.Log("calling disableObjects");
                        pawn.disableObjects();
                    }
                }
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.NotTracking && visualizer != null)
                {
                    _visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }
            // Show the fit-to-scan overlay if there are no images that are Tracking.
            foreach (var visualizer in _visualizers.Values)
            {
                if (visualizer.Image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
                {
                    if (visualizer.Image.Name.Equals("demo1"))
                    {
                        Demo2 = false;
                        LoadingOverlay.SetActive(false);
                        //disable landscape mode
                        Screen.autorotateToLandscapeRight = false;
                        Screen.autorotateToLandscapeLeft = false;
                        if (!clicked1)
                            LoadingDemo1.SetActive(true);
                        else
                            ShowUI();
                    }
                    else if (visualizer.Image.Name.Equals("demo2"))
                    {
                        Demo2 = true;
                        pawn.enableObjects();
                        HideUI();
                        LoadingOverlay.SetActive(false);
                        if (!clicked2)
                            LoadingDemo2.SetActive(true);

                        //enable landscape mode
                        Screen.autorotateToLandscapeRight = true;
                        Screen.autorotateToLandscapeLeft = true;


                    }
                    return;
                }
               // LoadingOverlay.SetActive(true);
            }
        }
        private bool clicked1 = false;
        private bool clicked2 = false;
        public void onClick1() {
            LoadingDemo1.SetActive(false);
            clicked1 = true;
            ShowUI();
        }
        public void onClick2()
        {
            LoadingDemo2.SetActive(false);
            clicked2 = true;
        }
    }
}
