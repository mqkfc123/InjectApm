// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: register/Register.proto
// </auto-generated>
// Original file comments:
//
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace SkyWalking.NetworkProtocol {
  /// <summary>
  ///register service for ApplicationCode, this service is called when service starts.
  /// </summary>
  public static partial class Register
  {
    static readonly string __ServiceName = "Register";

    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.Services> __Marshaller_Services = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.Services.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.ServiceRegisterMapping> __Marshaller_ServiceRegisterMapping = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.ServiceRegisterMapping.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.ServiceInstances> __Marshaller_ServiceInstances = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.ServiceInstances.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping> __Marshaller_ServiceInstanceRegisterMapping = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.Endpoints> __Marshaller_Endpoints = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.Endpoints.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.EndpointMapping> __Marshaller_EndpointMapping = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.EndpointMapping.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.NetAddresses> __Marshaller_NetAddresses = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.NetAddresses.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.NetAddressMapping> __Marshaller_NetAddressMapping = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.NetAddressMapping.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings> __Marshaller_ServiceAndNetworkAddressMappings = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::SkyWalking.NetworkProtocol.Commands> __Marshaller_Commands = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::SkyWalking.NetworkProtocol.Commands.Parser.ParseFrom);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.Services, global::SkyWalking.NetworkProtocol.ServiceRegisterMapping> __Method_doServiceRegister = new grpc::Method<global::SkyWalking.NetworkProtocol.Services, global::SkyWalking.NetworkProtocol.ServiceRegisterMapping>(
        grpc::MethodType.Unary,
        __ServiceName,
        "doServiceRegister",
        __Marshaller_Services,
        __Marshaller_ServiceRegisterMapping);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.ServiceInstances, global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping> __Method_doServiceInstanceRegister = new grpc::Method<global::SkyWalking.NetworkProtocol.ServiceInstances, global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping>(
        grpc::MethodType.Unary,
        __ServiceName,
        "doServiceInstanceRegister",
        __Marshaller_ServiceInstances,
        __Marshaller_ServiceInstanceRegisterMapping);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.Endpoints, global::SkyWalking.NetworkProtocol.EndpointMapping> __Method_doEndpointRegister = new grpc::Method<global::SkyWalking.NetworkProtocol.Endpoints, global::SkyWalking.NetworkProtocol.EndpointMapping>(
        grpc::MethodType.Unary,
        __ServiceName,
        "doEndpointRegister",
        __Marshaller_Endpoints,
        __Marshaller_EndpointMapping);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.NetAddresses, global::SkyWalking.NetworkProtocol.NetAddressMapping> __Method_doNetworkAddressRegister = new grpc::Method<global::SkyWalking.NetworkProtocol.NetAddresses, global::SkyWalking.NetworkProtocol.NetAddressMapping>(
        grpc::MethodType.Unary,
        __ServiceName,
        "doNetworkAddressRegister",
        __Marshaller_NetAddresses,
        __Marshaller_NetAddressMapping);

    static readonly grpc::Method<global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings, global::SkyWalking.NetworkProtocol.Commands> __Method_doServiceAndNetworkAddressMappingRegister = new grpc::Method<global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings, global::SkyWalking.NetworkProtocol.Commands>(
        grpc::MethodType.Unary,
        __ServiceName,
        "doServiceAndNetworkAddressMappingRegister",
        __Marshaller_ServiceAndNetworkAddressMappings,
        __Marshaller_Commands);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::SkyWalking.NetworkProtocol.RegisterReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Register</summary>
    [grpc::BindServiceMethod(typeof(Register), "BindService")]
    public abstract partial class RegisterBase
    {
      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.ServiceRegisterMapping> doServiceRegister(global::SkyWalking.NetworkProtocol.Services request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping> doServiceInstanceRegister(global::SkyWalking.NetworkProtocol.ServiceInstances request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.EndpointMapping> doEndpointRegister(global::SkyWalking.NetworkProtocol.Endpoints request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.NetAddressMapping> doNetworkAddressRegister(global::SkyWalking.NetworkProtocol.NetAddresses request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::SkyWalking.NetworkProtocol.Commands> doServiceAndNetworkAddressMappingRegister(global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Register</summary>
    public partial class RegisterClient : grpc::ClientBase<RegisterClient>
    {
      /// <summary>Creates a new client for Register</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public RegisterClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Register that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public RegisterClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected RegisterClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected RegisterClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::SkyWalking.NetworkProtocol.ServiceRegisterMapping doServiceRegister(global::SkyWalking.NetworkProtocol.Services request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceRegister(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.ServiceRegisterMapping doServiceRegister(global::SkyWalking.NetworkProtocol.Services request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_doServiceRegister, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.ServiceRegisterMapping> doServiceRegisterAsync(global::SkyWalking.NetworkProtocol.Services request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceRegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.ServiceRegisterMapping> doServiceRegisterAsync(global::SkyWalking.NetworkProtocol.Services request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_doServiceRegister, null, options, request);
      }
      public virtual global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping doServiceInstanceRegister(global::SkyWalking.NetworkProtocol.ServiceInstances request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceInstanceRegister(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping doServiceInstanceRegister(global::SkyWalking.NetworkProtocol.ServiceInstances request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_doServiceInstanceRegister, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping> doServiceInstanceRegisterAsync(global::SkyWalking.NetworkProtocol.ServiceInstances request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceInstanceRegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping> doServiceInstanceRegisterAsync(global::SkyWalking.NetworkProtocol.ServiceInstances request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_doServiceInstanceRegister, null, options, request);
      }
      public virtual global::SkyWalking.NetworkProtocol.EndpointMapping doEndpointRegister(global::SkyWalking.NetworkProtocol.Endpoints request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doEndpointRegister(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.EndpointMapping doEndpointRegister(global::SkyWalking.NetworkProtocol.Endpoints request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_doEndpointRegister, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.EndpointMapping> doEndpointRegisterAsync(global::SkyWalking.NetworkProtocol.Endpoints request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doEndpointRegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.EndpointMapping> doEndpointRegisterAsync(global::SkyWalking.NetworkProtocol.Endpoints request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_doEndpointRegister, null, options, request);
      }
      public virtual global::SkyWalking.NetworkProtocol.NetAddressMapping doNetworkAddressRegister(global::SkyWalking.NetworkProtocol.NetAddresses request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doNetworkAddressRegister(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.NetAddressMapping doNetworkAddressRegister(global::SkyWalking.NetworkProtocol.NetAddresses request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_doNetworkAddressRegister, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.NetAddressMapping> doNetworkAddressRegisterAsync(global::SkyWalking.NetworkProtocol.NetAddresses request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doNetworkAddressRegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.NetAddressMapping> doNetworkAddressRegisterAsync(global::SkyWalking.NetworkProtocol.NetAddresses request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_doNetworkAddressRegister, null, options, request);
      }
      public virtual global::SkyWalking.NetworkProtocol.Commands doServiceAndNetworkAddressMappingRegister(global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceAndNetworkAddressMappingRegister(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::SkyWalking.NetworkProtocol.Commands doServiceAndNetworkAddressMappingRegister(global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_doServiceAndNetworkAddressMappingRegister, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.Commands> doServiceAndNetworkAddressMappingRegisterAsync(global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return doServiceAndNetworkAddressMappingRegisterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::SkyWalking.NetworkProtocol.Commands> doServiceAndNetworkAddressMappingRegisterAsync(global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_doServiceAndNetworkAddressMappingRegister, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override RegisterClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new RegisterClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(RegisterBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_doServiceRegister, serviceImpl.doServiceRegister)
          .AddMethod(__Method_doServiceInstanceRegister, serviceImpl.doServiceInstanceRegister)
          .AddMethod(__Method_doEndpointRegister, serviceImpl.doEndpointRegister)
          .AddMethod(__Method_doNetworkAddressRegister, serviceImpl.doNetworkAddressRegister)
          .AddMethod(__Method_doServiceAndNetworkAddressMappingRegister, serviceImpl.doServiceAndNetworkAddressMappingRegister).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, RegisterBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_doServiceRegister, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SkyWalking.NetworkProtocol.Services, global::SkyWalking.NetworkProtocol.ServiceRegisterMapping>(serviceImpl.doServiceRegister));
      serviceBinder.AddMethod(__Method_doServiceInstanceRegister, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SkyWalking.NetworkProtocol.ServiceInstances, global::SkyWalking.NetworkProtocol.ServiceInstanceRegisterMapping>(serviceImpl.doServiceInstanceRegister));
      serviceBinder.AddMethod(__Method_doEndpointRegister, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SkyWalking.NetworkProtocol.Endpoints, global::SkyWalking.NetworkProtocol.EndpointMapping>(serviceImpl.doEndpointRegister));
      serviceBinder.AddMethod(__Method_doNetworkAddressRegister, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SkyWalking.NetworkProtocol.NetAddresses, global::SkyWalking.NetworkProtocol.NetAddressMapping>(serviceImpl.doNetworkAddressRegister));
      serviceBinder.AddMethod(__Method_doServiceAndNetworkAddressMappingRegister, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SkyWalking.NetworkProtocol.ServiceAndNetworkAddressMappings, global::SkyWalking.NetworkProtocol.Commands>(serviceImpl.doServiceAndNetworkAddressMappingRegister));
    }

  }
}
#endregion
