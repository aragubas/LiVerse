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
    ScrollableList expressionsList;
    Label characterNameLabel;
    AudioCaptureDevicePanel audioCaptureDevicePanel;

    bool characterFullView = false;

    public MainScreen(ScreenManager screenManager) : base(screenManager) {
      WindowRoot = new();

      HeaderBar = new() { DockType = DockFillContainerDockType.Left };
      centerSplit = new() { DockType = DockFillContainerDockType.Left };
      // Create CharacterRenderer
      characterRenderer = new();

      characterNameLabel = new("{character_name}", 21) { Color = Color.Black };
      Button settingsButton = new("Settings");

      HeaderBar.BackgroundRectDrawble = new() { Color = Color.FromNonPremultiplied(249, 249, 249, 255), IsFilled = true };
      HeaderBar.DockElement = settingsButton;
      HeaderBar.FillElement = characterNameLabel;

      centerCharacterSplit = new() { DockType = DockFillContainerDockType.Bottom, Margin = new(6) };
      centerCharacterSplit.FillElement = characterRenderer;
      expressionsList = new() { ListDirection = ScrollableListDirection.Horizontal, Gap = 6 };

      expressionsList.Elements.Add(new Label("Test Label 1"));
      expressionsList.Elements.Add(new Label("Test Label 2"));

      centerCharacterSplit.DockElement = expressionsList;

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
      UIRoot.UILayers.Add(WindowRoot);
      //settingsScreen.ToggleUILayer();

      // TODO: Remove hardcoded paths
      CharacterStore.CurrentCharacter = new Character("Aragubas", new() { new ExpressionBuilder() {
          Name = "default",
          SpriteCollectionBuilder = new() {
          Idle = @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Fechada.png",
          IdleBlink = @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Fechada.png",
          Speaking = @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Aberta.png",
          SpeakingBlink = @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Aberta.png"
        }
      }});

    }

    private void MicrophoneLevelMeter_MicrophoneLevelUntriggered() {
      characterRenderer.SetSpeaking(false);
    }

    private void MicrophoneLevelMeter_MicrophoneLevelTriggered() {
      characterRenderer.SetSpeaking(true);
    }


    public override void Deattach() { }

    public override void Dispose() {
      CaptureDeviceDriverStore.CaptureDeviceDriver.Dispose();
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!characterFullView) {
        spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
      } else {
        spriteBatch.GraphicsDevice.Clear(SettingsStore.WindowTransparencyColor);
      }

      UIRoot.DrawUILayers(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      // Set CharacterName Label      
      if (CharacterStore.CurrentCharacter != null) {
        characterNameLabel.Text = CharacterStore.CurrentCharacter.Name;

      } else { characterNameLabel.Text = "No character selected"; }

      HeaderBar.Visible = !characterFullView;
      expressionsList.Visible = !characterFullView;
      centerCharacterSplit.Margin = !characterFullView ? Vector2.One * 6 : Vector2.Zero;
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
