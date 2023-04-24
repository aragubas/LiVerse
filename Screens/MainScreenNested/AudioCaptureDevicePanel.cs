using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace LiVerse.Screens.MainScreenNested
{
    public class AudioCaptureDevicePanel : ControlBase
    {
        // Static ReadOnly Fields
        static readonly Color speakingIndicatorColor = Color.FromNonPremultiplied(8, 7, 5, 50);
        static readonly Color speakingIndicatorActiveColor = Color.FromNonPremultiplied(230, 50, 75, 255);
        static readonly Color speakingIndicatorLabelColor = Color.FromNonPremultiplied(255, 255, 255, 50);
        static readonly Color speakingIndicatorActiveLabelColor = Color.FromNonPremultiplied(255, 255, 255, 255);

        VerticalLevelTrigger micLevelTrigger;
        VerticalLevelTrigger levelDelayTrigger;
        Label speakingIndicatorLabel;
        DockFillContainer sideFillContainer;
        SolidColorRectangle speakingIndicatorSolidColorRect;

        public AudioCaptureDevicePanel()
        {
            micLevelTrigger = new() { ShowPeaks = true, MaximumValue = 84 };
            levelDelayTrigger = new() { MaximumValue = 1 };

            SideBySideContainer sideBySide = new() { Gap = 4f };
            sideFillContainer = new() { DockType = DockFillContainerDockType.Bottom, Margin = new(6), Gap = 6, FillElement = sideBySide };

            sideBySide.Elements.Add(micLevelTrigger);
            sideBySide.Elements.Add(levelDelayTrigger);

            speakingIndicatorLabel = new("Active", 21) { Color = speakingIndicatorLabelColor };
            speakingIndicatorSolidColorRect = new(speakingIndicatorLabel) { BackgroundColor = speakingIndicatorColor };

            sideFillContainer.DockElement = speakingIndicatorSolidColorRect;

            CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelTriggered += CaptureDeviceDriver_MicrophoneLevelTriggered;
            CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelUntriggered += CaptureDeviceDriver_MicrophoneLevelUntriggered;
            CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneVolumeLevelUpdated += CaptureDeviceDriver_MicrophoneVolumeLevelUpdated;

            // Sincronize Values
            micLevelTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel;
            micLevelTrigger.MaximumValue = CaptureDeviceDriverStore.CaptureDeviceDriver.MaximumLevel;
            levelDelayTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger;
        }

        private void CaptureDeviceDriver_MicrophoneVolumeLevelUpdated(double obj)
        {
            micLevelTrigger.CurrentValue = (float)obj;
        }

        private void CaptureDeviceDriver_MicrophoneLevelUntriggered()
        {
            speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;
            speakingIndicatorLabel.Color = speakingIndicatorLabelColor;
        }

        private void CaptureDeviceDriver_MicrophoneLevelTriggered()
        {
            speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorActiveColor;
            speakingIndicatorLabel.Color = speakingIndicatorActiveLabelColor;
        }

        public override void UpdateUI(double deltaTime)
        {
            FillElement(sideFillContainer);
        }

        public override void DrawElement(SpriteBatch spriteBatch, double deltaTime)
        {
            sideFillContainer.Draw(spriteBatch, deltaTime);
        }

        public override bool InputUpdate(KeyboardEvent keyboardEvent)
        {
            return sideFillContainer.InputUpdate(keyboardEvent);
        }

        public override bool InputUpdate(PointerEvent pointerEvent)
        {
            return sideFillContainer.InputUpdate(pointerEvent);
        }

        public override void Update(double deltaTime)
        {
            // Sincronize Changes
            CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel = micLevelTrigger.TriggerLevel;
            CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger = levelDelayTrigger.TriggerLevel;

            sideFillContainer.Update(deltaTime);

            // Set Values
            levelDelayTrigger.CurrentValue = (float)CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelay;

            // Sincronize Values
            micLevelTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel;
            micLevelTrigger.MaximumValue = CaptureDeviceDriverStore.CaptureDeviceDriver.MaximumLevel;
            levelDelayTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger;
        }
    }
}
