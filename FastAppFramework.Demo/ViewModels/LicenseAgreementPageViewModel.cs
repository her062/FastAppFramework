using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using FastAppFramework.Wpf;

namespace FastAppFramework.Demo.ViewModels
{
    public class LicenseAgreementPageViewModel : BindableBase
    {
#region Properties
        public FlowDocument License
        {
            get; private set;
        }

#endregion

#region Constructor/Destructor
        public LicenseAgreementPageViewModel()
        {
            // Setup Properties.
            {
                using (var stream = Assembly.GetEntryAssembly()?.GetManifestResourceStream("LICENSE"))
                {
                    if (stream == null)
                        throw new ApplicationException();

                    this.License = new FlowDocument();
                    var license = new TextRange(this.License.ContentStart, this.License.ContentEnd);
                    if (license.CanLoad(DataFormats.Text))
                        license.Load(stream, DataFormats.Text);
                }
            }
        }
#endregion
    }
}