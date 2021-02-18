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
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        public GameObject[] Models;

        /// <summary>
        /// A model for the lower left corner of the frame to place when an image is detected.
        /// </summary>
        //public GameObject FrameLowerLeft;

        /// <summary>
        /// A model for the lower right corner of the frame to place when an image is detected.
        /// </summary>
       // public GameObject FrameLowerRight;

        /// <summary>
        /// A model for the lower left corner of the frame to place when an image is detected.
        /// </summary>
       // public GameObject FrameUpperLeft;

        /// <summary>
        /// A model for the lower right corner of the frame to place when an image is detected.
        /// </summary>
        //public GameObject FrameUpperRight;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                Models[0].SetActive(false);
                Models[1].SetActive(false);
                Models[2].SetActive(false);
                Models[3].SetActive(false);
                return;
            }
            if (Image.Name == "demo2" && Image.TrackingState == TrackingState.Tracking)
            {
                Models[0].SetActive(false);
                Models[1].SetActive(false);
                float halfWidth = Image.ExtentX/2;
                float halfHeight = Image.ExtentZ/2;
                Models[2].transform.localPosition =
                    (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
                Models[3].transform.localPosition =
                   (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
                Models[2].SetActive(true);
                Models[3].SetActive(true);
            }
            else if (Image.Name == "demo1" && Image.TrackingState == TrackingState.Tracking)
            {
                Models[2].SetActive(false);
                Models[3].SetActive(false);
                float halfWidth = Image.ExtentX/4;
                float halfHeight = Image.ExtentZ/2;
                Models[0].transform.localPosition =
                    (halfWidth * Vector3.left) + (halfHeight * Vector3.forward);
                Models[1].transform.localPosition =
                   (halfWidth * Vector3.right) + (halfHeight * Vector3.forward);
                Models[0].SetActive(true);
                Models[1].SetActive(true); 
            }
            else {
                return;
            }
        }
    }
}
