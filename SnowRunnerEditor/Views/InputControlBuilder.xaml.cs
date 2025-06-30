using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SnowRunnerEditor.Src.Controls;
using SnowRunnerEditor.Views.CustomContorls;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SnowRunnerEditor.Views
{
    public sealed partial class InputControlBuilder : UserControl
    {
        public UIElement this[string name] => Elements[name];

        public InputControlBuilder()
        {
            InitializeComponent();
            Elements = new Dictionary<string, UIElement>().ToImmutableDictionary();
        }

        public ImmutableDictionary<string, UIElement> Elements { get; private set; } 

        public ItemSelector AddItemSelector(string name, string label, string buttonText, bool required, ItemSelector.PickTypeSelector selectType, string fileExtensions)
        {
            ItemSelector selector = new()
            {                
                Name = name,
                Label = label,
                Text = buttonText,
                Required = required,
                PickType = selectType,
                FileExtensions = fileExtensions,
                Margin = new Thickness(0, 5, 0, 5),
            };

            MainPanel.Children.Add(selector);
            Elements = Elements.Add(name, selector);

            return selector;
        }

        public ExtendedTxtBox AddExtendedTextBox(string name, string label, bool required)
        {
            ExtendedTxtBox box = new()
            {
                Name = name,
                Label = label,
                Required = required,
                Margin = new(0, 5, 0, 5),
            };

            MainPanel.Children.Add(box);
            Elements = Elements.Add(name, box);

            return box;
        }

        public ExtendedCheckBox AddExtendedCheckBox(string name, string label)
        {
            ExtendedCheckBox box = new()
            {
                Name = name,
                Label = label,
                Margin = new(5, 5, 0, 5),
            };

            MainPanel.Children.Add(box);
            Elements = Elements.Add(name, box);

            return box;
        }

        public TextBlock AddFooter(string name, string content)
        {
            TextBlock footer = new()
            {
                Name = name,
                Text = content,
                TextWrapping = TextWrapping.WrapWholeWords,
                Margin = new(0, 20, 0, 0)
            };
            
            MainPanel.Children.Add(footer);

            return footer;
        }
    }
}
