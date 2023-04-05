using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.src.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src.Screens
{
    public class MainScreen : IScreen {
    public WindowRoot WindowRoot { get; }
       

    // MainUI Members
    DockFillContainer mainFillContainer = new();
    VerticalLevelTrigger micLevelTrigger;
    DockFillContainer HeaderBar;
    DockFillContainer centerSplit;
    KeyboardState oldState;
    bool characterFullView = false;

    public MainScreen() {
      MicrophoneLevelMeter.Initialize();
      MicrophoneLevelMeter.MicrophoneVolumeLevelUpdate += MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate;

      WindowRoot = new WindowRoot();

      HeaderBar = new DockFillContainer();
      centerSplit = new DockFillContainer();

      Label characterNameLabel = new Label("{character_name}", 22);
      Label placeholderLabel = new Label("Placeholder", 42);
      Button charactersButton = new Button("Characters");
      micLevelTrigger = new VerticalLevelTrigger();
      micLevelTrigger.MaximumValue = 84;

      HeaderBar.DockType = DockFillContainerDockType.Left;
      HeaderBar.DockElement = charactersButton;
      HeaderBar.FillElement = characterNameLabel;

      centerSplit.DockType = DockFillContainerDockType.Left;
      centerSplit.DockElement = micLevelTrigger;
      centerSplit.FillElement = placeholderLabel;

      //HeaderBar.Lines = true;
      mainFillContainer.DockElement = HeaderBar;
      mainFillContainer.FillElement = centerSplit;

      WindowRoot.RootElement = mainFillContainer;

      MicrophoneLevelMeter.MicrophoneTriggerLevelTriggered += MicrophoneLevelMeter_MicrophoneTriggerLevelTriggered;
    }

    private void MicrophoneLevelMeter_MicrophoneTriggerLevelTriggered() {
      Console.WriteLine("Caldo de Pilha");
    }

    private void MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate(double value) {
      micLevelTrigger.CurrentValue = (float)value;
    }


    public void Deattach() {

    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      WindowRoot.Draw(spriteBatch, deltaTime);
    }

    public void Update(double deltaTime) {
      // Sincronize Values
      micLevelTrigger.TriggerLevel = MicrophoneLevelMeter.TriggerLevel;
      micLevelTrigger.MaximumValue = MicrophoneLevelMeter.MaximumLevel;

      WindowRoot.Update(deltaTime);

      // Sincronize Changes
      MicrophoneLevelMeter.TriggerLevel = micLevelTrigger.TriggerLevel;

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
