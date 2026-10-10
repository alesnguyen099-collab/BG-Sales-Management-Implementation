using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace BGSM.App;
public sealed partial class MainWindow : Window
{
 public MainWindow() => InitializeComponent();
 private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
 { if (args.SelectedItem is NavigationViewItem item && item.Content is string title) PageTitle.Text = title; }
}