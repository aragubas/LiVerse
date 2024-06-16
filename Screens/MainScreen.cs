using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.Screens.MainScreenNested;
using LiVerse.Stores;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LiVerse.Screens;

public class MainScreen : ScreenBase {
  SettingsScreen settingsScreen { get; set; }
  UILayer WindowRoot;

  // MainUI Members
  DockFillContainer mainFillContainer = new(null);
  DockFillContainer HeaderBar;
  DockFillContainer centerSplit;
  DockFillContainer centerCharacterSplit;
  CharacterRenderer.CharacterRenderer characterRenderer;
  Label characterNameLabel;

  // Panel Components
  AudioCaptureDevicePanel audioCaptureDevicePanel;
  CharacterExpressionsPanel characterExpressionsPanel;
  NewCharacterExpressionScreen newCharacterExpressionScreen;
  CharactersScreen charactersScreen;

  bool characterFullView = false;

  public MainScreen(ScreenManager screenManager) : base(screenManager) {
    // Layout
    // MainFillContainer
    //   dock: HeaderBar
    //   fill: CenterSplit
    //          dock: AudioCaptureDevicePanel
    //          fill: CenterCharacterSplit
    //                  dock: CharacterExpressionsPanel
    //                  fill: CharacterRenderer

    WindowRoot = new();

    HeaderBar = new(null) { DockDirection = DockDirection.Left };
    centerSplit = new(null) { DockDirection = DockDirection.Left };

    characterNameLabel = new("character name goes here", 20);
    Button settingsButton = new("Settings");
    Button charactersButton = new("Characters");

    DockFillContainer characterNameCharacterDock = new(HeaderBar);
    characterNameCharacterDock.DockDirection = DockDirection.Right;
    characterNameCharacterDock.FillElement = characterNameLabel;
    characterNameCharacterDock.DockElement = charactersButton;

    HeaderBar.BackgroundRectDrawable = new() {
      Color = ColorScheme.ForegroundLevel0,
      FillCenter = true
    };
    HeaderBar.DockElement = settingsButton;
    HeaderBar.FillElement = characterNameCharacterDock;

    // Create CharacterRenderer
    characterRenderer = new();

    // Create CenterCharacterSplit
    centerCharacterSplit = new(centerSplit) { DockDirection = DockDirection.Bottom, Margin = new(8) };
    centerCharacterSplit.FillElement = characterRenderer;

    // Create CharacterExpressionsPanel
    characterExpressionsPanel = new();
    characterExpressionsPanel.OnNewExpressionButtonPressed += () => {
      newCharacterExpressionScreen?.ToggleUILayer();
    };

    centerCharacterSplit.DockElement = characterExpressionsPanel;

    // Create AudioCaptureDevice Panel
    audioCaptureDevicePanel = new();

    // Assign panels to CenterSplit
    centerSplit.DockElement = audioCaptureDevicePanel;
    centerSplit.FillElement = centerCharacterSplit;

    mainFillContainer.DockElement = HeaderBar;
    mainFillContainer.FillElement = centerSplit;

    WindowRoot.RootElement = mainFillContainer;

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelTriggered += MicrophoneLevelMeter_MicrophoneLevelTriggered;
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelUntriggered += MicrophoneLevelMeter_MicrophoneLevelUntriggered;

      CaptureDeviceDriverStore.CaptureDeviceDriver.Initialize();
      CaptureDeviceDriverStore.CaptureDeviceDriver.SetDefaultDevice();
    }

    WindowRoot.KeyboardInputUpdateEvent += FullscreenViewToggle;

    // Adds WindowRoot UILayer
    UIRoot.UILayers.Add(WindowRoot);

    settingsScreen = new();
    settingsButton.Click += settingsScreen.ToggleUILayer;
    //settingsScreen.ToggleUILayer();

    newCharacterExpressionScreen = new();
    //newCharacterExpressionScreen.ToggleUILayer();

    charactersScreen = new();
    charactersButton.Click += charactersScreen.ToggleUILayer;
    charactersScreen.ToggleUILayer();

  }

  private void MicrophoneLevelMeter_MicrophoneLevelUntriggered() {
    characterRenderer.SetSpeaking(false);
  }

  private void MicrophoneLevelMeter_MicrophoneLevelTriggered() {
    characterRenderer.SetSpeaking(true);
  }


  public override void Detach() { }

  public override void Dispose() {
    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      CaptureDeviceDriverStore.CaptureDeviceDriver.Dispose();
    }
  }

  public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
    spriteBatch.GraphicsDevice.Clear(!characterFullView ? ColorScheme.BackgroundLevel0 : SettingsStore.WindowTransparencyColor);

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
