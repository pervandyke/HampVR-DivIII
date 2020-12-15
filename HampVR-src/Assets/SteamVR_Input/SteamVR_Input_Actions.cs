//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Boolean p_flight1_Accelerate;
        
        private static SteamVR_Action_Boolean p_flight1_LeftFire;
        
        private static SteamVR_Action_Boolean p_flight1_Recenter_Headset_Zero;
        
        private static SteamVR_Action_Boolean p_flight1_Decelerate;
        
        private static SteamVR_Action_Pose p_flight1_poseTest;
        
        private static SteamVR_Action_Boolean p_flight1_RightFire;
        
        private static SteamVR_Action_Boolean p_flight1_LeftSelect;
        
        private static SteamVR_Action_Boolean p_flight1_RightSelect;
        
        public static SteamVR_Action_Boolean flight1_Accelerate
        {
            get
            {
                return SteamVR_Actions.p_flight1_Accelerate.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_LeftFire
        {
            get
            {
                return SteamVR_Actions.p_flight1_LeftFire.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_Recenter_Headset_Zero
        {
            get
            {
                return SteamVR_Actions.p_flight1_Recenter_Headset_Zero.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_Decelerate
        {
            get
            {
                return SteamVR_Actions.p_flight1_Decelerate.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Pose flight1_poseTest
        {
            get
            {
                return SteamVR_Actions.p_flight1_poseTest.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_RightFire
        {
            get
            {
                return SteamVR_Actions.p_flight1_RightFire.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_LeftSelect
        {
            get
            {
                return SteamVR_Actions.p_flight1_LeftSelect.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean flight1_RightSelect
        {
            get
            {
                return SteamVR_Actions.p_flight1_RightSelect.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.flight1_Accelerate,
                    SteamVR_Actions.flight1_LeftFire,
                    SteamVR_Actions.flight1_Recenter_Headset_Zero,
                    SteamVR_Actions.flight1_Decelerate,
                    SteamVR_Actions.flight1_poseTest,
                    SteamVR_Actions.flight1_RightFire,
                    SteamVR_Actions.flight1_LeftSelect,
                    SteamVR_Actions.flight1_RightSelect};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.flight1_Accelerate,
                    SteamVR_Actions.flight1_LeftFire,
                    SteamVR_Actions.flight1_Recenter_Headset_Zero,
                    SteamVR_Actions.flight1_Decelerate,
                    SteamVR_Actions.flight1_poseTest,
                    SteamVR_Actions.flight1_RightFire,
                    SteamVR_Actions.flight1_LeftSelect,
                    SteamVR_Actions.flight1_RightSelect};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[0];
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[0];
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.flight1_poseTest};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.flight1_Accelerate,
                    SteamVR_Actions.flight1_LeftFire,
                    SteamVR_Actions.flight1_Recenter_Headset_Zero,
                    SteamVR_Actions.flight1_Decelerate,
                    SteamVR_Actions.flight1_RightFire,
                    SteamVR_Actions.flight1_LeftSelect,
                    SteamVR_Actions.flight1_RightSelect};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[0];
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[0];
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[0];
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.flight1_Accelerate,
                    SteamVR_Actions.flight1_LeftFire,
                    SteamVR_Actions.flight1_Recenter_Headset_Zero,
                    SteamVR_Actions.flight1_Decelerate,
                    SteamVR_Actions.flight1_RightFire,
                    SteamVR_Actions.flight1_LeftSelect,
                    SteamVR_Actions.flight1_RightSelect};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_flight1_Accelerate = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/Accelerate")));
            SteamVR_Actions.p_flight1_LeftFire = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/LeftFire")));
            SteamVR_Actions.p_flight1_Recenter_Headset_Zero = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/Recenter_Headset_Zero")));
            SteamVR_Actions.p_flight1_Decelerate = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/Decelerate")));
            SteamVR_Actions.p_flight1_poseTest = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/Flight1/in/poseTest")));
            SteamVR_Actions.p_flight1_RightFire = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/RightFire")));
            SteamVR_Actions.p_flight1_LeftSelect = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/LeftSelect")));
            SteamVR_Actions.p_flight1_RightSelect = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Flight1/in/RightSelect")));
        }
    }
}
