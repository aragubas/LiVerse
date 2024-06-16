using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.Screens.MainScreenNested.SettingsScreenNested;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LiVerse.Screens.MainScreenNested;

public struct SettingsPage {
  public string Title;
  public ControlBase SettingScreen;
}

public struct SettingsCategory {
  public string Title;
  public SettingsPage[] Pages;
}

public class SettingsScreen : NestedScreen {
  public bool Active { get; set; } = true;

  List<SettingsCategory> settingsCategories { get; } = new();
  Button? lastSelectedPage;

  // UI Elements
  ScrollableList categoriesSelectList;
  DockFillContainer settingViewDockFill;
  Label currentPageTitle;

  public SettingsScreen() {
    // Sets Dark Background
    RootLayer.BackgroundRectDrawable = new() { Color = Color.FromNonPremultiplied(0, 0, 0, 127) };

    DockFillContainer dockFill = new(null) {
      Margin = new Vector2(48),
      DockDirection = DockDirection.Left
    };
    DockFillContainer titleDockFill = new(dockFill) {
      DockDirection = DockDirection.Right
    };

    Button exitButton = new(" X ");
    exitButton.Click += ToggleUILayer;

    categoriesSelectList = new(dockFill) { Gap = 2 };
    settingViewDockFill = new(dockFill);
    currentPageTitle = new Label("No page selected", 28, "Ubuntu") {
      Color = ColorScheme.TextNormal
    };

    titleDockFill.FillElement = currentPageTitle;
    titleDockFill.DockElement = exitButton;

    settingViewDockFill.BackgroundRectDrawable = new() {
      Color = ColorScheme.ForegroundLevel0
    };
    settingViewDockFill.DockElement = titleDockFill;

    dockFill.BackgroundRectDrawable = new() {
      Color = ColorScheme.ForegroundLevel1
    };
    dockFill.DockElement = categoriesSelectList;
    dockFill.FillElement = settingViewDockFill;

    RootLayer.RootElement = dockFill;
    RootLayer.KeyboardInputUpdateEvent += UIRootLayer_KeyboardInputUpdateEvent;

    LoadDefaultSettingsPages();
  }

  void LoadDefaultSettingsPages() {
    settingsCategories.Clear();

    SettingsCategory generalCategory = new();
    List<SettingsPage> generalCategoryPages = new();
    SettingsPage audioSettingsPage = new() { Title = "Audio", SettingScreen = new AudioSettingsScreen() };
    SettingsPage graphicsSettingsPage = new() { Title = "Graphics", SettingScreen = new GraphicsSettingsScreen() };

    generalCategoryPages.Add(audioSettingsPage);
    generalCategoryPages.Add(graphicsSettingsPage);

    generalCategory.Title = "General";
    generalCategory.Pages = generalCategoryPages.ToArray();

    settingsCategories.Add(generalCategory);

    BuildSettings();
  }

  void BuildSettings() {
    categoriesSelectList.Elements.Clear();

    foreach (var category in settingsCategories) {
      Label categoryTitle = new(category.Title, 26, "Ubuntu") {
        Margin = new(8)
      };

      categoriesSelectList.Elements.Add(categoryTitle);

      foreach (var page in category.Pages) {
        Button settingsPageButton = new(page.Title, buttonStyle: ButtonStyle.Selectable);
        settingsPageButton.Label.HorizontalAlignment = LabelHorizontalAlignment.Left;
        settingsPageButton.Click += new Action(() => { SelectCategory(settingsPageButton, page); });

        // Select Graphics category as the default
        if (settingsPageButton.Label.Text == "Graphics") {
          SelectCategory(settingsPageButton, page);
        }

        categoriesSelectList.Elements.Add(settingsPageButton);
      }
    }
  }

  void SelectCategory(Button sender, SettingsPage page) {
    settingViewDockFill.FillElement = page.SettingScreen;
    settingViewDockFill.FillElement.Margin = new(8);
    currentPageTitle.Text = page.Title;

    if (lastSelectedPage != null) lastSelectedPage.IsSelected = false;
    sender.IsSelected = true;
    lastSelectedPage = sender;
  }

  private void UIRootLayer_KeyboardInputUpdateEvent(AnaBanUI.Events.KeyboardEvent obj) {
    if (obj.NewKeyboardState.IsKeyUp(Keys.Escape) && obj.OldKeyboardState.IsKeyDown(Keys.Escape)) {
      ToggleUILayer();
    }
  }

  public void ToggleUILayer() {
    if (UIRoot.UILayers.Contains(RootLayer)) {
      UIRoot.UILayers.Remove(RootLayer);
      return;
    }

    UIRoot.UILayers.Add(RootLayer);
  }
}
