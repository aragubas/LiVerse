﻿using LiVerse.AnaBanUI;
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
    public event Action Close;
    UILayer UIRootLayer;

    List<SettingsCategory> settingsCategories = new();
    Button? lastSelectedPage;

    // UI Elements
    ScrollableList categoriesSelectList;
    MarginContainer marginContainer;
    MarginContainer currentPageMarginContainer;
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
        Label categoryTitle = new(category.Title, 26, "Ubuntu");
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
      currentPageMarginContainer.FillElement = page.SettingScreen;
      currentPageTitle.Text = page.Title;

      if (lastSelectedPage != null) lastSelectedPage.IsSelected = false;
      sender.IsSelected = true;
      lastSelectedPage = sender;
    }

    public SettingsScreen() {
      UIRootLayer = new UILayer();

      marginContainer = new(16, backgroundRectDrable: new RectangleDrawable() { Color = Color.FromNonPremultiplied(0, 0, 0, 127) });
      currentPageMarginContainer = new(8);
      DockFillContainer dockFill = new() { Margin = 1, DockType = DockFillContainerDockType.Left };
      DockFillContainer titleDockFill = new() { Margin = 1, DockType = DockFillContainerDockType.Right };
      Button exitButton = new(" X ");
      exitButton.Click += new Action(() => { Close?.Invoke(); });
      categoriesSelectList = new() { Gap = 2 };
      settingViewDockFill = new();
      currentPageTitle = new Label("No page selected", 28, "Ubuntu") { Color = Color.Black };

      titleDockFill.FillElement = currentPageTitle;
      titleDockFill.DockElement = exitButton;

      settingViewDockFill.BackgroundRectDrawble = new() { Color = Color.White };
      settingViewDockFill.DockElement = titleDockFill;
      settingViewDockFill.FillElement = currentPageMarginContainer;

      dockFill.BackgroundRectDrawble = new() { Color = Color.FromNonPremultiplied(240, 240, 240, 255) };
      dockFill.ForegroundRectDrawble = new() { Color = Color.FromNonPremultiplied(194, 194, 194, 255), IsFilled = false };
      dockFill.DockElement = categoriesSelectList;
      dockFill.FillElement = new MarginContainer(1, dockElement: settingViewDockFill);

      marginContainer.FillElement = dockFill;

      UIRootLayer.RootElement = marginContainer;

      LoadDefaultSettingsPages();
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {     
      UIRootLayer.Draw(spriteBatch, deltaTime);
    }

    public void Update(double deltaTime) {
      UIRootLayer.Update(deltaTime);

      
    }
  }
}
