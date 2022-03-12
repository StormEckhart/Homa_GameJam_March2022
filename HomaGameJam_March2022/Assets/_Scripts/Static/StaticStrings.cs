using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStrings
{
    //All strings related to names
    public class Names
    {
        //Player related names
        public static string Ball = "Ball (Modelling Clay)";

        public static string CameraView = "OnSuccessOrFailureCameraView";


        //Level related names
        public static string SpawnPoint = "Spawn";

        public static string Ground = "Ground";
        public static string MovingGroup = "Moving";
        public static string DecorationGroup = "Decoration";

        public static string FXsOnSuccess = "FXs OnSuccess";


        //UI related names
        public static string WelcomeScreen = "WelcomeScreen";
        public static string SuccessScreen = "SuccessScreen";
        public static string FailureScreen = "FailureScreen";

        public static string InGameScreen = "InGame";

        public static string InGamePointsText = "CurrentPointsText";
        public static string SuccessPointsText = "TotalPointsText";
    }

    //All strings related to tags
    public class Tags
    {
        //Trigger related tags
        public static string Collectible = "Collectible";

        public static string Obstacle = "Obstacle";

        public static string FinishLine = "End";
    }

    //All strings related to animator parameters
    public class Animator
    {
        //Player animation related animator strings
        public static string IsPushingBool = "IsPushing";
        public static string IsRunningBool = "IsRunning";


        public static string DefeatTrigger = "Lost";
        public static string DanceTrigger = "Won";
        public static string ResetTrigger = "Reset";
    }
}
