using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Core;
using SkyApm.Core.Tracing;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CInject.SampleWinform
{

    public partial class Form1 : Form
    {

        private ITracingContext _tracingContext = WorkContext.TracingContext;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {

            var context = _tracingContext.CreateEntrySegmentContext("btnChangeValue_Click", new TextCarrierHeaderCollection(new Dictionary<string, string>()));

            context.Span.AddTag("新节点1", "测试");
            context.Span.AddLog(LogEvent.Message($"Worker running at2: {DateTime.Now}"));


            _tracingContext.Release(context);

            ChangeValue(txtInputValue);
        }

        private void ChangeValue(TextBox textValue)
        {

            try
            {

                var context = _tracingContext.CreateEntrySegmentContext("ChangeValue", new TextCarrierHeaderCollection(new Dictionary<string, string>()));

                context.Span.AddTag("新节点2", "测试");
                context.Span.AddLog(LogEvent.Message($"Worker running at3: {DateTime.Now}"));

                Thread.Sleep(2000);
                this.lblValue.Text = textValue.Text;

                _tracingContext.Release(context);
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
