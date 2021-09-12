namespace Notification.Wpf
{
    /// <summary>
    /// Способ отображения текста в сообщении
    /// </summary>
    public enum NotificationTextTrimType
    {
        /// <summary>No trim text </summary>
        NoTrim,
        /// <summary>Always trim text </summary>
        Trim,
        /// <summary>Attach long text and trim</summary>
        Attach,
        /// <summary>Automatic attach if need trim</summary>
        AttachIfMoreRows
    }
}