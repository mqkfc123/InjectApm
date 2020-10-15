using Autofac;
using CInject.SampleWinform.Common;
using CInject.SampleWinform.Transport;
using Grpc.Core;
using SCInject.SampleWinform.Common;
using SkyApm.Abstractions;
using SkyApm.Abstractions.Tracing;
using SkyApm.Core;
using SkyWalking.NetworkProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CInject.SampleWinform
{

    public partial class Form1 : Form
    {
        IInstrumentStartup _service = WorkContext.LifetimeScope.Resolve<IInstrumentStartup>();
        //Worker _worker = WorkContext.LifetimeScope.Resolve<Worker>();


        public Form1()
        {
            InitializeComponent();
            _service.StartAsync();
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {

            ChangeValue(txtInputValue);
        }


        private void ChangeValue(TextBox textValue)
        {

            // Channel channel = new Channel("10.16.0.25:11800", ChannelCredentials.Insecure);
            //Channel channel = new Channel("10.16.2.113:11800", ChannelCredentials.Insecure);

            try
            {
                //    var client = new Register.RegisterClient(channel);

                //    var services = new Services();
                //    services.Services_.Add(new Service
                //    {
                //        ServiceName = "sample_client"
                //    });

                //    var metadata = new Metadata { new Metadata.Entry("Authentication", textValue.Text) };

                //    var reply = client.doServiceRegister(services);


                //    Console.WriteLine("来自:" + reply.Services);

                //    var instanceUUID = Guid.NewGuid().ToString("N");

                //    var serviceInstanceId = instance(channel, instanceUUID, reply.Services[0].Value);
                //    clr(channel, reply.Services[0].Value, serviceInstanceId);
                //    trace(channel, reply.Services[0].Value, serviceInstanceId);

                //    doPing(channel, instanceUUID, serviceInstanceId);
                this.lblValue.Text = textValue.Text;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private const string OS_NAME = "os_name";
        private const string HOST_NAME = "host_name";
        private const string IPV4 = "ipv4";
        private const string PROCESS_NO = "process_no";
        private const string LANGUAGE = "language";

        private int instance(Channel connection, string instanceUUID, int serviceId)
        {
            var serviceInstanceId = 0;

            var properties = new AgentOsInfoRequest
            {
                HostName = DnsHelpers.GetHostName(),
                IpAddress = DnsHelpers.GetIpV4s(),
                OsName = PlatformInformation.GetOSName(),
                ProcessNo = Process.GetCurrentProcess().Id,
                Language = "dotnet"
            };
            var request = new ServiceInstanceRequest
            {
                ServiceId = serviceId,
                InstanceUUID = instanceUUID,
                Properties = properties
            };

            var client = new Register.RegisterClient(connection);
            var instance = new ServiceInstance
            {
                ServiceId = request.ServiceId,
                InstanceUUID = request.InstanceUUID,
                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            instance.Properties.Add(new KeyStringValuePair
            { Key = OS_NAME, Value = request.Properties.OsName });
            instance.Properties.Add(new KeyStringValuePair
            { Key = HOST_NAME, Value = request.Properties.HostName });
            instance.Properties.Add(new KeyStringValuePair
            { Key = PROCESS_NO, Value = request.Properties.ProcessNo.ToString() });
            instance.Properties.Add(new KeyStringValuePair
            { Key = LANGUAGE, Value = request.Properties.Language });
            foreach (var ip in request.Properties.IpAddress)
            {
                instance.Properties.Add(new KeyStringValuePair { Key = IPV4, Value = ip });
            }

            var serviceInstances = new ServiceInstances();
            serviceInstances.Instances.Add(instance);

            try
            {

                var value = Task.Run(async () =>
                {
                    return await Polly.Polling(3,
                            () => client.doServiceInstanceRegisterAsync(serviceInstances));
                }).Result;

                //var reply = Task.Run(async () =>
                //{
                //    return await client.doServiceInstanceRegisterAsync(serviceInstances);
                //}).Result;
                //foreach (var serviceInstance in reply.ServiceInstances)
                //{
                //    serviceInstanceId = serviceInstance.Value;
                //}

                serviceInstanceId = value;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return serviceInstanceId;

        }

        private void clr(Channel channel, int serviceId, int serviceInstanceId)
        {

            var availableWorkerThreads = 0;
            var availableCompletionPortThreads = 0;
            var maxWorkerThreads = 0;
            var maxCompletionPortThreads = 0;

            var cpuStats = new CPUStatsRequest
            {
                UsagePercent = CpuHelpers.UsagePercent
            };
            var gcStats = new GCStatsRequest
            {
                Gen0CollectCount = GCHelpers.Gen0CollectCount,
                Gen1CollectCount = GCHelpers.Gen1CollectCount,
                Gen2CollectCount = GCHelpers.Gen2CollectCount,
                HeapMemory = GCHelpers.TotalMemory
            };
            ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
            ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
            var threadStats = new ThreadStatsRequest
            {
                MaxCompletionPortThreads = maxCompletionPortThreads,
                MaxWorkerThreads = maxWorkerThreads,
                AvailableCompletionPortThreads = availableCompletionPortThreads,
                AvailableWorkerThreads = availableWorkerThreads
            };
            var statsRequest = new CLRStatsRequest
            {
                CPU = cpuStats,
                GC = gcStats,
                Thread = threadStats
            };
            try
            {
                var client = new CLRMetricReportService.CLRMetricReportServiceClient(channel);

                var request = new CLRMetricCollection
                {
                    ServiceInstanceId = serviceInstanceId
                };
                var metric = new CLRMetric
                {
                    Cpu = new CPU
                    {
                        UsagePercent = statsRequest.CPU.UsagePercent
                    },
                    Gc = new ClrGC
                    {
                        Gen0CollectCount = statsRequest.GC.Gen0CollectCount,
                        Gen1CollectCount = statsRequest.GC.Gen1CollectCount,
                        Gen2CollectCount = statsRequest.GC.Gen2CollectCount,
                        HeapMemory = statsRequest.GC.HeapMemory
                    },
                    Thread = new ClrThread
                    {
                        AvailableWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        AvailableCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads,
                        MaxWorkerThreads = statsRequest.Thread.MaxWorkerThreads,
                        MaxCompletionPortThreads = statsRequest.Thread.MaxCompletionPortThreads
                    },
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };
                request.Metrics.Add(metric);


                var reply = Task.Run(async () =>
                {
                    return await client.collectAsync(request);
                }).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void trace(Channel channel, int serviceId, int serviceInstanceId)
        {
            var client = new TraceSegmentReportService.TraceSegmentReportServiceClient(channel);

            using (var asyncClientStreamingCall = client.collect())
            {
                SegmentContextMapper context = new SegmentContextMapper();

                SegmentRequest segment = context.Map(serviceId, serviceInstanceId, this.txtInputValue.Text);

                asyncClientStreamingCall.RequestStream.WriteAsync(SegmentV6Helpers.Map(segment));

                asyncClientStreamingCall.RequestStream.CompleteAsync();

                //cli
                //CommandLineApplication
                var var2 = Task.Run(async () =>
                  {
                      return await asyncClientStreamingCall.ResponseAsync;
                  }).Result;

            }

        }


        private void doPing(Channel channel, string instanceId, int serviceInstanceId)
        {
            var client = new ServiceInstancePing.ServiceInstancePingClient(channel);
            //IObserver<T>

            var reply = Task.Run(async () =>
            {
                return await client.doPingAsync(new ServiceInstancePingPkg
                {
                    ServiceInstanceId = serviceInstanceId,
                    ServiceInstanceUUID = instanceId,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                });
            }).Result;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = default(CancellationToken);

            var source = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);

            Task.Run(async () =>
            {
                // await _worker.ExecuteAsync(source.Token);
            });

        }
    }

}
