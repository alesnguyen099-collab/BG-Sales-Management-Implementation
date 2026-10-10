using System.Collections.ObjectModel;
using BGSM.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BGSM.App;

public sealed partial class MainWindow : Window
{
    private readonly DatabaseInitializer _initializer = new();
    private readonly CustomerRepository _repository = new();
    private readonly ObservableCollection<Customer> _items = new();
    private bool _initialized;

    public MainWindow()
    {
        InitializeComponent();
        CustomerList.ItemsSource = _items;
        Activated += OnActivated;
    }

    private async void OnActivated(object sender, WindowActivatedEventArgs args)
    {
        if (_initialized) return;
        _initialized = true;
        try
        {
            await _initializer.InitializeAsync();
            await LoadCustomersAsync();
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Lỗi khởi tạo dữ liệu", ex.Message);
        }
    }

    private void Overview_Click(object sender, RoutedEventArgs e)
    {
        OverviewPanel.Visibility = Visibility.Visible;
        CustomersPanel.Visibility = Visibility.Collapsed;
    }

    private async void Customers_Click(object sender, RoutedEventArgs e)
    {
        OverviewPanel.Visibility = Visibility.Collapsed;
        CustomersPanel.Visibility = Visibility.Visible;
        await LoadCustomersAsync();
    }

    private async void ComingSoon_Click(object sender, RoutedEventArgs e)
    {
        await ShowMessageAsync("Đang phát triển", "Chức năng này chưa có trong bản hiện tại.");
    }

    private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_initialized && CustomersPanel.Visibility == Visibility.Visible)
            await LoadCustomersAsync();
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e) => await LoadCustomersAsync();

    private async Task LoadCustomersAsync()
    {
        try
        {
            var results = await _repository.SearchAsync(SearchBox.Text);
            _items.Clear();
            foreach (var item in results) _items.Add(item);
            StatusText.Text = $"Hiển thị {_items.Count} khách hàng";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Không tải được dữ liệu.";
            await ShowMessageAsync("Lỗi tải khách hàng", ex.Message);
        }
    }

    private async void AddCustomer_Click(object sender, RoutedEventArgs e)
    {
        var name = new TextBox { PlaceholderText = "Tên khách hàng (bắt buộc)" };
        var code = new TextBox { PlaceholderText = "Mã khách hàng (không bắt buộc)" };
        var region = new TextBox { PlaceholderText = "Khu vực (không bắt buộc)" };
        var phone = new TextBox { PlaceholderText = "Số điện thoại (không bắt buộc)" };
        var form = new StackPanel { Spacing = 10 };
        form.Children.Add(name);
        form.Children.Add(code);
        form.Children.Add(region);
        form.Children.Add(phone);

        var dialog = new ContentDialog
        {
            Title = "Thêm khách hàng",
            Content = form,
            PrimaryButtonText = "Lưu",
            CloseButtonText = "Hủy",
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = Content.XamlRoot
        };
        if (await dialog.ShowAsync() != ContentDialogResult.Primary) return;
        if (string.IsNullOrWhiteSpace(name.Text))
        {
            await ShowMessageAsync("Thiếu thông tin", "Vui lòng nhập tên khách hàng.");
            return;
        }

        try
        {
            await _repository.CreateAsync(name.Text, code.Text, region.Text, phone.Text);
            await LoadCustomersAsync();
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Không lưu được khách hàng", ex.Message);
        }
    }

    private async Task ShowMessageAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "Đóng",
            XamlRoot = Content.XamlRoot
        };
        await dialog.ShowAsync();
    }
}