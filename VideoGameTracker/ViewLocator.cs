using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VideoGameTracker.ViewModels;

namespace VideoGameTracker
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object? param)
        {
            if (param is null)
                return new TextBlock { Text = "Not Found: null" };

            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (IControl)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
