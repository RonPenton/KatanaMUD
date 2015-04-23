var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var KMud;
(function (KMud) {
    var ActorInformationMessage = (function (_super) {
        __extends(ActorInformationMessage, _super);
        function ActorInformationMessage() {
            _super.call(this, 'ActorInformationMessage');
        }
        ActorInformationMessage.ClassName = 'ActorInformationMessage';
        return ActorInformationMessage;
    })(KMud.MessageBase);
    KMud.ActorInformationMessage = ActorInformationMessage;
    var CommunicationMessage = (function (_super) {
        __extends(CommunicationMessage, _super);
        function CommunicationMessage() {
            _super.call(this, 'CommunicationMessage');
        }
        CommunicationMessage.ClassName = 'CommunicationMessage';
        return CommunicationMessage;
    })(KMud.MessageBase);
    KMud.CommunicationMessage = CommunicationMessage;
    var InventoryMessage = (function (_super) {
        __extends(InventoryMessage, _super);
        function InventoryMessage() {
            _super.call(this, 'InventoryMessage');
        }
        InventoryMessage.ClassName = 'InventoryMessage';
        return InventoryMessage;
    })(KMud.MessageBase);
    KMud.InventoryMessage = InventoryMessage;
    var InventoryListMessage = (function (_super) {
        __extends(InventoryListMessage, _super);
        function InventoryListMessage() {
            _super.call(this, 'InventoryListMessage');
        }
        InventoryListMessage.ClassName = 'InventoryListMessage';
        return InventoryListMessage;
    })(KMud.MessageBase);
    KMud.InventoryListMessage = InventoryListMessage;
    var GetItemMessage = (function (_super) {
        __extends(GetItemMessage, _super);
        function GetItemMessage() {
            _super.call(this, 'GetItemMessage');
        }
        GetItemMessage.ClassName = 'GetItemMessage';
        return GetItemMessage;
    })(KMud.MessageBase);
    KMud.GetItemMessage = GetItemMessage;
    var DropItemMessage = (function (_super) {
        __extends(DropItemMessage, _super);
        function DropItemMessage() {
            _super.call(this, 'DropItemMessage');
        }
        DropItemMessage.ClassName = 'DropItemMessage';
        return DropItemMessage;
    })(KMud.MessageBase);
    KMud.DropItemMessage = DropItemMessage;
    var LoginRejected = (function (_super) {
        __extends(LoginRejected, _super);
        function LoginRejected() {
            _super.call(this, 'LoginRejected');
        }
        LoginRejected.ClassName = 'LoginRejected';
        return LoginRejected;
    })(KMud.MessageBase);
    KMud.LoginRejected = LoginRejected;
    var LookMessage = (function (_super) {
        __extends(LookMessage, _super);
        function LookMessage() {
            _super.call(this, 'LookMessage');
        }
        LookMessage.ClassName = 'LookMessage';
        return LookMessage;
    })(KMud.MessageBase);
    KMud.LookMessage = LookMessage;
    var MoveMessage = (function (_super) {
        __extends(MoveMessage, _super);
        function MoveMessage() {
            _super.call(this, 'MoveMessage');
        }
        MoveMessage.ClassName = 'MoveMessage';
        return MoveMessage;
    })(KMud.MessageBase);
    KMud.MoveMessage = MoveMessage;
    var PingMessage = (function (_super) {
        __extends(PingMessage, _super);
        function PingMessage() {
            _super.call(this, 'PingMessage');
        }
        PingMessage.ClassName = 'PingMessage';
        return PingMessage;
    })(KMud.MessageBase);
    KMud.PingMessage = PingMessage;
    var PongMessage = (function (_super) {
        __extends(PongMessage, _super);
        function PongMessage() {
            _super.call(this, 'PongMessage');
        }
        PongMessage.ClassName = 'PongMessage';
        return PongMessage;
    })(KMud.MessageBase);
    KMud.PongMessage = PongMessage;
    var RoomDescriptionMessage = (function (_super) {
        __extends(RoomDescriptionMessage, _super);
        function RoomDescriptionMessage() {
            _super.call(this, 'RoomDescriptionMessage');
        }
        RoomDescriptionMessage.ClassName = 'RoomDescriptionMessage';
        return RoomDescriptionMessage;
    })(KMud.MessageBase);
    KMud.RoomDescriptionMessage = RoomDescriptionMessage;
    var ServerMessage = (function (_super) {
        __extends(ServerMessage, _super);
        function ServerMessage() {
            _super.call(this, 'ServerMessage');
        }
        ServerMessage.ClassName = 'ServerMessage';
        return ServerMessage;
    })(KMud.MessageBase);
    KMud.ServerMessage = ServerMessage;
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
    var ExitDescription = (function () {
        function ExitDescription() {
        }
        return ExitDescription;
    })();
    KMud.ExitDescription = ExitDescription;
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
})(KMud || (KMud = {}));
