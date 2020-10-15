// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: common/common.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SkyWalking.NetworkProtocol {

  /// <summary>Holder for reflection information generated from common/common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for common/common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNjb21tb24vY29tbW9uLnByb3RvIjAKEktleVN0cmluZ1ZhbHVlUGFpchIL",
            "CgNrZXkYASABKAkSDQoFdmFsdWUYAiABKAkiLQoPS2V5SW50VmFsdWVQYWly",
            "EgsKA2tleRgBIAEoCRINCgV2YWx1ZRgCIAEoBSIbCgNDUFUSFAoMdXNhZ2VQ",
            "ZXJjZW50GAIgASgBIiYKCENvbW1hbmRzEhoKCGNvbW1hbmRzGAEgAygLMggu",
            "Q29tbWFuZCI9CgdDb21tYW5kEg8KB2NvbW1hbmQYASABKAkSIQoEYXJncxgC",
            "IAMoCzITLktleVN0cmluZ1ZhbHVlUGFpciowCgtEZXRlY3RQb2ludBIKCgZj",
            "bGllbnQQABIKCgZzZXJ2ZXIQARIJCgVwcm94eRACKkcKC1NlcnZpY2VUeXBl",
            "EgoKBm5vcm1hbBAAEgwKCGRhdGFiYXNlEAESBgoCbXEQAhIJCgVjYWNoZRAD",
            "EgsKB2Jyb3dzZXIQBEJJCihvcmcuYXBhY2hlLnNreXdhbGtpbmcuYXBtLm5l",
            "dHdvcmsuY29tbW9uUAGqAhpTa3lXYWxraW5nLk5ldHdvcmtQcm90b2NvbGIG",
            "cHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::SkyWalking.NetworkProtocol.DetectPoint), typeof(global::SkyWalking.NetworkProtocol.ServiceType), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SkyWalking.NetworkProtocol.KeyStringValuePair), global::SkyWalking.NetworkProtocol.KeyStringValuePair.Parser, new[]{ "Key", "Value" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SkyWalking.NetworkProtocol.KeyIntValuePair), global::SkyWalking.NetworkProtocol.KeyIntValuePair.Parser, new[]{ "Key", "Value" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SkyWalking.NetworkProtocol.CPU), global::SkyWalking.NetworkProtocol.CPU.Parser, new[]{ "UsagePercent" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SkyWalking.NetworkProtocol.Commands), global::SkyWalking.NetworkProtocol.Commands.Parser, new[]{ "Commands_" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SkyWalking.NetworkProtocol.Command), global::SkyWalking.NetworkProtocol.Command.Parser, new[]{ "Command_", "Args" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  /// In most cases, detect point should be `server` or `client`.
  /// Even in service mesh, this means `server`/`client` side sidecar
  /// `proxy` is reserved only.
  /// </summary>
  public enum DetectPoint {
    [pbr::OriginalName("client")] Client = 0,
    [pbr::OriginalName("server")] Server = 1,
    [pbr::OriginalName("proxy")] Proxy = 2,
  }

  public enum ServiceType {
    /// <summary>
    /// An agent works inside the normal business application.
    /// </summary>
    [pbr::OriginalName("normal")] Normal = 0,
    /// <summary>
    /// An agent works inside the database.
    /// </summary>
    [pbr::OriginalName("database")] Database = 1,
    /// <summary>
    /// An agent works inside the MQ.
    /// </summary>
    [pbr::OriginalName("mq")] Mq = 2,
    /// <summary>
    /// An agent works inside the cache server.
    /// </summary>
    [pbr::OriginalName("cache")] Cache = 3,
    /// <summary>
    /// An agent works inside the browser.
    /// </summary>
    [pbr::OriginalName("browser")] Browser = 4,
  }

  #endregion

  #region Messages
  public sealed partial class KeyStringValuePair : pb::IMessage<KeyStringValuePair> {
    private static readonly pb::MessageParser<KeyStringValuePair> _parser = new pb::MessageParser<KeyStringValuePair>(() => new KeyStringValuePair());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<KeyStringValuePair> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SkyWalking.NetworkProtocol.CommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyStringValuePair() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyStringValuePair(KeyStringValuePair other) : this() {
      key_ = other.key_;
      value_ = other.value_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyStringValuePair Clone() {
      return new KeyStringValuePair(this);
    }

    /// <summary>Field number for the "key" field.</summary>
    public const int KeyFieldNumber = 1;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "value" field.</summary>
    public const int ValueFieldNumber = 2;
    private string value_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Value {
      get { return value_; }
      set {
        value_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as KeyStringValuePair);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(KeyStringValuePair other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Key != other.Key) return false;
      if (Value != other.Value) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (Value.Length != 0) hash ^= Value.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Key.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Key);
      }
      if (Value.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Value);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (Value.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Value);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(KeyStringValuePair other) {
      if (other == null) {
        return;
      }
      if (other.Key.Length != 0) {
        Key = other.Key;
      }
      if (other.Value.Length != 0) {
        Value = other.Value;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Key = input.ReadString();
            break;
          }
          case 18: {
            Value = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class KeyIntValuePair : pb::IMessage<KeyIntValuePair> {
    private static readonly pb::MessageParser<KeyIntValuePair> _parser = new pb::MessageParser<KeyIntValuePair>(() => new KeyIntValuePair());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<KeyIntValuePair> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SkyWalking.NetworkProtocol.CommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyIntValuePair() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyIntValuePair(KeyIntValuePair other) : this() {
      key_ = other.key_;
      value_ = other.value_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public KeyIntValuePair Clone() {
      return new KeyIntValuePair(this);
    }

    /// <summary>Field number for the "key" field.</summary>
    public const int KeyFieldNumber = 1;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "value" field.</summary>
    public const int ValueFieldNumber = 2;
    private int value_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as KeyIntValuePair);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(KeyIntValuePair other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Key != other.Key) return false;
      if (Value != other.Value) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (Value != 0) hash ^= Value.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Key.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Key);
      }
      if (Value != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Value);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (Value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Value);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(KeyIntValuePair other) {
      if (other == null) {
        return;
      }
      if (other.Key.Length != 0) {
        Key = other.Key;
      }
      if (other.Value != 0) {
        Value = other.Value;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Key = input.ReadString();
            break;
          }
          case 16: {
            Value = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class CPU : pb::IMessage<CPU> {
    private static readonly pb::MessageParser<CPU> _parser = new pb::MessageParser<CPU>(() => new CPU());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CPU> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SkyWalking.NetworkProtocol.CommonReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CPU() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CPU(CPU other) : this() {
      usagePercent_ = other.usagePercent_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CPU Clone() {
      return new CPU(this);
    }

    /// <summary>Field number for the "usagePercent" field.</summary>
    public const int UsagePercentFieldNumber = 2;
    private double usagePercent_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double UsagePercent {
      get { return usagePercent_; }
      set {
        usagePercent_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CPU);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CPU other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(UsagePercent, other.UsagePercent)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UsagePercent != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(UsagePercent);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UsagePercent != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(UsagePercent);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UsagePercent != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CPU other) {
      if (other == null) {
        return;
      }
      if (other.UsagePercent != 0D) {
        UsagePercent = other.UsagePercent;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 17: {
            UsagePercent = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Commands : pb::IMessage<Commands> {
    private static readonly pb::MessageParser<Commands> _parser = new pb::MessageParser<Commands>(() => new Commands());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Commands> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SkyWalking.NetworkProtocol.CommonReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Commands() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Commands(Commands other) : this() {
      commands_ = other.commands_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Commands Clone() {
      return new Commands(this);
    }

    /// <summary>Field number for the "commands" field.</summary>
    public const int Commands_FieldNumber = 1;
    private static readonly pb::FieldCodec<global::SkyWalking.NetworkProtocol.Command> _repeated_commands_codec
        = pb::FieldCodec.ForMessage(10, global::SkyWalking.NetworkProtocol.Command.Parser);
    private readonly pbc::RepeatedField<global::SkyWalking.NetworkProtocol.Command> commands_ = new pbc::RepeatedField<global::SkyWalking.NetworkProtocol.Command>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SkyWalking.NetworkProtocol.Command> Commands_ {
      get { return commands_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Commands);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Commands other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!commands_.Equals(other.commands_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= commands_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      commands_.WriteTo(output, _repeated_commands_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += commands_.CalculateSize(_repeated_commands_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Commands other) {
      if (other == null) {
        return;
      }
      commands_.Add(other.commands_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            commands_.AddEntriesFrom(input, _repeated_commands_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Command : pb::IMessage<Command> {
    private static readonly pb::MessageParser<Command> _parser = new pb::MessageParser<Command>(() => new Command());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Command> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SkyWalking.NetworkProtocol.CommonReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Command() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Command(Command other) : this() {
      command_ = other.command_;
      args_ = other.args_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Command Clone() {
      return new Command(this);
    }

    /// <summary>Field number for the "command" field.</summary>
    public const int Command_FieldNumber = 1;
    private string command_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Command_ {
      get { return command_; }
      set {
        command_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "args" field.</summary>
    public const int ArgsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::SkyWalking.NetworkProtocol.KeyStringValuePair> _repeated_args_codec
        = pb::FieldCodec.ForMessage(18, global::SkyWalking.NetworkProtocol.KeyStringValuePair.Parser);
    private readonly pbc::RepeatedField<global::SkyWalking.NetworkProtocol.KeyStringValuePair> args_ = new pbc::RepeatedField<global::SkyWalking.NetworkProtocol.KeyStringValuePair>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SkyWalking.NetworkProtocol.KeyStringValuePair> Args {
      get { return args_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Command);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Command other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Command_ != other.Command_) return false;
      if(!args_.Equals(other.args_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Command_.Length != 0) hash ^= Command_.GetHashCode();
      hash ^= args_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Command_.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Command_);
      }
      args_.WriteTo(output, _repeated_args_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Command_.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Command_);
      }
      size += args_.CalculateSize(_repeated_args_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Command other) {
      if (other == null) {
        return;
      }
      if (other.Command_.Length != 0) {
        Command_ = other.Command_;
      }
      args_.Add(other.args_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Command_ = input.ReadString();
            break;
          }
          case 18: {
            args_.AddEntriesFrom(input, _repeated_args_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
