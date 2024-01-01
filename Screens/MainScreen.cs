using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.CharacterRenderer;
using LiVerse.Screens.MainScreenNested;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace LiVerse.Screens {
  public class MainScreen : ScreenBase {
    SettingsScreen settingsScreen { get; set; }
    UILayer WindowRoot;

    // MainUI Members
    DockFillContainer mainFillContainer = new();
    DockFillContainer HeaderBar;
    DockFillContainer centerSplit;
    DockFillContainer centerCharacterSplit;
    CharacterRenderer.CharacterRenderer characterRenderer;
    Label characterNameLabel;

    // Panel Components
    AudioCaptureDevicePanel audioCaptureDevicePanel;
    CharacterExpressionsPanel characterExpressionsPanel;
    NewCharacterExpressionScreen newCharacterExpressionScreen;

    bool characterFullView = false;

    public MainScreen(ScreenManager screenManager) : base(screenManager) {
      WindowRoot = new();

      HeaderBar = new() { DockType = DockFillContainerDockType.Left };
      centerSplit = new() { DockType = DockFillContainerDockType.Left };

      characterNameLabel = new("character name goes here", 20) { Color = Color.Black };
      Button settingsButton = new("Settings");

      HeaderBar.BackgroundRectDrawable = new() { Color = Color.FromNonPremultiplied(249, 249, 249, 255), FillCenter = true };
      HeaderBar.DockElement = settingsButton;
      HeaderBar.FillElement = characterNameLabel;

      // Create CharacterRenderer
      characterRenderer = new();

      centerCharacterSplit = new() { DockType = DockFillContainerDockType.Bottom, Margin = new(6) };
      centerCharacterSplit.FillElement = characterRenderer;

      characterExpressionsPanel = new();

      centerCharacterSplit.DockElement = characterExpressionsPanel;

      audioCaptureDevicePanel = new();

      centerSplit.DockElement = audioCaptureDevicePanel;
      centerSplit.FillElement = centerCharacterSplit;

      mainFillContainer.DockElement = HeaderBar;
      mainFillContainer.FillElement = centerSplit;

      WindowRoot.RootElement = mainFillContainer;

      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelTriggered += MicrophoneLevelMeter_MicrophoneLevelTriggered;
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelUntriggered += MicrophoneLevelMeter_MicrophoneLevelUntriggered;

      CaptureDeviceDriverStore.CaptureDeviceDriver.Initialize();
      CaptureDeviceDriverStore.CaptureDeviceDriver.SetDefaultDevice();

      WindowRoot.KeyboardInputUpdateEvent += FullscreenViewToggle;

      // Registers WindowRoot UILayer
      settingsScreen = new();
      settingsButton.Click += settingsScreen.ToggleUILayer;
      //settingsScreen.ToggleUILayer();

      newCharacterExpressionScreen = new();
      //newCharacterExpressionScreen.ToggleUILayer();

      UIRoot.UILayers.Add(WindowRoot);
    }

    private void MicrophoneLevelMeter_MicrophoneLevelUntriggered() {
      characterRenderer.SetSpeaking(false);
    }

    private void MicrophoneLevelMeter_MicrophoneLevelTriggered() {
      characterRenderer.SetSpeaking(true);
    }


    public override void Detach() { }

    public override void Dispose() {
      CaptureDeviceDriverStore.CaptureDeviceDriver.Dispose();
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.GraphicsDevice.Clear(!characterFullView ? Color.CornflowerBlue : SettingsStore.WindowTransparencyColor);

      UIRoot.DrawUILayers(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      // Set CharacterName Label      
      if (CharacterStore.CurrentCharacter != null) {
        characterNameLabel.Text = CharacterStore.CurrentCharacter.Name;

      } else { characterNameLabel.Text = "No character selected"; }

      HeaderBar.Visible = !characterFullView;
      if (centerCharacterSplit.DockElement != null) centerCharacterSplit.DockElement.Visible = !characterFullView;
      //centerCharacterSplit.Margin = !characterFullView ? Vector2.One * 6 : Vector2.Zero;
      if (centerSplit.DockElement != null) centerSplit.DockElement.Visible = !characterFullView;
    }

    void FullscreenViewToggle(KeyboardEvent keyboardEvent) {
      // Check if toggle key has been pressed
      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Escape) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Escape)) {
        characterFullView = !characterFullView;
      }
    }

  }
}
