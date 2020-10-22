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

        //private ITracingContext _tracingContext = WorkContext.TracingContext;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            //var context = _tracingContext.CreateEntrySegmentContext("btnChangeValue_JayJ1", new TextCarrierHeaderCollection(new Dictionary<string, string>()));
            //context.Span.AddTag("新节点1", "测试");
            //context.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now}"));

            ChangeValue(txtInputValue.Text);
        }

        private void ChangeValue(string textValue)
        {

            //var context = _tracingContext.CreateExitSegmentContext("ChangeValue", "test");
            try
            {
                //context.Span.AddTag("新节点2", "测试");
                //context.Span.AddLog(LogEvent.Message($"Worker running at3: {DateTime.Now}"));
                Thread.Sleep(2000);
                this.lblValue.Text = textValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //var entryContext = WorkContext.EntrySegmentContextAccessor;
                //_tracingContext.Release(entryContext.Context);
                //_tracingContext.Release(context);
                ChangeValue2(textValue);
            }

        }
        private void ChangeValue2(string textValue)
        {

            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }
    }

}
