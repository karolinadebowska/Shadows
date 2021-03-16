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
        //public Examples.AugmentedImage.AugmentedImageController augmImgContr;
        private int count = 0;
        public AugmentedImageController controller;
        private GameObject prefab;
        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject[] PawnPrefab;
        [HideInInspector]
        public static GameObject puzzle;
        [HideInInspector]
        public static GameObject lamp;
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
            Debug.Log("demo2 in canstart "+controller.Demo2);
            if (count<1 && (controller.Demo2 == true) && gesture.TargetObject == null)
            {
                //Initialize();
                Debug.Log("can start manipulation");
                return true;
            }

            return false;
        }
        /*
        public void Initialize()
        {
            Debug.Log("awake in pawn");
            if (GameObject.FindGameObjectWithTag("lamp"))
            {
                lamp = (GameObject)FindObjectOfType(typeof(GameObject));
                Debug.Log("lamp " + lamp);
            }
           // if (GameObject.FindGameObjectWithTag("puzzle"))
           // {
               // puzzle = (GameObject)FindObjectOfType(typeof(GameObject));
               // Debug.Log("puzzle " + puzzle);
           // }
        }*/
        public void Start()
        {
            controller = GameObject.Find("Controller").GetComponent<AugmentedImageController>();
        }
        public Pose pose;
        public AugmentedImage image;
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
            /*
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
            */
                //Debug.Log("demo2 inside count changer" + augmImgContr.Demo2);
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                
                /*if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                // else if (controller._tempAugmentedImages[0].Name.Equals("demo2"))
                // { 
                else { */
                    this.image = controller._tempAugmentedImages[0];
                    Debug.Log("name of current demo :"+this.image.Name);
                    pose = image.CenterPose;
                    count = count + 1;
                    Debug.Log("I shouldn't be a null objet: " + lamp);
                    Debug.Log("rotation "+pose.rotation);
                    var gameObject0 = Instantiate(PawnPrefab[0], new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z-0.05f), new Quaternion(-0.707f, 0f, 0f, 0.707f));
                    var gameObject1 = Instantiate(PawnPrefab[1], new Vector3(pose.position.x + 0.1f, pose.position.y, pose.position.z - 0.05f), pose.rotation);
                    //Debug.Log(hit.Pose.position + "= " + pose.position+"rotation :"+hit.Pose.rotation+" = "+pose.rotation);
                    // Instantiate manipulator.
                    var manipulator0 =
                       Instantiate(ManipulatorPrefab, new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z - 0.05f), pose.rotation);
                    //var manipulator1 = Instantiate(ManipulatorPrefab, new Vector3(pose.position.x + 0.1f, pose.position.y, pose.position.z - 0.1f), pose.rotation);
                var manipulator1 =
                  Instantiate(ManipulatorPrefab, new Vector3(pose.position.x + 0.1f, pose.position.y, pose.position.z - 0.05f), pose.rotation);
                // Make game object a child of the manipulator.

                gameObject0.transform.parent = manipulator0.transform;
                gameObject1.transform.parent = manipulator1.transform;
                    // Make game object a child of the manipulator.
                    //gameObject1.transform.parent = manipulator1.transform;
                    //gameObject.transform.parent = manipulator.transform;
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    Debug.Log("Image: " + image+" Count: "+count);
                var anchor0 = image.CreateAnchor(new Pose(new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z - 0.05f), pose.rotation));
                var anchor1 = image.CreateAnchor(new Pose(new Vector3(pose.position.x + 0.1f, pose.position.y, pose.position.z - 0.05f), pose.rotation));

                // Make manipulator a child of the anchor.
                manipulator0.transform.parent = anchor0.transform;
                manipulator1.transform.parent = anchor1.transform;
                // Select the placed object.
                manipulator0.GetComponent<Manipulator>().Select();
                manipulator1.GetComponent<Manipulator>().Select();
                // the physical world evolves.
                // var anchor1 = hit.Trackable.CreateAnchor(new Pose(new Vector3(hit.Pose.position.x + 0.1f, hit.Pose.position.y, hit.Pose.position.z), hit.Pose.rotation));
                // Pose pose = Pose.ctor(new Vector3(-0.4f, -0.5f, -0.5f), Quarternion)
                // Make manipulator a child of the anchor.
                //manipulator1.transform.parent = anchor1.transform;

                // Select the placed object.
                //manipulator1.GetComponent<Manipulator>().Select();
                // }
           // }

            /*
            
            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit) && count < 1)
            {
                //Debug.Log("demo2 inside count changer" + augmImgContr.Demo2);
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    foreach (var image in controller._tempAugmentedImages)
                    {
                        pose = image.CenterPose;
                    }
                    count = count + 1;
                    var gameObject0 = Instantiate(PawnPrefab[0], new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z), pose.rotation);
                    //var gameObject1 = Instantiate(PawnPrefab[1], puzzle.transform.position, puzzle.transform.rotation);
                    Debug.Log(hit.Pose.position + "= " + pose.position+"rotation :"+hit.Pose.rotation+" = "+pose.rotation);
                    // Instantiate manipulator.
                    var manipulator0 =
                       Instantiate(ManipulatorPrefab, new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z), pose.rotation);
                    
                    //var manipulator1 =
                    //  Instantiate(ManipulatorPrefab, new Vector3(hit.Pose.position.x + 0.1f, hit.Pose.position.y, hit.Pose.position.z), hit.Pose.rotation);
                    // Make game object a child of the manipulator.

                    gameObject0.transform.parent = manipulator0.transform;

                    // Make game object a child of the manipulator.
                    //gameObject1.transform.parent = manipulator1.transform;
                    //gameObject.transform.parent = manipulator.transform;
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.

                    var anchor0 = hit.Trackable.CreateAnchor(new Pose(new Vector3(pose.position.x - 0.1f, pose.position.y, pose.position.z), pose.rotation));
                    // Make manipulator a child of the anchor.
                    manipulator0.transform.parent = anchor0.transform;
                    // Select the placed object.
                    manipulator0.GetComponent<Manipulator>().Select();

                    // the physical world evolves.
                   // var anchor1 = hit.Trackable.CreateAnchor(new Pose(new Vector3(hit.Pose.position.x + 0.1f, hit.Pose.position.y, hit.Pose.position.z), hit.Pose.rotation));
                    // Pose pose = Pose.ctor(new Vector3(-0.4f, -0.5f, -0.5f), Quarternion)
                    // Make manipulator a child of the anchor.
                    //manipulator1.transform.parent = anchor1.transform;

                    // Select the placed object.
                    //manipulator1.GetComponent<Manipulator>().Select();
                }
            }*/
        }
    }
}
