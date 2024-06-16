using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;

namespace LiVerse.Screens.MainScreenNested;

public class CharactersScreen : NestedScreen {

  public CharactersScreen() {
    // Sets Dark Background
    RootLayer.BackgroundRectDrawable = new() { Color = Color.FromNonPremultiplied(0, 0, 0, 127) };

    //Layout
    //mainFillContainer
    //  dock:
    //    titlebarFillContainer:
    //      dock:
    //        closeButton
    //      fill:
    //        titleLabel
    //  fill:
    //    mainSplitContainer
    //    
    //

    DockFillContainer mainFillContainer = new(null) {
      Margin = new Vector2(40),
      BackgroundRectDrawable = new() {
        Color = ColorScheme.ForegroundLevel0
      }
    };


    #region Titlebar
    
    DockFillContainer titlebarFillContainer = new(mainFillContainer) { DockDirection = DockDirection.Right };
    Button closeButton = new(titlebarFillContainer, " X ");
    Label titleLabel = new(titlebarFillContainer, "Characters");

    closeButton.Click += ToggleUILayer;

    titlebarFillContainer.DockElement = closeButton;
    titlebarFillContainer.FillElement = titleLabel;

    #endregion
    mainFillContainer.DockElement = titlebarFillContainer;

    // MainSplit container
    DockFillContainer mainSplitContainer = new(mainFillContainer) {
      DockDirection = DockDirection.Left
    };

    
    #region Left Panel
    DockFillContainer leftPanel = new(mainSplitContainer) { DockDirection = DockDirection.Top }; 
    ScrollableList charactersList = new(leftPanel);
    Button newCharacterButton = new(leftPanel, "New Character");

    leftPanel.DockElement = newCharacterButton;
    leftPanel.FillElement = charactersList;

    #endregion
    mainSplitContainer.DockElement = leftPanel;


    Label test = new(mainSplitContainer, "Test");
    mainSplitContainer.FillElement = test;



    // Adds MainSplitContainer to mainFillContainer
    mainFillContainer.FillElement = mainSplitContainer;
  
    RootLayer.RootElement = mainFillContainer;
  }
}
