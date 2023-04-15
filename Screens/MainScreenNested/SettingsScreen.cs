using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.Screens.MainScreenNested {  
  public struct SettingsPage {
    public string Title;
    public ControlBase SettingScreen;
  }

  public struct SettingsCategory {
    public string Title;
    public SettingsPage[] Pages;
  }
  
  public class SettingsScreen {
    public bool Active { get; set; } = true;
    UILayer UIRootLayer;

    List<SettingsCategory> settingsCategories { get; } = new();
    Button? lastSelectedPage;

    // UI Elements
    ScrollableList categoriesSelectList;
    DockFillContainer settingViewDockFill;
    Label currentPageTitle;

    void LoadDefaultSettingsPages() {
      settingsCategories.Clear();

      SettingsCategory generalCategory = new();
      List<SettingsPage> generalCategoryPages = new();
      SettingsPage audioSettingsPage = new() { Title = "Audio", SettingScreen = new AudioSettingsScreen() };
      SettingsPage graphicsSettingsPage = new() { Title = "Graphics", SettingScreen = new AudioSettingsScreen() };

      generalCategoryPages.Add(audioSettingsPage);
      generalCategoryPages.Add(graphicsSettingsPage);

      generalCategory.Title = "General";
      generalCategory.Pages = generalCategoryPages.ToArray();

      settingsCategories.Add(generalCategory);

      BuildSettings();
    }

    void BuildSettings() {
      categoriesSelectList.Elements.Clear();

      bool isFirstPage = true;
      foreach (var category in settingsCategories) {
        Label categoryTitle = new(category.Title, 26, "Ubuntu") { Margin = new(8) };
        categoryTitle.Color = Color.Black;

        categoriesSelectList.Elements.Add(categoryTitle);

        foreach(var page in category.Pages) {
          Button settingsPageButton = new(page.Title, buttonStyle: ButtonStyle.Selectable);
          settingsPageButton.Label.TextHorizontalAlignment = LabelTextHorizontalAlignment.Left;
          settingsPageButton.Click += new Action(() => { SelectCategory(settingsPageButton, page); });

          if (isFirstPage) {
            isFirstPage = false;
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

    public SettingsScreen() {
      UIRootLayer = new UILayer() { BackgroundRectDrawable = new() { Color = Color.Black, Opacity = 127 } };

      DockFillContainer dockFill = new() { Margin = new Vector2(48), DockType = DockFillContainerDockType.Left };
      DockFillContainer titleDockFill = new() { DockType = DockFillContainerDockType.Right };
      Button exitButton = new(" X ");
      exitButton.Click += ToggleUILayer;
      categoriesSelectList = new() { Gap = 2 };
      settingViewDockFill = new();
      currentPageTitle = new Label("No page selected", 28, "Ubuntu") { Color = Color.Black };

      titleDockFill.FillElement = currentPageTitle;
      titleDockFill.DockElement = exitButton;

      settingViewDockFill.BackgroundRectDrawble = new() { Color = Color.White };
      settingViewDockFill.DockElement = titleDockFill;

      dockFill.BackgroundRectDrawble = new() { Color = Color.FromNonPremultiplied(240, 240, 240, 255) };
      dockFill.ForegroundRectDrawble = new() { Color = Color.FromNonPremultiplied(194, 194, 194, 255), IsFilled = false };
      dockFill.DockElement = categoriesSelectList;
      dockFill.FillElement = settingViewDockFill;

      UIRootLayer.RootElement = dockFill;

      LoadDefaultSettingsPages();
    }

    public void ToggleUILayer() {
      if (UIRoot.UILayers.Contains(UIRootLayer)) {
        UIRoot.UILayers.Remove(UIRootLayer);
        return;
      }

      UIRoot.UILayers.Add(UIRootLayer);
    }
  }
}
