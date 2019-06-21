// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: s2clientprotocol/common.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SC2APIProtocol {

  /// <summary>Holder for reflection information generated from s2clientprotocol/common.proto</summary>
  public static partial class CommonReflection {

    #region Descriptor
    /// <summary>File descriptor for s2clientprotocol/common.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CommonReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1zMmNsaWVudHByb3RvY29sL2NvbW1vbi5wcm90bxIOU0MyQVBJUHJvdG9j",
            "b2wiPgoQQXZhaWxhYmxlQWJpbGl0eRISCgphYmlsaXR5X2lkGAEgASgFEhYK",
            "DnJlcXVpcmVzX3BvaW50GAIgASgIIlgKCUltYWdlRGF0YRIWCg5iaXRzX3Bl",
            "cl9waXhlbBgBIAEoBRIlCgRzaXplGAIgASgLMhcuU0MyQVBJUHJvdG9jb2wu",
            "U2l6ZTJESRIMCgRkYXRhGAMgASgMIh4KBlBvaW50SRIJCgF4GAEgASgFEgkK",
            "AXkYAiABKAUiVAoKUmVjdGFuZ2xlSRIiCgJwMBgBIAEoCzIWLlNDMkFQSVBy",
            "b3RvY29sLlBvaW50SRIiCgJwMRgCIAEoCzIWLlNDMkFQSVByb3RvY29sLlBv",
            "aW50SSIfCgdQb2ludDJEEgkKAXgYASABKAISCQoBeRgCIAEoAiIoCgVQb2lu",
            "dBIJCgF4GAEgASgCEgkKAXkYAiABKAISCQoBehgDIAEoAiIfCgdTaXplMkRJ",
            "EgkKAXgYASABKAUSCQoBeRgCIAEoBSpBCgRSYWNlEgoKBk5vUmFjZRAAEgoK",
            "BlRlcnJhbhABEggKBFplcmcQAhILCgdQcm90b3NzEAMSCgoGUmFuZG9tEARi",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::SC2APIProtocol.Race), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.AvailableAbility), global::SC2APIProtocol.AvailableAbility.Parser, new[]{ "AbilityId", "RequiresPoint" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.ImageData), global::SC2APIProtocol.ImageData.Parser, new[]{ "BitsPerPixel", "Size", "Data" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.PointI), global::SC2APIProtocol.PointI.Parser, new[]{ "X", "Y" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.RectangleI), global::SC2APIProtocol.RectangleI.Parser, new[]{ "P0", "P1" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.Point2D), global::SC2APIProtocol.Point2D.Parser, new[]{ "X", "Y" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.Point), global::SC2APIProtocol.Point.Parser, new[]{ "X", "Y", "Z" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SC2APIProtocol.Size2DI), global::SC2APIProtocol.Size2DI.Parser, new[]{ "X", "Y" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum Race {
    [pbr::OriginalName("NoRace")] NoRace = 0,
    [pbr::OriginalName("Terran")] Terran = 1,
    [pbr::OriginalName("Zerg")] Zerg = 2,
    [pbr::OriginalName("Protoss")] Protoss = 3,
    [pbr::OriginalName("Random")] Random = 4,
  }

  #endregion

  #region Messages
  public sealed partial class AvailableAbility : pb::IMessage<AvailableAbility> {
    private static readonly pb::MessageParser<AvailableAbility> _parser = new pb::MessageParser<AvailableAbility>(() => new AvailableAbility());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AvailableAbility> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AvailableAbility() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AvailableAbility(AvailableAbility other) : this() {
      abilityId_ = other.abilityId_;
      requiresPoint_ = other.requiresPoint_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AvailableAbility Clone() {
      return new AvailableAbility(this);
    }

    /// <summary>Field number for the "ability_id" field.</summary>
    public const int AbilityIdFieldNumber = 1;
    private int abilityId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AbilityId {
      get { return abilityId_; }
      set {
        abilityId_ = value;
      }
    }

    /// <summary>Field number for the "requires_point" field.</summary>
    public const int RequiresPointFieldNumber = 2;
    private bool requiresPoint_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool RequiresPoint {
      get { return requiresPoint_; }
      set {
        requiresPoint_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AvailableAbility);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AvailableAbility other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AbilityId != other.AbilityId) return false;
      if (RequiresPoint != other.RequiresPoint) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AbilityId != 0) hash ^= AbilityId.GetHashCode();
      if (RequiresPoint != false) hash ^= RequiresPoint.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (AbilityId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(AbilityId);
      }
      if (RequiresPoint != false) {
        output.WriteRawTag(16);
        output.WriteBool(RequiresPoint);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AbilityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AbilityId);
      }
      if (RequiresPoint != false) {
        size += 1 + 1;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AvailableAbility other) {
      if (other == null) {
        return;
      }
      if (other.AbilityId != 0) {
        AbilityId = other.AbilityId;
      }
      if (other.RequiresPoint != false) {
        RequiresPoint = other.RequiresPoint;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            AbilityId = input.ReadInt32();
            break;
          }
          case 16: {
            RequiresPoint = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  public sealed partial class ImageData : pb::IMessage<ImageData> {
    private static readonly pb::MessageParser<ImageData> _parser = new pb::MessageParser<ImageData>(() => new ImageData());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ImageData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ImageData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ImageData(ImageData other) : this() {
      bitsPerPixel_ = other.bitsPerPixel_;
      Size = other.size_ != null ? other.Size.Clone() : null;
      data_ = other.data_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ImageData Clone() {
      return new ImageData(this);
    }

    /// <summary>Field number for the "bits_per_pixel" field.</summary>
    public const int BitsPerPixelFieldNumber = 1;
    private int bitsPerPixel_;
    /// <summary>
    /// Number of bits per pixel; 8 bits for a byte etc.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int BitsPerPixel {
      get { return bitsPerPixel_; }
      set {
        bitsPerPixel_ = value;
      }
    }

    /// <summary>Field number for the "size" field.</summary>
    public const int SizeFieldNumber = 2;
    private global::SC2APIProtocol.Size2DI size_;
    /// <summary>
    /// Dimension in pixels.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SC2APIProtocol.Size2DI Size {
      get { return size_; }
      set {
        size_ = value;
      }
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 3;
    private pb::ByteString data_ = pb::ByteString.Empty;
    /// <summary>
    /// Binary data; the size of this buffer in bytes is width * height * bits_per_pixel / 8.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Data {
      get { return data_; }
      set {
        data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ImageData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ImageData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (BitsPerPixel != other.BitsPerPixel) return false;
      if (!object.Equals(Size, other.Size)) return false;
      if (Data != other.Data) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (BitsPerPixel != 0) hash ^= BitsPerPixel.GetHashCode();
      if (size_ != null) hash ^= Size.GetHashCode();
      if (Data.Length != 0) hash ^= Data.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (BitsPerPixel != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(BitsPerPixel);
      }
      if (size_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Size);
      }
      if (Data.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(Data);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (BitsPerPixel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(BitsPerPixel);
      }
      if (size_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Size);
      }
      if (Data.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ImageData other) {
      if (other == null) {
        return;
      }
      if (other.BitsPerPixel != 0) {
        BitsPerPixel = other.BitsPerPixel;
      }
      if (other.size_ != null) {
        if (size_ == null) {
          size_ = new global::SC2APIProtocol.Size2DI();
        }
        Size.MergeFrom(other.Size);
      }
      if (other.Data.Length != 0) {
        Data = other.Data;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            BitsPerPixel = input.ReadInt32();
            break;
          }
          case 18: {
            if (size_ == null) {
              size_ = new global::SC2APIProtocol.Size2DI();
            }
            input.ReadMessage(size_);
            break;
          }
          case 26: {
            Data = input.ReadBytes();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Point on the screen/minimap (e.g., 0..64).
  /// Note: bottom left of the screen is 0, 0.
  /// </summary>
  public sealed partial class PointI : pb::IMessage<PointI> {
    private static readonly pb::MessageParser<PointI> _parser = new pb::MessageParser<PointI>(() => new PointI());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PointI> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PointI() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PointI(PointI other) : this() {
      x_ = other.x_;
      y_ = other.y_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PointI Clone() {
      return new PointI(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private int x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private int y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PointI);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PointI other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0) hash ^= X.GetHashCode();
      if (Y != 0) hash ^= Y.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Y);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
      }
      if (Y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PointI other) {
      if (other == null) {
        return;
      }
      if (other.X != 0) {
        X = other.X;
      }
      if (other.Y != 0) {
        Y = other.Y;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            X = input.ReadInt32();
            break;
          }
          case 16: {
            Y = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Screen space rectangular area.
  /// </summary>
  public sealed partial class RectangleI : pb::IMessage<RectangleI> {
    private static readonly pb::MessageParser<RectangleI> _parser = new pb::MessageParser<RectangleI>(() => new RectangleI());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RectangleI> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RectangleI() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RectangleI(RectangleI other) : this() {
      P0 = other.p0_ != null ? other.P0.Clone() : null;
      P1 = other.p1_ != null ? other.P1.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RectangleI Clone() {
      return new RectangleI(this);
    }

    /// <summary>Field number for the "p0" field.</summary>
    public const int P0FieldNumber = 1;
    private global::SC2APIProtocol.PointI p0_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SC2APIProtocol.PointI P0 {
      get { return p0_; }
      set {
        p0_ = value;
      }
    }

    /// <summary>Field number for the "p1" field.</summary>
    public const int P1FieldNumber = 2;
    private global::SC2APIProtocol.PointI p1_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SC2APIProtocol.PointI P1 {
      get { return p1_; }
      set {
        p1_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RectangleI);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RectangleI other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(P0, other.P0)) return false;
      if (!object.Equals(P1, other.P1)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (p0_ != null) hash ^= P0.GetHashCode();
      if (p1_ != null) hash ^= P1.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (p0_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(P0);
      }
      if (p1_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(P1);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (p0_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(P0);
      }
      if (p1_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(P1);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RectangleI other) {
      if (other == null) {
        return;
      }
      if (other.p0_ != null) {
        if (p0_ == null) {
          p0_ = new global::SC2APIProtocol.PointI();
        }
        P0.MergeFrom(other.P0);
      }
      if (other.p1_ != null) {
        if (p1_ == null) {
          p1_ = new global::SC2APIProtocol.PointI();
        }
        P1.MergeFrom(other.P1);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (p0_ == null) {
              p0_ = new global::SC2APIProtocol.PointI();
            }
            input.ReadMessage(p0_);
            break;
          }
          case 18: {
            if (p1_ == null) {
              p1_ = new global::SC2APIProtocol.PointI();
            }
            input.ReadMessage(p1_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Point on the game board, 0..255.
  /// Note: bottom left of the screen is 0, 0.
  /// </summary>
  public sealed partial class Point2D : pb::IMessage<Point2D> {
    private static readonly pb::MessageParser<Point2D> _parser = new pb::MessageParser<Point2D>(() => new Point2D());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Point2D> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point2D() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point2D(Point2D other) : this() {
      x_ = other.x_;
      y_ = other.y_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point2D Clone() {
      return new Point2D(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private float x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private float y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Point2D);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Point2D other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0F) hash ^= X.GetHashCode();
      if (Y != 0F) hash ^= Y.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Y);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Point2D other) {
      if (other == null) {
        return;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 13: {
            X = input.ReadFloat();
            break;
          }
          case 21: {
            Y = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Point on the game board, 0..255.
  /// Note: bottom left of the screen is 0, 0.
  /// </summary>
  public sealed partial class Point : pb::IMessage<Point> {
    private static readonly pb::MessageParser<Point> _parser = new pb::MessageParser<Point>(() => new Point());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Point> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[5]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point(Point other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Point Clone() {
      return new Point(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private float x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private float y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 3;
    private float z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Point);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Point other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      if (Z != other.Z) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0F) hash ^= X.GetHashCode();
      if (Y != 0F) hash ^= Y.GetHashCode();
      if (Z != 0F) hash ^= Z.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Z);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      if (Z != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Point other) {
      if (other == null) {
        return;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
      if (other.Z != 0F) {
        Z = other.Z;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 13: {
            X = input.ReadFloat();
            break;
          }
          case 21: {
            Y = input.ReadFloat();
            break;
          }
          case 29: {
            Z = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// Screen dimensions.
  /// </summary>
  public sealed partial class Size2DI : pb::IMessage<Size2DI> {
    private static readonly pb::MessageParser<Size2DI> _parser = new pb::MessageParser<Size2DI>(() => new Size2DI());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Size2DI> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SC2APIProtocol.CommonReflection.Descriptor.MessageTypes[6]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Size2DI() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Size2DI(Size2DI other) : this() {
      x_ = other.x_;
      y_ = other.y_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Size2DI Clone() {
      return new Size2DI(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private int x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private int y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Size2DI);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Size2DI other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0) hash ^= X.GetHashCode();
      if (Y != 0) hash ^= Y.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Y);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
      }
      if (Y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Size2DI other) {
      if (other == null) {
        return;
      }
      if (other.X != 0) {
        X = other.X;
      }
      if (other.Y != 0) {
        Y = other.Y;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            X = input.ReadInt32();
            break;
          }
          case 16: {
            Y = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
