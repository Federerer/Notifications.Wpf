using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Media;

namespace Notification.Wpf.Base.Interfaces
{
    /// <summary> Text content settings </summary>
    public interface ITextContentSettings
    {
        /// <summary> Message font </summary>
        public FontFamily FontFamily { get; set; }
        /// <summary> Message font size </summary>
        public double FontSize { get; set; }
        /// <summary> Message horizontal text alignment </summary>
        public HorizontalAlignment HorizontalTextAlignment { get; set; }
        /// <summary> Message vertical text alignment </summary>
        public VerticalAlignment VerticalTextAlignment { get; set; }

        /// <summary> Font style </summary>
        public FontStyle FontStyle { get; set; }
        /// <summary> Font Weight </summary>
        public FontWeight FontWeight { get; set; }
    }
}
