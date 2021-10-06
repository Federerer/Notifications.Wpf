using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;
using Notification.Wpf.Base.Interfaces;

namespace Notification.Wpf.Base
{
    /// <summary> Text content settings </summary>
    public class TextContentSettings : ITextContentSettings
    {
        #region Implementation of ITextSettings

        /// <inheritdoc />
        public FontFamily FontFamily { get; set; } = new FontFamily("Tahoma");

        /// <inheritdoc />
        public double FontSize { get; set; } = 14;

        /// <inheritdoc />
        public HorizontalAlignment HorizontalTextAlignment { get; set; } = HorizontalAlignment.Left;

        /// <inheritdoc />
        public VerticalAlignment VerticalTextAlignment { get; set; } = VerticalAlignment.Center;

        /// <inheritdoc />
        public FontStyle FontStyle { get; set; } = FontStyles.Normal;

        /// <inheritdoc />
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;

        #endregion
    }
}
