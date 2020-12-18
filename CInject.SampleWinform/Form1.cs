﻿using SkyApm.Abstractions.Common;
using SkyApm.Abstractions.Context.Utils;
using SkyApm.Abstractions.Tracing;
using SkyApm.Abstractions.Tracing.Segments;
using SkyApm.Abstractions.Utils;
using SkyApm.Core;
using SkyApm.Core.Context;
using SkyApm.Core.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //var httpRequestSpan = ContextManager.CreateEntrySpan("Form1_Load", null);
            //Thread.Sleep(1000);

            //httpRequestSpan?.AsHttp();
            ////httpRequestSpan.SetComponent(ComponentsDefine.AspNet);
            //httpRequestSpan?.Tag("ddddd", "ffffff");
            //httpRequestSpan?.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            //    new Dictionary<string, object>
            //    {
            //        {"event", "AspNet BeginRequest"},
            //        {"message", $"Request starting xxxxxxxxxxx "}
            //    });

            //httpRequestSpan?.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            // new Dictionary<string, object>
            // {
            //        {"event", "AspNet EndRequest"},
            //        {"message", $"Request finished "}
            // });

            //ContextManager.StopSpan(httpRequestSpan);
        }


        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            var context = _tracingContext.CreateEntrySegmentContext("btnChangeValue_JayJ1", new TextCarrierHeaderCollection(new Dictionary<string, string>()));
            context.Span.AddTag("新节点", "测试");
            context.Span.AddLog(LogEvent.Message($"Worker running at: {DateTime.Now}"));
            try
            {
                ChangeValue(txtInputValue.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _tracingContext.Release(context);
            }
        }

        private void ChangeValue(string textValue)
        {
            var context = _tracingContext.CreateLocalSegmentContext("ChangeValue1");
            try
            {
                context.Span.AddTag("新节点1", "测试1");
                context.Span.AddLog(LogEvent.Message($"Worker running at1: {DateTime.Now}"));
                Thread.Sleep(2000);
                this.lblValue.Text = textValue; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _tracingContext.Release(context);
            }

        }
        private void ChangeValue2(string textValue)
        {
            var context = _tracingContext.CreateExitSegmentContext("ChangeValue2", "test2");

            try
            {
                context.Span.AddTag("新节点2", "测试2");
                context.Span.AddLog(LogEvent.Message($"Worker running at2: {DateTime.Now}"));
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _tracingContext.Release(context);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("button1_Click_1");
            test();
            //sample();
        }

        private void test2()
        {
            MessageBox.Show("test2");
            Thread.Sleep(2000);
        }

        private void test()
        {
            MessageBox.Show("test");
            Thread.Sleep(2000);
            test2();
        }

        private void sample()
        {
            MessageBox.Show("sample");
            Thread.Sleep(1000);
        }

        private void button2_Click(object sender, EventArgs e)
        {

           // var httpRequestSpan = ContextManager.CreateEntrySpan("button22_Click", null);
            Thread.Sleep(1000);
            button22("button22");
            button44("button44");
            // httpRequestSpan.AsHttp();
            //httpRequestSpan.SetComponent(ComponentsDefine.AspNet);
            //httpRequestSpan.Tag("ddddd", "ffffff");
            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            //    new Dictionary<string, object>
            //    {
            //        {"event", "AspNet BeginRequest"},
            //        {"message", $"Request starting xxxxxxxxxxx "}
            //    });

            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            // new Dictionary<string, object>
            // {
            //        {"event", "AspNet EndRequest"},
            //        {"message", $"Request finished "}
            // });

            //ContextManager.StopSpan(httpRequestSpan);

        }


        private void button22(string textValue)
        {

            //var httpRequestSpan = ContextManager.CreateLocalSpan("button22");
            Thread.Sleep(1000);
            button33("");

            //httpRequestSpan.AsHttp();
            ////httpRequestSpan.SetComponent(ComponentsDefine.AspNet);
            //httpRequestSpan.Tag("LocalSpan", "ffffff");
            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            //    new Dictionary<string, object>
            //    {
            //        {"event", "AspNet BeginRequest"},
            //        {"message", $"Request starting xxxxxxxxxxx "}
            //    });

            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            // new Dictionary<string, object>
            // {
            //        {"event", "AspNet EndRequest"},
            //        {"message", $"Request finished "}
            // });

            //ContextManager.StopSpan(httpRequestSpan);

        }


        private void button33(string textValue)
        {

            //var httpRequestSpan = ContextManager.CreateLocalSpan("button33");

            //httpRequestSpan.AsHttp();
            ////httpRequestSpan.SetComponent(ComponentsDefine.AspNet);
            //httpRequestSpan.Tag("LocalSpan33", "button33");
            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            //    new Dictionary<string, object>
            //    {
            //        {"event", "AspNet BeginRequest"},
            //        {"message", $"Request starting xxxxxxxxxxx "}
            //    });

            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            // new Dictionary<string, object>
            // {
            //        {"event", "AspNet EndRequest"},
            //        {"message", $"Request finished "}
            // });

            //ContextManager.StopSpan(httpRequestSpan);
            Thread.Sleep(1000);
        }

        private void button44(string textValue)
        {
            Thread.Sleep(1000);
            //var httpRequestSpan = ContextManager.CreateLocalSpan("button44");

            //httpRequestSpan.AsHttp();
            ////httpRequestSpan.SetComponent(ComponentsDefine.AspNet);
            //httpRequestSpan.Tag("LocalSpan44", "button44");
            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            //    new Dictionary<string, object>
            //    {
            //        {"event", "AspNet BeginRequest"},
            //        {"message", $"Request starting xxxxxxxxxxx "}
            //    });

            //httpRequestSpan.Log(DateTimeOffsetUtcNow.ToUnixTimeMilliseconds(),
            // new Dictionary<string, object>
            // {
            //        {"event", "AspNet EndRequest"},
            //        {"message", $"Request finished "}
            // });

            //ContextManager.StopSpan(httpRequestSpan);

        }

        
    }

}
