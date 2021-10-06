using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;

namespace Notification.Wpf.Base.Interfaces
{
    /// <summary> Text content settings </summary>
    public interface ITextContentSettings
    {
        /// <summary> font </summary>
        public FontFamily FontFamily { get; set; }
        /// <summary> font size </summary>
        public double FontSize { get; set; }
        /// <summary> horizontal text alignment </summary>
        public TextAlignment TextAlignment { get; set; } 
        /// <summary> horizontal alignment </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }
        /// <summary> vertical text alignment </summary>
        public VerticalAlignment VerticalTextAlignment { get; set; }

        /// <summary> Font style </summary>
        public FontStyle FontStyle { get; set; }
        /// <summary> Font Weight </summary>
        public FontWeight FontWeight { get; set; }
    }
}
