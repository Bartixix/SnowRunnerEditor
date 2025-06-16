using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SnowRunnerEditor.Views;
using Windows.Foundation;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SnowRunnerEditor
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitTitleBar();
        }

        private void InitTitleBar()
        {
            ExtendsContentIntoTitleBar = true;

            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            NavBar.Loaded += SetupRegionsMid;
            NavBar.SizeChanged += SetupRegionsMid;
        }

        private void SetupRegionsMid(object sender, RoutedEventArgs e) => SetupRegions();

        private void SetupRegions()
        {
            double scale = NavBar.XamlRoot.RasterizationScale;

            RectInt32[] rectArr =
            [
                GetRectInt32(NavBar.MenuBar, scale)
            ];

            InputNonClientPointerSource nonClientSrc = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
            nonClientSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArr);
        }

        private static RectInt32 GetRectInt32(FrameworkElement element, double scale)
        {
            GeneralTransform transform = element.TransformToVisual(null);
            Rect bounds = transform.TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));
            RectInt32 rect = GetRect(bounds, scale);

            return rect;
        }

        private static RectInt32 GetRect(Rect bounds, double scale)
        {
            return new RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
        }
    }
}
