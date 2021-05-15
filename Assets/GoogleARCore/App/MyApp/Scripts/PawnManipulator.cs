//-----------------------------------------------------------------------
// <copyright file="PawnManipulator.cs" company="Google LLC">
//
// Copyright 2019 Google LLC
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

namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using GoogleARCore.Examples.AugmentedImage;
    using UnityEngine;
    using System;

    /// <summary>
    /// Controls the placement of objects via a tap gesture.
    /// </summary>
    public class PawnManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;
        private int clickCount = 0;
        public AugmentedImageController controller;
        public static bool _gameWon = false;
        public bool GameWon
        {
            get
            { return _gameWon; }
            set
            { _gameWon = value; }
        }
        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject[] PawnPrefab;
        [HideInInspector]
        private Pose pose;
        public static bool _gameMode = false;
        public bool GameMode
        {
            get
            { return _gameMode; }
            set
            { _gameMode = value; }
        }
        private AugmentedImage image;
        private static GameObject gameObject0;
        private static GameObject gameObject1;
        private static GameObject manipulator0;
        private static GameObject manipulator1;
        private static Anchor anchor0;
        private static Anchor anchor1;
        private Vector3 lampOriginalPosition;
        private Vector3 puzzleOriginalPosition;
        public bool firstTimeGameOn = true;
        /// <summary>
        /// Manipulator prefab to attach placed objects to.
        /// </summary>
        public GameObject ManipulatorPrefab;
        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            
            if (controller.Demo2 == true)
            {
                if (gesture.TargetObject == null)
                {
                    //Debug.Log("can start manipulation");
                }
                else if (GameMode && gesture.TargetObject != null)
                {
                    RaycastHit hitobject;
                    Ray ray = FirstPersonCamera.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hitobject)) { 

                        //Debug.Log(hitobject.transform.name+ " "+ hitobject.transform.tag);
                        // Check if what is hit is the desired object
                        if (hitobject.transform.tag == "lamp")
                        {
                            GameWon = true;
                        }
                    }
                }
                return true;
            }
            return false;
        }
        //Function to get a random number 
        private static readonly System.Random random = new System.Random();
        private static readonly object syncLock = new object();
        public static float generateRandom(float min, float max)
        {
            lock (syncLock)
            {
                double val = (random.NextDouble() * (max - min) + min);
                return (float)val;
            }
        }
        private float xUpperBound, xLowerBound;
        private float zUpperBound, zLowerBound;
        public void turnGameMode()
        {
            GameMode = true;
            //Debug.Log("camera: " + FirstPersonCamera);
            //Debug.Log("lamp original position: " + lampOriginalPosition + " " + gameObject0.transform.position + " " + gameObject0.transform.localPosition);
            if (firstTimeGameOn)
            {
                lampOriginalPosition = gameObject0.transform.position;
                puzzleOriginalPosition = gameObject1.transform.position;
                xUpperBound = lampOriginalPosition.x + 0.3f;
                xLowerBound = lampOriginalPosition.x - 0.05f;
                zUpperBound = lampOriginalPosition.z + 0.1f;
                zLowerBound = lampOriginalPosition.z - 0.3f;
            }
            float valueX = generateRandom(xLowerBound, xUpperBound);
            float valueZ = generateRandom(zLowerBound, zUpperBound);
            //Debug.Log(new Vector3(lampOriginalPosition.x + valueX, lampOriginalPosition.y, lampOriginalPosition.z + valueZ));
            if (Math.Abs(valueX - xLowerBound) > Math.Abs(valueX - zUpperBound))
            {
                //Debug.Log("rotation " + gameObject0.transform.rotation);
                Vector3 rotationVector = new Vector3(-90, 180, 0);
                gameObject0.transform.rotation = Quaternion.Euler(rotationVector);
            }
            else
            {
                gameObject0.transform.rotation = new Quaternion(-0.707f, 0f, 0f, 0.707f);
            }
            firstTimeGameOn = false;
            manipulator0.transform.position = new Vector3(lampOriginalPosition.x + valueX, lampOriginalPosition.y, lampOriginalPosition.z + valueZ);
            controller.displayControl();
        }
        public void Start()
        {
            FirstPersonCamera = Camera.main;
            clickCount = 0;
        }
        public void resetPositions() {
            manipulator0.transform.position = new Vector3(lampOriginalPosition.x, lampOriginalPosition.y, lampOriginalPosition.z);
            gameObject0.transform.rotation = new Quaternion(-0.707f, 0f, 0f, 0.707f);
            manipulator1.transform.position = new Vector3(puzzleOriginalPosition.x, puzzleOriginalPosition.y, puzzleOriginalPosition.z);
        }
        public void disableObjects() {
            if (gameObject0.activeSelf)
            {
                //Debug.Log("disableObject");
                gameObject0.SetActive(false);
                gameObject1.SetActive(false);
            }
        }
        public void enableObjects()
        {
            if (gameObject0 && gameObject1)
            {
                //Debug.Log("enableObject");
                gameObject0.SetActive(true);
                gameObject1.SetActive(true);
            }
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }
            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            if (clickCount < 1)
            {
                clickCount = clickCount + 1;
                //find an image to place an anchor
                int index = controller._tempAugmentedImages.FindIndex(x => x.Name == "demo2");
                image = controller._tempAugmentedImages[index];
                pose = image.CenterPose;
                //instatntiate game objects
                gameObject0 = Instantiate(PawnPrefab[0], new Vector3(pose.position.x - 0.055f, pose.position.y, pose.position.z - 0.03f), new Quaternion(-0.707f, 0f, 0f, 0.707f));
                gameObject1 = Instantiate(PawnPrefab[1], new Vector3(pose.position.x, pose.position.y, pose.position.z - 0.03f), new Quaternion(0f, 1f, 0f, 0f));
                // Instantiate manipulators
                manipulator0 = Instantiate(ManipulatorPrefab, new Vector3(pose.position.x - 0.055f, pose.position.y, pose.position.z - 0.03f), pose.rotation);
                manipulator1 = Instantiate(ManipulatorPrefab, new Vector3(pose.position.x, pose.position.y, pose.position.z - 0.03f), pose.rotation);
                // Make game object a child of the manipulator.
                gameObject0.transform.parent = manipulator0.transform;
                gameObject1.transform.parent = manipulator1.transform;
                // Create an anchor to allow ARCore to track the image
                anchor0 = image.CreateAnchor(new Pose(new Vector3(pose.position.x - 0.055f, pose.position.y, pose.position.z - 0.03f), pose.rotation));
                anchor1 = image.CreateAnchor(new Pose(new Vector3(pose.position.x, pose.position.y, pose.position.z - 0.03f), pose.rotation));
                // Make manipulator a child of the anchor.
                manipulator0.transform.parent = anchor0.transform;
                manipulator1.transform.parent = anchor1.transform;
                // Select the placed object.
                manipulator0.GetComponent<Manipulator>().Select();
                manipulator1.GetComponent<Manipulator>().Select();
            }
        }
    }
}
