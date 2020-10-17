using System;
using System.Windows.Forms;

namespace CInject.SampleWinform
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {

            ChangeValue(txtInputValue);
        }

        private void ChangeValue(TextBox textValue)
        {

            try
            {
                this.lblValue.Text = textValue.Text;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
  

        private void button1_Click(object sender, EventArgs e)
        {
             

        }
    }

}
