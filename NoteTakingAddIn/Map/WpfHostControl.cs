using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.Integration;

namespace NoteTakingAddIn
{
    public partial class WpfHostControl : UserControl
    {
        public WpfHostControl()
        {
            InitializeComponent();
        }

        public ElementHost WpfElementControl
        {
            get
            {
                return this.wpfElement;
            }
        }
        public ElementHost wpfElement2Control
        {
            get
            {
                return this.wpfElement2;
            }
        }

        private void wpfElement_ChildChanged(object sender, ChildChangedEventArgs e)
        {

        }

        private void WpfHostControl_Load(object sender, EventArgs e)
        {

        }

        private void wpfElement2_ChildChanged(object sender, ChildChangedEventArgs e)
        {

        }
    }
}
