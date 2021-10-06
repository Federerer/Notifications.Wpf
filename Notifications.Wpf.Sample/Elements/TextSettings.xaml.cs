using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notification.Wpf.Sample.Elements
{
    /// <summary>
    /// Логика взаимодействия для TextSettings.xaml
    /// </summary>
    public partial class TextSettings : UserControl
    {
        public TextSettings()
        {
            InitializeComponent();
            StyleBox.ItemsSource = new List<FontStyle>() { FontStyles.Normal, FontStyles.Italic, FontStyles.Oblique };
            WeightBox.ItemsSource = new List<FontWeight>()
            {
                FontWeights.Normal,
                FontWeights.Black,
                FontWeights.Bold,
                FontWeights.DemiBold,
                FontWeights.ExtraBlack,
                FontWeights.ExtraBold,
                FontWeights.ExtraLight,
                FontWeights.Heavy,
                FontWeights.Light,
                FontWeights.Medium,
                FontWeights.Regular,
                FontWeights.SemiBold,
                FontWeights.Thin,
                FontWeights.UltraBlack,
                FontWeights.UltraBold,
                FontWeights.UltraLight
            };
        }

        #region TextAlign : TextAlignment - Выравнивание текста

        /// <summary>Выравнивание текста</summary>
        public static readonly DependencyProperty TextAlignProperty =
            DependencyProperty.Register(
                nameof(TextAlign),
                typeof(TextAlignment),
                typeof(TextSettings),
                new PropertyMetadata(default(TextAlignment)));

        /// <summary>Выравнивание текста</summary>
        public TextAlignment TextAlign { get => (TextAlignment)GetValue(TextAlignProperty); set => SetValue(TextAlignProperty, value); }

        #endregion
    }
}
