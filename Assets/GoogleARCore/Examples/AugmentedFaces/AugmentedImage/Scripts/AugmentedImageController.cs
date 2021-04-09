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
    //using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using GoogleARCore;
    using GoogleARCore.Examples.ObjectManipulation;
    using UnityEngine;
    using UnityEngine.UI;
    using System;

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
        public static bool _quizMode = false;
        public Text buttonValue1;
        public Text buttonValue2;
        public Text buttonValue3;
        public Text buttonValue4;
        public bool QuizMode
        {
            get
            { return _quizMode; }
            set
            { _quizMode = value; }
        }
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject LoadingOverlay;
        public GameObject DuringGameDemo2;
        public GameObject LoadingDemo1;
        public GameObject HomeButton;
        public GameObject LoadingDemo2;
        public GameObject QuestionDemo2;
        public GameObject Demo2Success;
        public GameObject Demo1Success;
        public GameObject Demo1Failure;
        public GameObject StartScreen;
        public GameObject AboutScreen;
        public GameObject Quiz;
        //public GameObject mainSlider;
        public PawnManipulator manipulator;
        public SunMovement sunMovement;

        public CanvasGroup canvasGroupDemo1, canvasGroupDemo2;
        public AugmentedImageVisualizer visualizer = null;
        private Dictionary<int, AugmentedImageVisualizer> _visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();
        public List<AugmentedImage> _tempAugmentedImages = new List<AugmentedImage>();
        [HideInInspector]
        public int counter = 0;
        [HideInInspector]
        public bool _demo2;
        [HideInInspector]
        public bool Demo2
        {
            get {return _demo2; }
            set {_demo2 = value;}
        }

        void HideUI(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f; //this makes everything transparent
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        }
        void ShowUI(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            if (canvasGroup == canvasGroupDemo1 && GameObject.FindGameObjectWithTag("amPm"))
            {
                amPm = (Text)FindObjectOfType(typeof(Text));
                amPm.gameObject.SetActive(true);
            }
        }
        public void disableLayers() {
            StartScreen.SetActive(true);
            LoadingDemo1.SetActive(false);
            DuringGameDemo2.SetActive(false);
            Quiz.SetActive(false);
            LoadingDemo2.SetActive(false);
            QuestionDemo2.SetActive(false);
            Demo2Success.SetActive(false);
            Demo1Success.SetActive(false);
            Demo1Failure.SetActive(false);
            AboutScreen.SetActive(false);
            HideUI(canvasGroupDemo1);
            HideUI(canvasGroupDemo2);
            HomeButton.SetActive(false);
        }
        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            disableLayers();

            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }
        private void switchDemos(AugmentedImage image) {
            if (Demo2 && image.Name.Equals("demo1"))
            {
                //remove the object
                _visualizers.Remove(image.DatabaseIndex);
                GameObject.Destroy(visualizer.gameObject);
                //interupt a game
                QuizMode = false;
            }
            else if (!Demo2 && image.Name.Equals("demo2"))
            {
                manipulator.disableObjects();
                //interupt a game
                manipulator.GameMode = false;
            }
        }
        public void Hide()
        {
            LoadingDemo2.SetActive(false);
        }
        public void displayControl() {
            foreach (var visualizer in _visualizers.Values)
            {
                if (visualizer.Image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
                {
                    if (visualizer.Image.Name.Equals("demo1"))
                    {
                        Demo2 = false;
                        LoadingOverlay.SetActive(false);
                        HideUI(canvasGroupDemo2);
                        //disable landscape mode
                       // Screen.autorotateToLandscapeRight = false;
                        //Screen.autorotateToLandscapeLeft = false;
                        if (!clicked1)
                            LoadingDemo1.SetActive(true);
                        else if (clicked1 && !QuizMode)
                            ShowUI(canvasGroupDemo1);
                    }
                    else if (visualizer.Image.Name.Equals("demo2"))
                    {
                        Quiz.SetActive(false);
                        Demo2 = true;
                        manipulator.enableObjects();
                        HideUI(canvasGroupDemo1);
                        LoadingOverlay.SetActive(false);
                        if (!clicked2)
                        {
                            LoadingDemo2.SetActive(true);
                        }
                        else if (clicked2 && !manipulator.GameMode)
                            ShowUI(canvasGroupDemo2);
                        else if (clicked2 && manipulator.GameMode)
                            HideUI(canvasGroupDemo2);
                        //disable landscape mode
                       // Screen.autorotateToLandscapeRight = false;
                       // Screen.autorotateToLandscapeLeft = false;
                        if (manipulator.GameMode)
                        {
                            //the first time user launches the game
                            if (!readyToPlayDemo2)
                            {
                                HideUI(canvasGroupDemo2);
                                Camera.main.cullingMask = ~(1 << 9);
                                QuestionDemo2.SetActive(true);
                            }
                            else if (manipulator.GameWon){
                                Camera.main.cullingMask = -1;
                                DuringGameDemo2.SetActive(false);
                                Demo2Success.SetActive(true);
                            }
                        }
                    }
                    return;
                }
                // LoadingOverlay.SetActive(true);
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
                _visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && visualizer == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (AugmentedImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    _visualizers.Add(image.DatabaseIndex, visualizer);
                }
                //remove not a current visualizer                
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && visualizer != null)
                {
                    switchDemos(image);
                }
                //destroy objects that are not being tracked
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.NotTracking && visualizer != null)
                {
                    _visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }
            displayControl();
        }

        private bool clicked1 = false;
        private bool clicked2 = false;
        private bool readyToPlayDemo2 = false;
        public void onClick1() {
            LoadingDemo1.SetActive(false);
            clicked1 = true;
            ShowUI(canvasGroupDemo1);
        }
        public void onClick2()
        {
            LoadingDemo2.SetActive(false);
            clicked2 = true;
            ShowUI(canvasGroupDemo2);
        }
        public void wantToPlay()
        {
            QuestionDemo2.SetActive(false);
            HideUI(canvasGroupDemo2);
            DuringGameDemo2.SetActive(true);
            readyToPlayDemo2 = true;
            manipulator.GameWon = false;
        }
        public void playAgainDemo2() {
            Camera.main.cullingMask = ~(1 << 9);
            Demo2Success.SetActive(false);
            manipulator.GameWon = false;
            DuringGameDemo2.SetActive(true);
            clicked2 = true;
            manipulator.turnGameMode();
        }
        public void endGameDemo2()
        {
            Camera.main.cullingMask = -1;
            DuringGameDemo2.SetActive(false);
            manipulator.GameMode = false;
            manipulator.GameWon = false;
            ShowUI(canvasGroupDemo2);
            Demo2Success.SetActive(false);
            readyToPlayDemo2 = false;
            manipulator.resetPositions();
        }

        //QUIZ
        //Function to get a random number 
        private static readonly System.Random random = new System.Random();
        private static readonly object syncLock = new object();
        public static int generateRandom(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }

        public string getTimeFromValue(int value) {
            string answer;
            if (value > 6)
                answer = value - 6 + "PM";
            else if (value == 6)
                answer = value + 6 + "PM";
            else
                answer = (value + 6) + "AM";
            return answer;
        }

        public void SetValue(Text buttonValue, int value)
        {
            buttonValue.text = getTimeFromValue(value);
        }
        public string correctVal;
        [HideInInspector]
        public void turnQuizMode() {
            Demo1Success.SetActive(false);
            QuizMode = true;
            HideUI(canvasGroupDemo1);
            Quiz.SetActive(true);
            int val = generateRandom(0,12);
            //Debug.Log("random in controller " + val);
            sunMovement.randomPosition(val);
            correctVal = getTimeFromValue(val);
            HashSet<int> numbers = new HashSet<int>();
            numbers.Add(val);
            while (numbers.Count != 4)
            {
                numbers.Add(generateRandom(0,12));
            }
            List<int> hList = numbers.ToList();
            // Console.WriteLine(String.Join(",", numbers));

            var shuffled = hList.OrderBy(x => Guid.NewGuid()).ToList();
            Console.WriteLine(String.Join(",", shuffled));
            SetValue(buttonValue1, shuffled[0]);
            SetValue(buttonValue2, shuffled[1]);
            SetValue(buttonValue3, shuffled[2]);
            SetValue(buttonValue4, shuffled[3]);
        }
        public void OnClicked(Button button)
        {
            Text aButton = button.GetComponentInChildren<Text>();
            int pressedVal = Int32.Parse(aButton.text.Substring(0, aButton.text.Length - 2));
            int correct = Int32.Parse(correctVal.Substring(0, correctVal.Length - 2));
            if (correct == pressedVal)
            {
                //button.image.color = Color.green;
                Demo1Success.SetActive(true);
                Quiz.SetActive(false);
            }
            else {
                Demo1Failure.SetActive(true);
            }
        }
        public void EndQuiz() {
            Demo1Success.SetActive(false);
            QuizMode = false;
            ShowUI(canvasGroupDemo1);
        }
        public void CloseFailure()
        {
            Demo1Failure.SetActive(false);
        }
        public void CloseQuiz()
        {
            QuizMode = false;
            Demo1Success.SetActive(false);
            Quiz.SetActive(false);
            ShowUI(canvasGroupDemo1);

        }
        public void QuizAgain()
        {
            Quiz.SetActive(true);
            turnQuizMode();
        }
        public void doExitGame()
        {
            Application.Quit();
        }
        public void startPressed() {
            StartScreen.SetActive(false);
            HomeButton.SetActive(true);
        }
        public void aboutPressed()
        {
            AboutScreen.SetActive(true);
        }
        public void HideAbout()
        {
            AboutScreen.SetActive(false);
        }
        public void homePressed() {
            disableLayers();
        }
    }
}
