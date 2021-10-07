using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;
using Notification.Wpf.Base.Interfaces;
using Notification.Wpf.Constants;

namespace Notification.Wpf.Base
{
    /// <summary> Text content settings </summary>
    public class TextContentSettings : ITextContentSettings
    {
        #region Implementation of ITextSettings

        /// <inheritdoc />
        public FontFamily FontFamily { get; set; } = new (NotificationConstants.FontName);

        /// <inheritdoc />
        public double FontSize { get; set; } = NotificationConstants.BaseTextSize;

        /// <inheritdoc />
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        /// <inheritdoc />
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

        /// <inheritdoc />
        public VerticalAlignment VerticalTextAlignment { get; set; } = VerticalAlignment.Stretch;

        /// <inheritdoc />
        public FontStyle FontStyle { get; set; } = FontStyles.Normal;

        /// <inheritdoc />
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;

        #endregion
    }
}
