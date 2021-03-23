//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google LLC">
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
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Uses objects to visualize an AugmentedImage
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;
        /// <summary>
        /// Array of models to place when an image is detected.
        /// </summary>
        public GameObject[] Models;

        private int counter = 0;
        public void Awake()
        {
            //this makes sure that position will be set only once
            if (counter == 0)
            {
               // Debug.Log("awake");
                Models[1].transform.localPosition = new Vector3(-0.1f, 0.0f, 0.1f);
            }
            counter = counter + 1;
        }
        /// <summary>
        /// The Start method which prevents all objects from rendering at once
        /// </summary>
        public void Start()
        {
           // Debug.Log("start in a visualizer");
            float halfWidth = 0;
            float halfHeight = 0;
            //SunMovement.setPosition();
            Models[0].SetActive(false);
            Models[1].SetActive(false);
            //Models[2].SetActive(false);
            //Models[3].SetActive(false);
            if (Image != null) {
                halfWidth = Image.ExtentX / 2;
                halfHeight = Image.ExtentZ / 2;
            }
            //cat
            Models[0].transform.localPosition =
                (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
            //Debug.Log("cat position: " + Models[0].transform.localPosition);
            //Models[2].transform.localPosition =
           // Models[2].transform.localPosition =
            //    (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
           // Models[3].transform.localPosition =
            //   (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
        }

        /// <summary>
        /// The Update method which render correct models for a demo
        /// </summary>
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                Models[0].SetActive(false);
                Models[1].SetActive(false);
              //  Models[2].SetActive(false);
              //  Models[3].SetActive(false);
                return;
            }
            if (Image.Name == "demo2" && Image.TrackingState == TrackingState.Tracking)
            {
                Models[0].SetActive(false);
                Models[1].SetActive(false);
            }
            if (Image.Name == "demo1" && Image.TrackingState == TrackingState.Tracking)
            {
                Models[0].SetActive(true);
                Models[1].SetActive(true);
            }
            else
            {
                return;
            }
        }
    }
}
