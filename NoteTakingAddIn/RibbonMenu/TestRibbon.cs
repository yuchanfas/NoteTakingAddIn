using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

using Debug = System.Diagnostics.Debug;


namespace NoteTakingAddIn
{
    public partial class TestRibbon
    {
        private void AttributeRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void SelectAttribute(object sender, RibbonControlEventArgs e)
        {
            Debug.WriteLine("Attribute:"+e.Control.Id);
        }
    }
}
