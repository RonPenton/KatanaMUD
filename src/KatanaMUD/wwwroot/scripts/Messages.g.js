var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var KMud;
(function (KMud) {
    var ActionNotAllowedMessage = (function (_super) {
        __extends(ActionNotAllowedMessage, _super);
        function ActionNotAllowedMessage(Message) {
            _super.call(this, 'ActionNotAllowedMessage');
            this.Message = Message;
        }
        ActionNotAllowedMessage.ClassName = 'ActionNotAllowedMessage';
        return ActionNotAllowedMessage;
    })(KMud.MessageBase);
    KMud.ActionNotAllowedMessage = ActionNotAllowedMessage;
    var ActorInformationMessage = (function (_super) {
        __extends(ActorInformationMessage, _super);
        function ActorInformationMessage(Name, Id, IsYou) {
            _super.call(this, 'ActorInformationMessage');
            this.Name = Name;
            this.Id = Id;
            this.IsYou = IsYou;
        }
        ActorInformationMessage.ClassName = 'ActorInformationMessage';
        return ActorInformationMessage;
    })(KMud.MessageBase);
    KMud.ActorInformationMessage = ActorInformationMessage;
    var AmbiguousItemMessage = (function (_super) {
        __extends(AmbiguousItemMessage, _super);
        function AmbiguousItemMessage(Items) {
            _super.call(this, 'AmbiguousItemMessage');
            this.Items = Items;
        }
        AmbiguousItemMessage.ClassName = 'AmbiguousItemMessage';
        return AmbiguousItemMessage;
    })(KMud.MessageBase);
    KMud.AmbiguousItemMessage = AmbiguousItemMessage;
    var AmbiguousActorMessage = (function (_super) {
        __extends(AmbiguousActorMessage, _super);
        function AmbiguousActorMessage(Actors) {
            _super.call(this, 'AmbiguousActorMessage');
            this.Actors = Actors;
        }
        AmbiguousActorMessage.ClassName = 'AmbiguousActorMessage';
        return AmbiguousActorMessage;
    })(KMud.MessageBase);
    KMud.AmbiguousActorMessage = AmbiguousActorMessage;
    var CommunicationMessage = (function (_super) {
        __extends(CommunicationMessage, _super);
        function CommunicationMessage(Message, Type, Chatroom, ActorName, ActorId) {
            _super.call(this, 'CommunicationMessage');
            this.Message = Message;
            this.Type = Type;
            this.Chatroom = Chatroom;
            this.ActorName = ActorName;
            this.ActorId = ActorId;
        }
        CommunicationMessage.ClassName = 'CommunicationMessage';
        return CommunicationMessage;
    })(KMud.MessageBase);
    KMud.CommunicationMessage = CommunicationMessage;
    var RemoveCommand = (function (_super) {
        __extends(RemoveCommand, _super);
        function RemoveCommand(ItemId, ItemName) {
            _super.call(this, 'RemoveCommand');
            this.ItemId = ItemId;
            this.ItemName = ItemName;
        }
        RemoveCommand.ClassName = 'RemoveCommand';
        return RemoveCommand;
    })(KMud.MessageBase);
    KMud.RemoveCommand = RemoveCommand;
    var EquipCommand = (function (_super) {
        __extends(EquipCommand, _super);
        function EquipCommand(ItemId, ItemName) {
            _super.call(this, 'EquipCommand');
            this.ItemId = ItemId;
            this.ItemName = ItemName;
        }
        EquipCommand.ClassName = 'EquipCommand';
        return EquipCommand;
    })(KMud.MessageBase);
    KMud.EquipCommand = EquipCommand;
    var ItemEquippedChangedMessage = (function (_super) {
        __extends(ItemEquippedChangedMessage, _super);
        function ItemEquippedChangedMessage(Actor, Item, Equipped) {
            _super.call(this, 'ItemEquippedChangedMessage');
            this.Actor = Actor;
            this.Item = Item;
            this.Equipped = Equipped;
        }
        ItemEquippedChangedMessage.ClassName = 'ItemEquippedChangedMessage';
        return ItemEquippedChangedMessage;
    })(KMud.MessageBase);
    KMud.ItemEquippedChangedMessage = ItemEquippedChangedMessage;
    var GenericMessage = (function (_super) {
        __extends(GenericMessage, _super);
        function GenericMessage(Message, Class) {
            _super.call(this, 'GenericMessage');
            this.Message = Message;
            this.Class = Class;
        }
        GenericMessage.ClassName = 'GenericMessage';
        return GenericMessage;
    })(KMud.MessageBase);
    KMud.GenericMessage = GenericMessage;
    var InventoryCommand = (function (_super) {
        __extends(InventoryCommand, _super);
        function InventoryCommand() {
            _super.call(this, 'InventoryCommand');
        }
        InventoryCommand.ClassName = 'InventoryCommand';
        return InventoryCommand;
    })(KMud.MessageBase);
    KMud.InventoryCommand = InventoryCommand;
    var InventoryListMessage = (function (_super) {
        __extends(InventoryListMessage, _super);
        function InventoryListMessage(Cash, TotalCash, Items, Encumbrance, MaxEncumbrance, Currency) {
            _super.call(this, 'InventoryListMessage');
            this.Cash = Cash;
            this.TotalCash = TotalCash;
            this.Items = Items;
            this.Encumbrance = Encumbrance;
            this.MaxEncumbrance = MaxEncumbrance;
            this.Currency = Currency;
        }
        InventoryListMessage.ClassName = 'InventoryListMessage';
        return InventoryListMessage;
    })(KMud.MessageBase);
    KMud.InventoryListMessage = InventoryListMessage;
    var GetItemCommand = (function (_super) {
        __extends(GetItemCommand, _super);
        function GetItemCommand(ItemId, ItemName, Quantity) {
            _super.call(this, 'GetItemCommand');
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.Quantity = Quantity;
        }
        GetItemCommand.ClassName = 'GetItemCommand';
        return GetItemCommand;
    })(KMud.MessageBase);
    KMud.GetItemCommand = GetItemCommand;
    var DropItemCommand = (function (_super) {
        __extends(DropItemCommand, _super);
        function DropItemCommand(ItemId, ItemName, Quantity, Hide) {
            _super.call(this, 'DropItemCommand');
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.Quantity = Quantity;
            this.Hide = Hide;
        }
        DropItemCommand.ClassName = 'DropItemCommand';
        return DropItemCommand;
    })(KMud.MessageBase);
    KMud.DropItemCommand = DropItemCommand;
    var ItemOwnershipMessage = (function (_super) {
        __extends(ItemOwnershipMessage, _super);
        function ItemOwnershipMessage(Items, Giver, Taker, Hide) {
            _super.call(this, 'ItemOwnershipMessage');
            this.Items = Items;
            this.Giver = Giver;
            this.Taker = Taker;
            this.Hide = Hide;
        }
        ItemOwnershipMessage.ClassName = 'ItemOwnershipMessage';
        return ItemOwnershipMessage;
    })(KMud.MessageBase);
    KMud.ItemOwnershipMessage = ItemOwnershipMessage;
    var CashTransferMessage = (function (_super) {
        __extends(CashTransferMessage, _super);
        function CashTransferMessage(Currency, Quantity, Giver, Taker, Hide) {
            _super.call(this, 'CashTransferMessage');
            this.Currency = Currency;
            this.Quantity = Quantity;
            this.Giver = Giver;
            this.Taker = Taker;
            this.Hide = Hide;
        }
        CashTransferMessage.ClassName = 'CashTransferMessage';
        return CashTransferMessage;
    })(KMud.MessageBase);
    KMud.CashTransferMessage = CashTransferMessage;
    var LoginRejected = (function (_super) {
        __extends(LoginRejected, _super);
        function LoginRejected(RejectionMessage, NoCharacter) {
            _super.call(this, 'LoginRejected');
            this.RejectionMessage = RejectionMessage;
            this.NoCharacter = NoCharacter;
        }
        LoginRejected.ClassName = 'LoginRejected';
        return LoginRejected;
    })(KMud.MessageBase);
    KMud.LoginRejected = LoginRejected;
    var LoginStateMessage = (function (_super) {
        __extends(LoginStateMessage, _super);
        function LoginStateMessage(Actor, Login) {
            _super.call(this, 'LoginStateMessage');
            this.Actor = Actor;
            this.Login = Login;
        }
        LoginStateMessage.ClassName = 'LoginStateMessage';
        return LoginStateMessage;
    })(KMud.MessageBase);
    KMud.LoginStateMessage = LoginStateMessage;
    var LookMessage = (function (_super) {
        __extends(LookMessage, _super);
        function LookMessage(Direction, Actor, Item, YouFigureItOut, Brief) {
            _super.call(this, 'LookMessage');
            this.Direction = Direction;
            this.Actor = Actor;
            this.Item = Item;
            this.YouFigureItOut = YouFigureItOut;
            this.Brief = Brief;
        }
        LookMessage.ClassName = 'LookMessage';
        return LookMessage;
    })(KMud.MessageBase);
    KMud.LookMessage = LookMessage;
    var MoveMessage = (function (_super) {
        __extends(MoveMessage, _super);
        function MoveMessage(Direction, Portal) {
            _super.call(this, 'MoveMessage');
            this.Direction = Direction;
            this.Portal = Portal;
        }
        MoveMessage.ClassName = 'MoveMessage';
        return MoveMessage;
    })(KMud.MessageBase);
    KMud.MoveMessage = MoveMessage;
    var PartyMovementMessage = (function (_super) {
        __extends(PartyMovementMessage, _super);
        function PartyMovementMessage(Leader, Actors, Direction, Enter, CustomText) {
            _super.call(this, 'PartyMovementMessage');
            this.Leader = Leader;
            this.Actors = Actors;
            this.Direction = Direction;
            this.Enter = Enter;
            this.CustomText = CustomText;
        }
        PartyMovementMessage.ClassName = 'PartyMovementMessage';
        return PartyMovementMessage;
    })(KMud.MessageBase);
    KMud.PartyMovementMessage = PartyMovementMessage;
    var PingMessage = (function (_super) {
        __extends(PingMessage, _super);
        function PingMessage(SendTime) {
            _super.call(this, 'PingMessage');
            this.SendTime = SendTime;
        }
        PingMessage.ClassName = 'PingMessage';
        return PingMessage;
    })(KMud.MessageBase);
    KMud.PingMessage = PingMessage;
    var PongMessage = (function (_super) {
        __extends(PongMessage, _super);
        function PongMessage(SendTime) {
            _super.call(this, 'PongMessage');
            this.SendTime = SendTime;
        }
        PongMessage.ClassName = 'PongMessage';
        return PongMessage;
    })(KMud.MessageBase);
    KMud.PongMessage = PongMessage;
    var RoomDescriptionMessage = (function (_super) {
        __extends(RoomDescriptionMessage, _super);
        function RoomDescriptionMessage(RoomId, Name, Description, Actors, VisibleItems, FoundItems, VisibleCash, FoundCash, Exits, IsCurrentRoom, CannotSee, CannotSeeMessage, LightLevel) {
            _super.call(this, 'RoomDescriptionMessage');
            this.RoomId = RoomId;
            this.Name = Name;
            this.Description = Description;
            this.Actors = Actors;
            this.VisibleItems = VisibleItems;
            this.FoundItems = FoundItems;
            this.VisibleCash = VisibleCash;
            this.FoundCash = FoundCash;
            this.Exits = Exits;
            this.IsCurrentRoom = IsCurrentRoom;
            this.CannotSee = CannotSee;
            this.CannotSeeMessage = CannotSeeMessage;
            this.LightLevel = LightLevel;
        }
        RoomDescriptionMessage.ClassName = 'RoomDescriptionMessage';
        return RoomDescriptionMessage;
    })(KMud.MessageBase);
    KMud.RoomDescriptionMessage = RoomDescriptionMessage;
    var SearchCommand = (function (_super) {
        __extends(SearchCommand, _super);
        function SearchCommand(Direction) {
            _super.call(this, 'SearchCommand');
            this.Direction = Direction;
        }
        SearchCommand.ClassName = 'SearchCommand';
        return SearchCommand;
    })(KMud.MessageBase);
    KMud.SearchCommand = SearchCommand;
    var SearchMessage = (function (_super) {
        __extends(SearchMessage, _super);
        function SearchMessage(FoundItems, FoundCash) {
            _super.call(this, 'SearchMessage');
            this.FoundItems = FoundItems;
            this.FoundCash = FoundCash;
        }
        SearchMessage.ClassName = 'SearchMessage';
        return SearchMessage;
    })(KMud.MessageBase);
    KMud.SearchMessage = SearchMessage;
    var ServerMessage = (function (_super) {
        __extends(ServerMessage, _super);
        function ServerMessage(Contents) {
            _super.call(this, 'ServerMessage');
            this.Contents = Contents;
        }
        ServerMessage.ClassName = 'ServerMessage';
        return ServerMessage;
    })(KMud.MessageBase);
    KMud.ServerMessage = ServerMessage;
    var SysopMessage = (function (_super) {
        __extends(SysopMessage, _super);
        function SysopMessage(Command) {
            _super.call(this, 'SysopMessage');
            this.Command = Command;
        }
        SysopMessage.ClassName = 'SysopMessage';
        return SysopMessage;
    })(KMud.MessageBase);
    KMud.SysopMessage = SysopMessage;
    var ItemDescription = (function () {
        function ItemDescription() {
        }
        return ItemDescription;
    })();
    KMud.ItemDescription = ItemDescription;
    var ActorDescription = (function () {
        function ActorDescription() {
        }
        return ActorDescription;
    })();
    KMud.ActorDescription = ActorDescription;
    var CurrencyDescription = (function () {
        function CurrencyDescription() {
        }
        return CurrencyDescription;
    })();
    KMud.CurrencyDescription = CurrencyDescription;
    var ExitDescription = (function () {
        function ExitDescription() {
        }
        return ExitDescription;
    })();
    KMud.ExitDescription = ExitDescription;
    (function (EquipmentSlot) {
        EquipmentSlot[EquipmentSlot["Weapon"] = 0] = "Weapon";
        EquipmentSlot[EquipmentSlot["Offhand"] = 1] = "Offhand";
        EquipmentSlot[EquipmentSlot["Head"] = 2] = "Head";
        EquipmentSlot[EquipmentSlot["Chest"] = 3] = "Chest";
        EquipmentSlot[EquipmentSlot["Legs"] = 4] = "Legs";
        EquipmentSlot[EquipmentSlot["Hands"] = 5] = "Hands";
        EquipmentSlot[EquipmentSlot["Feet"] = 6] = "Feet";
        EquipmentSlot[EquipmentSlot["Face"] = 7] = "Face";
        EquipmentSlot[EquipmentSlot["Arms"] = 8] = "Arms";
        EquipmentSlot[EquipmentSlot["Shoulders"] = 9] = "Shoulders";
        EquipmentSlot[EquipmentSlot["Back"] = 10] = "Back";
        EquipmentSlot[EquipmentSlot["Pocket"] = 11] = "Pocket";
        EquipmentSlot[EquipmentSlot["Waist"] = 12] = "Waist";
        EquipmentSlot[EquipmentSlot["Eyes"] = 13] = "Eyes";
        EquipmentSlot[EquipmentSlot["Ears"] = 14] = "Ears";
        EquipmentSlot[EquipmentSlot["Neck"] = 15] = "Neck";
        EquipmentSlot[EquipmentSlot["Wrists"] = 16] = "Wrists";
        EquipmentSlot[EquipmentSlot["Fingers"] = 17] = "Fingers";
        EquipmentSlot[EquipmentSlot["Light"] = 18] = "Light";
    })(KMud.EquipmentSlot || (KMud.EquipmentSlot = {}));
    var EquipmentSlot = KMud.EquipmentSlot;
    (function (CommunicationType) {
        CommunicationType[CommunicationType["Gossip"] = 0] = "Gossip";
        CommunicationType[CommunicationType["Auction"] = 1] = "Auction";
        CommunicationType[CommunicationType["Say"] = 2] = "Say";
        CommunicationType[CommunicationType["Yell"] = 3] = "Yell";
        CommunicationType[CommunicationType["Region"] = 4] = "Region";
        CommunicationType[CommunicationType["Gangpath"] = 5] = "Gangpath";
        CommunicationType[CommunicationType["Officerpath"] = 6] = "Officerpath";
        CommunicationType[CommunicationType["Chatroom"] = 7] = "Chatroom";
        CommunicationType[CommunicationType["Telepath"] = 8] = "Telepath";
    })(KMud.CommunicationType || (KMud.CommunicationType = {}));
    var CommunicationType = KMud.CommunicationType;
    (function (Direction) {
        Direction[Direction["North"] = 0] = "North";
        Direction[Direction["South"] = 1] = "South";
        Direction[Direction["East"] = 2] = "East";
        Direction[Direction["West"] = 3] = "West";
        Direction[Direction["Northeast"] = 4] = "Northeast";
        Direction[Direction["Northwest"] = 5] = "Northwest";
        Direction[Direction["Southeast"] = 6] = "Southeast";
        Direction[Direction["Southwest"] = 7] = "Southwest";
        Direction[Direction["Up"] = 8] = "Up";
        Direction[Direction["Down"] = 9] = "Down";
    })(KMud.Direction || (KMud.Direction = {}));
    var Direction = KMud.Direction;
    (function (LightLevel) {
        LightLevel[LightLevel["Daylight"] = 50] = "Daylight";
        LightLevel[LightLevel["Nothing"] = -10000] = "Nothing";
        LightLevel[LightLevel["PitchBlack"] = -500] = "PitchBlack";
        LightLevel[LightLevel["VeryDark"] = -250] = "VeryDark";
        LightLevel[LightLevel["BarelyVisible"] = -200] = "BarelyVisible";
        LightLevel[LightLevel["DimlyLit"] = -150] = "DimlyLit";
        LightLevel[LightLevel["RegularLight"] = -50] = "RegularLight";
    })(KMud.LightLevel || (KMud.LightLevel = {}));
    var LightLevel = KMud.LightLevel;
})(KMud || (KMud = {}));
