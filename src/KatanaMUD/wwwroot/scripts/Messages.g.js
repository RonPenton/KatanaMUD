var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var KMud;
(function (KMud) {
    var CommunicationMessage = (function (_super) {
        __extends(CommunicationMessage, _super);
        function CommunicationMessage() {
            _super.call(this, 'CommunicationMessage');
        }
        CommunicationMessage.ClassName = 'CommunicationMessage';
        return CommunicationMessage;
    })(KMud.MessageBase);
    KMud.CommunicationMessage = CommunicationMessage;
    var LoginRejected = (function (_super) {
        __extends(LoginRejected, _super);
        function LoginRejected() {
            _super.call(this, 'LoginRejected');
        }
        LoginRejected.ClassName = 'LoginRejected';
        return LoginRejected;
    })(KMud.MessageBase);
    KMud.LoginRejected = LoginRejected;
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
    var ServerMessage = (function (_super) {
        __extends(ServerMessage, _super);
        function ServerMessage() {
            _super.call(this, 'ServerMessage');
        }
        ServerMessage.ClassName = 'ServerMessage';
        return ServerMessage;
    })(KMud.MessageBase);
    KMud.ServerMessage = ServerMessage;
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
})(KMud || (KMud = {}));
