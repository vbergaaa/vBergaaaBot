using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Collections;
using SC2APIProtocol;

// ReSharper disable MemberCanBePrivate.Global

namespace Bot {
    public class Unit {
        public SC2APIProtocol.Unit original;
        private UnitTypeData unitTypeData;

        public string name;
        public uint unitType;
        public float integrity;
        public Vector3 position;
        public ulong tag;
        public float buildProgress;
        public UnitOrder order;
        public RepeatedField<UnitOrder> orders;
        public int supply;
        public bool isVisible;
        public int idealWorkers;
        public int assignedWorkers;
        public Point2D Pos;
        public int Owner;
        public float Energy;
        public ulong Tag;

        public Unit(SC2APIProtocol.Unit unit) {
            this.original = unit;
            this.unitTypeData = Controller.gameData.Units[(int) unit.UnitType];

            this.name = unitTypeData.Name;
            this.tag = unit.Tag;
            this.unitType = unit.UnitType;
            this.position = new Vector3(unit.Pos.X, unit.Pos.Y, unit.Pos.Z);
            this.integrity = (unit.Health + unit.Shield) / (unit.HealthMax + unit.ShieldMax);
            this.buildProgress = unit.BuildProgress;
            this.idealWorkers = unit.IdealHarvesters;
            this.assignedWorkers = unit.AssignedHarvesters;
            this.Pos = new Point2D { X = unit.Pos.X, Y = unit.Pos.Y };
            this.Owner = unit.Owner;
            this.Energy = unit.Energy;
            this.Tag = unit.Tag;
            
            this.order = unit.Orders.Count > 0 ? unit.Orders[0] : new UnitOrder();
            this.orders = unit.Orders;
            this.isVisible = (unit.DisplayType == DisplayType.Visible);

            this.supply = (int) unitTypeData.FoodRequired;
        }                        
        
        
        public double GetDistance(Unit otherUnit) {
            return Vector3.Distance(position, otherUnit.position);
        }

        public double GetDistance(Vector3 location) {
            return Vector3.Distance(position, location);
        }
        
        public void Train(uint unitType, bool queue=false) {            
            if (!queue && orders.Count > 0)
                return;            

            var abilityID = Abilities.GetID(unitType);            
            var action = Controller.CreateRawUnitCommand(abilityID);
            action.ActionRaw.UnitCommand.UnitTags.Add(tag);
            Controller.AddAction(action);

            var targetName = Controller.GetUnitName(unitType);
            Logger.Info("Started training: {0}", targetName);
        }
        
        private void FocusCamera() {
            var action = new Action();
            action.ActionRaw = new ActionRaw();
            action.ActionRaw.CameraMove = new ActionRawCameraMove();
            action.ActionRaw.CameraMove.CenterWorldSpace = new Point();
            action.ActionRaw.CameraMove.CenterWorldSpace.X = position.X;
            action.ActionRaw.CameraMove.CenterWorldSpace.Y = position.Y;
            action.ActionRaw.CameraMove.CenterWorldSpace.Z = position.Z;            
            Controller.AddAction(action);
        }
        
        
        public void Move(Point2D target) {
            var action = Controller.CreateRawUnitCommand(Abilities.MOVE);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            action.ActionRaw.UnitCommand.UnitTags.Add(tag);
            Controller.AddAction(action);
        }
        
        public void Smart(Unit unit) {
            var action = Controller.CreateRawUnitCommand(Abilities.SMART);
            action.ActionRaw.UnitCommand.TargetUnitTag = unit.tag;
            action.ActionRaw.UnitCommand.UnitTags.Add(tag);
            Controller.AddAction(action);
        }


        
    }
}