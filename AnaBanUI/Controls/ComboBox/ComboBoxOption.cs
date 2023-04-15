namespace LiVerse.AnaBanUI.Controls.ComboBox {
  public struct ComboBoxOption {
    public string OptionText { get; set; }
    public object? ExtraData { get; set; }

    public ComboBoxOption(string optionText, object? extraData = null) {
      OptionText = optionText;
      ExtraData = extraData;
    }
  }
}
