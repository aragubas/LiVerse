﻿using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.CharacterRenderer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LiVerse.Screens
{
    public class MainScreen : IScreen {
    public WindowRoot WindowRoot { get; }
       

    // MainUI Members
    DockFillContainer mainFillContainer = new();
    VerticalLevelTrigger micLevelTrigger;
    VerticalLevelTrigger levelDelayTrigger;
    DockFillContainer HeaderBar;
    DockFillContainer centerSplit;
    KeyboardState oldState;
    bool characterFullView = false;
    CharacterRenderer.CharacterRenderer characterRenderer;

    DockFillContainer sideFillContainer;
    SolidColorRectangle speakingIndicatorSolidColorRect;
    Label speakingIndicatorLabel;
    Label characterNameLabel;
    static readonly Color speakingIndicatorColor = Color.FromNonPremultiplied(8, 7, 5, 50);
    static readonly Color speakingIndicatorActiveColor = Color.FromNonPremultiplied(230, 50, 75, 255);
    static readonly Color speakingIndicatorLabelColor = Color.FromNonPremultiplied(255, 255, 255, 50);
    static readonly Color speakingIndicatorActiveLabelColor = Color.FromNonPremultiplied(255, 255, 255, 255);

    public MainScreen() {
      MicrophoneLevelMeter.Initialize();
      MicrophoneLevelMeter.MicrophoneVolumeLevelUpdate += MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate;

      WindowRoot = new WindowRoot();

      HeaderBar = new DockFillContainer();
      centerSplit = new DockFillContainer();
      // Create CharacterRenderer
      characterRenderer = new CharacterRenderer.CharacterRenderer();

      characterNameLabel = new Label("{character_name}", 26);
      Button charactersButton = new Button("Characters");
      micLevelTrigger = new VerticalLevelTrigger();
      levelDelayTrigger = new VerticalLevelTrigger();
      micLevelTrigger.MaximumValue = 84;
      micLevelTrigger.ShowPeaks = true;
      levelDelayTrigger.MaximumValue = 1;

      SideBySideContainer sideBySide = new SideBySideContainer();
      sideFillContainer = new DockFillContainer();
      sideFillContainer.Margin = 4f;

      sideBySide.Elements.Add(micLevelTrigger);
      sideBySide.Elements.Add(levelDelayTrigger);
      sideBySide.Gap = 4f;

      sideFillContainer.DockType = DockFillContainerDockType.Bottom;
      sideFillContainer.FillElement = sideBySide;

      speakingIndicatorLabel = new Label("Active", 22);
      speakingIndicatorLabel.Color = speakingIndicatorLabelColor;
      speakingIndicatorSolidColorRect = new SolidColorRectangle(speakingIndicatorLabel);
      speakingIndicatorSolidColorRect.Margin = 4f;
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;

      sideFillContainer.DockElement = speakingIndicatorSolidColorRect;

      HeaderBar.DockType = DockFillContainerDockType.Left;
      HeaderBar.DockElement = charactersButton;
      HeaderBar.FillElement = characterNameLabel;
      HeaderBar.Margin = 4f;

      centerSplit.DockType = DockFillContainerDockType.Left;
      centerSplit.DockElement = sideFillContainer;
      centerSplit.FillElement = characterRenderer;

      //HeaderBar.Lines = true;
      mainFillContainer.DockElement = HeaderBar;
      mainFillContainer.FillElement = centerSplit;
      mainFillContainer.Margin = 4f;

      WindowRoot.RootElement = mainFillContainer;

      MicrophoneLevelMeter.CharacterStartSpeaking += MicrophoneLevelMeter_CharacterStartSpeaking;
      MicrophoneLevelMeter.CharacterStopSpeaking += MicrophoneLevelMeter_CharacterStopSpeaking;
    }

    private void MicrophoneLevelMeter_CharacterStopSpeaking() {
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;
      speakingIndicatorLabel.Color = speakingIndicatorLabelColor;
    }

    private void MicrophoneLevelMeter_CharacterStartSpeaking() {
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorActiveColor;
      speakingIndicatorLabel.Color = speakingIndicatorActiveLabelColor;
    }

    private void MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate(double value) {
      micLevelTrigger.CurrentValue = (float)value;
    }


    public void Deattach() {

    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!characterFullView) {
        spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
      }else {
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);
      }
      
      WindowRoot.Draw(spriteBatch, deltaTime);
    }

    public void Update(double deltaTime) {
      MicrophoneLevelMeter.Update(deltaTime);

      // Set Values
      levelDelayTrigger.CurrentValue = (float)MicrophoneLevelMeter.ActivationDelay;

      // Sincronize Values
      micLevelTrigger.TriggerLevel = MicrophoneLevelMeter.TriggerLevel;
      micLevelTrigger.MaximumValue = MicrophoneLevelMeter.MaximumLevel;
      levelDelayTrigger.TriggerLevel = MicrophoneLevelMeter.ActivationDelayTrigger;

      WindowRoot.Update(deltaTime);

      // Set CharacterName Label
      if (characterRenderer.CurrentCharacter != null) {
        characterNameLabel.Text = characterRenderer.CurrentCharacter.Name;
      }else { characterNameLabel.Text = "No character selected"; }
      

      // Sincronize Changes
      MicrophoneLevelMeter.TriggerLevel = micLevelTrigger.TriggerLevel;
      MicrophoneLevelMeter.ActivationDelayTrigger = levelDelayTrigger.TriggerLevel;

      HeaderBar.Visible = !characterFullView;
      if (centerSplit.DockElement != null) centerSplit.DockElement.Visible = !characterFullView;

      // Check if toggle key has been pressed
      if (Keyboard.GetState().IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape)) {
        characterFullView = !characterFullView;
      }

      oldState = Keyboard.GetState();
    }
  }
}