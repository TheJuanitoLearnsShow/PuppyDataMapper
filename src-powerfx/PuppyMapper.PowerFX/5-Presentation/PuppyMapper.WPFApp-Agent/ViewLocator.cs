using System;
using System.Windows;
using System.Windows.Controls;
using PuppyMapper.WPFApp.ViewModels;

namespace PuppyMapper.WPFApp;

public class ViewLocator : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is null)
            return null;

        var name = item.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return new DataTemplate { TargetType = type };
        }

        return new DataTemplate
        {
            VisualTree = new FrameworkElementFactory(typeof(TextBlock))
            {
                Text = "Not Found: " + name
            }
        };
    }
}