using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Core;
using SkyApm.Core.Tracing;
using System;
using System.Collections.Generic;
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
            var _tracingContext = WorkContext.TracingContext;

            var context =  _tracingContext.CreateEntrySegmentContext("btnChangeValue_Click2", new TextCarrierHeaderCollection(new Dictionary<string, string>()));

            context.Span.AddTag("新节点1", "测试");
            context.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now}"));

            _tracingContext.Release(context);

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
