using System;

namespace Infragistics.Framework.Converters
{
    public class CodeSpan
    {
        public CodeSpan()
        {
            Text = "";
            Font = 12;
            Foreground = Paints.White;
            Background = Paint.FromString("#FF1E1E1E");
        }

        public CodeSpan(string text)
        {
            Foreground = Paints.White;
            Text = text;
        }
        public CodeSpan(string text, Paint foreground)
        {
            Foreground = foreground;
            Text = text;
        }
        /// <summary>
        /// Gets or sets Text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets Text
        /// </summary>
        public double Font { get; set; }

        public Paint Foreground { get; set; }
        public Paint Background { get; set; }
       
        public new string ToString()
        {
            return "" + Text; 
        }
    }
  
}