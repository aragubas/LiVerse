using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using LiVerse.CaptureDeviceDriver;
using LiVerse.CaptureDeviceDriver.WasapiCaptureDevice;
using LiVerse.CharacterRenderer;
using LiVerse.Screens.MainScreenNested;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LiVerse.Screens
{
    public class MainScreen : ScreenBase {
    SettingsScreen settingsScreen { get; set; }
    UILayer WindowRoot;

    // MainUI Members
    DockFillContainer mainFillContainer = new();
    VerticalLevelTrigger micLevelTrigger;
    VerticalLevelTrigger levelDelayTrigger;
    DockFillContainer HeaderBar;
    DockFillContainer centerSplit;
    DockFillContainer centerCharacterSplit;
    CharacterRenderer.CharacterRenderer characterRenderer;
    DockFillContainer sideFillContainer;
    SolidColorRectangle speakingIndicatorSolidColorRect;
    Label speakingIndicatorLabel;
    Label characterNameLabel;

    bool characterFullView = false;

    // Static ReadOnly Fields
    static readonly Color speakingIndicatorColor = Color.FromNonPremultiplied(8, 7, 5, 50);
    static readonly Color speakingIndicatorActiveColor = Color.FromNonPremultiplied(230, 50, 75, 255);
    static readonly Color speakingIndicatorLabelColor = Color.FromNonPremultiplied(255, 255, 255, 50);
    static readonly Color speakingIndicatorActiveLabelColor = Color.FromNonPremultiplied(255, 255, 255, 255);

    public MainScreen(ScreenManager screenManager) : base(screenManager) {
      WindowRoot = new();

      HeaderBar = new();
      centerSplit = new();
      // Create CharacterRenderer
      characterRenderer = new();

      characterNameLabel = new("{character_name}", 21) { Color = Color.Black };
      Button settingsButton = new("Settings", 21);

      micLevelTrigger = new() { ShowPeaks = true, MaximumValue = 84 };
      levelDelayTrigger = new() { MaximumValue = 1 };

      SideBySideContainer sideBySide = new() { Gap = 4f };
      sideFillContainer = new() { DockType = DockFillContainerDockType.Bottom, Margin = new(6), Gap = 6, FillElement = sideBySide };

      sideBySide.Elements.Add(micLevelTrigger);
      sideBySide.Elements.Add(levelDelayTrigger);

      speakingIndicatorLabel = new("Active", 21);
      speakingIndicatorLabel.Color = speakingIndicatorLabelColor;
      speakingIndicatorSolidColorRect = new(speakingIndicatorLabel);
      //speakingIndicatorSolidColorRect.Margin = new(4);
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;
      
      sideFillContainer.DockElement = speakingIndicatorSolidColorRect;

      HeaderBar.DockType = DockFillContainerDockType.Left;
      HeaderBar.BackgroundRectDrawble = new() { Color = Color.FromNonPremultiplied(249, 249, 249, 255), IsFilled = true };
      HeaderBar.DockElement = settingsButton;
      HeaderBar.FillElement = characterNameLabel;

      centerCharacterSplit = new() { DockType = DockFillContainerDockType.Bottom };
      centerCharacterSplit.FillElement = characterRenderer;

      centerSplit.DockType = DockFillContainerDockType.Left;
      centerSplit.DockElement = sideFillContainer;
      centerSplit.FillElement = centerCharacterSplit;

      //HeaderBar.Lines = true;
      mainFillContainer.DockElement = HeaderBar;
      mainFillContainer.FillElement = centerSplit;

      WindowRoot.RootElement = mainFillContainer;

      CaptureDeviceDriverManager.CaptureDeviceDriver.MicrophoneLevelTriggered += MicrophoneLevelMeter_CharacterStartSpeaking;
      CaptureDeviceDriverManager.CaptureDeviceDriver.MicrophoneLevelUntriggered += MicrophoneLevelMeter_CharacterStopSpeaking;
      CaptureDeviceDriverManager.CaptureDeviceDriver.MicrophoneVolumeLevelUpdated += MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate;

      CaptureDeviceDriverManager.CaptureDeviceDriver.Initialize();
      CaptureDeviceDriverManager.CaptureDeviceDriver.SetDefaultDevice();

      WindowRoot.KeyboardInputUpdateEvent += FullscreenViewToggle;

      // Registers WindowRoot UILayer
      settingsScreen = new();
      settingsButton.Click += settingsScreen.ToggleUILayer;
      UIRoot.UILayers.Add(WindowRoot);
      //settingsScreen.ToggleUILayer();

      CharacterStore.LoadCharacter(new Character("Aragubas", new SpriteCollection() {
        Idle = ResourceManager.LoadTexture2DFromFile(@"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Fechada.png"),
        IdleBlink = ResourceManager.LoadTexture2DFromFile(@"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Fechada.png"),
        Speaking = ResourceManager.LoadTexture2DFromFile(@"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Aberta.png"),
        SpeakingBlink = ResourceManager.LoadTexture2DFromFile(@"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Aberta.png")
      }));
    }

    private void MicrophoneLevelMeter_CharacterStopSpeaking() {
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;
      speakingIndicatorLabel.Color = speakingIndicatorLabelColor;

      characterRenderer.SetSpeaking(false);
    }

    private void MicrophoneLevelMeter_CharacterStartSpeaking() {
      speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorActiveColor;
      speakingIndicatorLabel.Color = speakingIndicatorActiveLabelColor;

      characterRenderer.SetSpeaking(true);
    }

    private void MicrophoneLevelMeter_MicrophoneVolumeLevelUpdate(double value) {
      micLevelTrigger.CurrentValue = (float)value;
    }


    public override void Deattach() { }

    public override void Dispose() {
      CaptureDeviceDriverManager.CaptureDeviceDriver.Dispose();
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!characterFullView) {
        spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
      }else {
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);
      }

      UIRoot.DrawUILayers(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      // Set Values
      levelDelayTrigger.CurrentValue = (float)CaptureDeviceDriverManager.CaptureDeviceDriver.ActivationDelay;

      // Sincronize Values
      micLevelTrigger.TriggerLevel = CaptureDeviceDriverManager.CaptureDeviceDriver.TriggerLevel;
      micLevelTrigger.MaximumValue = CaptureDeviceDriverManager.CaptureDeviceDriver.MaximumLevel;
      levelDelayTrigger.TriggerLevel = CaptureDeviceDriverManager.CaptureDeviceDriver.ActivationDelayTrigger;

      UIRoot.UpdateUILayers(deltaTime);

      // Set CharacterName Label      
      if (CharacterStore.CurrentCharacter != null) {
        characterNameLabel.Text = CharacterStore.CurrentCharacter.Name;
      }else { characterNameLabel.Text = "No character selected"; }

      // Sincronize Changes
      CaptureDeviceDriverManager.CaptureDeviceDriver.TriggerLevel = micLevelTrigger.TriggerLevel;
      CaptureDeviceDriverManager.CaptureDeviceDriver.ActivationDelayTrigger = levelDelayTrigger.TriggerLevel;      

      HeaderBar.Visible = !characterFullView;
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
