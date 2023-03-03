using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace FastAppFramework.Demo
{
    public static class RichTextBoxHelper
    {
#region Properties
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document",
            typeof(FlowDocument),
            typeof(RichTextBoxHelper),
            new PropertyMetadata(DocumentPropertyChanged)
        );
        public static FlowDocument GetDocument(DependencyObject d) => (FlowDocument)d.GetValue(DocumentProperty);
        public static void SetDocument(DependencyObject d, FlowDocument value) => d.SetValue(DocumentProperty, value);
#endregion

#region Private Functions
        private static void DocumentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as RichTextBox;
            if ((obj != null) && (e.NewValue is FlowDocument doc))
            {
                var source = new TextRange(doc.ContentStart, doc.ContentEnd);
                using (var stream = new MemoryStream())
                {
                    XamlWriter.Save(source, stream);
                    source.Save(stream, DataFormats.XamlPackage);

                    var dest = new TextRange(obj.Document.ContentStart, obj.Document.ContentEnd);
                    dest.Load(stream, DataFormats.XamlPackage);
                }
            }
        }
#endregion
    }
}